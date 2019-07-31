using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene01 : Cutscene
{
    public Fade fade;

    public PlayerMovement player;
    public MoveSlimeDot bossSlime;

    public DialogueManager dialogueManager;
    public JsonManager jsonManager;
   
    public bool cutsceneIsEnd;
    public bool phase02start;
    public bool bossIsDead;

    public Transform phase02Pos;
    public StageController stageController;  

    bool middlePhaseCanOn = true;
    bool endCanOn = true;

    PauseManager pauseManager;
    private SoundManager soundManager;

    void Start()
    {
        //dialogueManager = GameObject.Find("Dialogue  Manager").GetComponent<DialogueManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        if (!cutsceneIsEnd)
        {
            StartCoroutine(CoFirstCutscene(2, 5));
        }            
    }    

    void Update()
    {
        if (phase02start)
        {          
            phase02start = false;  
            StartCoroutine(CoLastCutscene(7, 8));            
        }

        if (middlePhaseCanOn && bossSlime.HP == 5)
        {
            middlePhaseCanOn = false;
            StartCoroutine(BetweenPhase());
        } 

        if (endCanOn && bossSlime.HP <= 0)
        {
            endCanOn = false;
            StartCoroutine(EndCutscene(13, 14));
        }       
    }

    IEnumerator BetweenPhase()
    {
        player.pause = true;
        stageController.AllMiniSlimeFalse();
        bossSlime.StopCor();
        //player.transform.position = bossSlime.transform.position + new Vector3(-4, 0, 0);

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), 5, 6);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        bossSlime.transform.position = new Vector3(0, bossSlime.transform.position.y, bossSlime.transform.position.z);
        fade.transform.SetAsLastSibling();
        fade.FadeIn(3.0f);

        yield return new WaitForSeconds(3.0f);

        // 발판 제거
        stageController.phase01Block.SetActive(false);

        fade.FadeOut(3.0f);
        yield return new WaitForSeconds(3.0f);
        fade.transform.SetAsFirstSibling();

        player.pause = false;

        //bossIsDead = true;
        
    }

    IEnumerator CoFirstCutscene(int dialogueStart, int dialogueEnd)
    {        
        cutsceneIsEnd = false;
        
        soundManager.SimplePlayBGM(0);
        
        dialogueManager.panel.gameObject.SetActive(false);
        dialogueManager.namePanel.gameObject.SetActive(false);

        player.Pause();
        player.ChangeTransform(bossSlime.gameObject.transform.position + new Vector3(-5, 1, 0));

        fade.FadeOut(3.0f);       

        yield return new WaitForSeconds(3.0f);
        fade.transform.SetAsFirstSibling();

        dialogueManager.panel.gameObject.SetActive(true);
        dialogueManager.namePanel.gameObject.SetActive(true);
       
        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);
        
        //Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;

        player.Release();  

        bossSlime.StartBossMove(1);
        cutsceneIsEnd = true;
    }

    IEnumerator CoLastCutscene(int dialogueStart, int dialogueEnd)
    {
        

        cutsceneIsEnd = false;

        //fade.FadeIn(3.0f);
        //fade.FadeOut(3.0f);

        //bossSlime.transform.position = phase02Pos.position;  
        bossSlime.StopCor();

        yield return new WaitForSeconds(0.5f);    

        player.Pause();
        player.ChangeTransform(bossSlime.transform.position + new Vector3(-7, 1, 0));    

        //Camera.main.GetComponent<MultipleTargetCamera>().targets[0] = player.gameObject.transform; 
        //Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;   

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        player.Release();

        yield return new WaitForSeconds(2.0f);

        bossSlime.StartBossMove(2);
        cutsceneIsEnd = true;
    }

    IEnumerator EndCutscene(int start, int end)
    {
        //jsonManager.Save(1, true);

        bossSlime.StopCor();
        cutsceneIsEnd = false;
        player.Pause();

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), start, end);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        bossSlime.Die();
        soundManager.PlaySfx(soundManager.EffectSounds[4]);
        yield return new WaitUntil(()=>bossSlime.isDie);

        cutsceneIsEnd = true;
    }
}
