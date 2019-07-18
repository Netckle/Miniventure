using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene01 : Cutscene
{
    public Fade fade;

    public PlayerMovement player;
    public MoveSlimeDot bossSlime;
   
    public bool cutsceneIsEnd;
    public bool bossIsDead;

    public Transform phase02Pos;
    public StageController stageController;  

    bool middlePhaseCanOn = true;

    void Start()
    {
        if (!cutsceneIsEnd)
        {
            StartCoroutine(CoFirstCutscene(0, 6));
        }            
    }

    

    void Update()
    {
        if (bossIsDead)
        {          
            bossIsDead = false;  
            StartCoroutine(CoLastCutscene(7, 10));
            
        }

        if (middlePhaseCanOn && bossSlime.HP == 5)
        {
            middlePhaseCanOn = false;
            StartCoroutine(BetweenPhase());
        }
    }

    IEnumerator BetweenPhase()
    {
        bossSlime.transform.position = new Vector3(0, bossSlime.transform.position.y, bossSlime.transform.position.z);
        bossSlime.StopCor();
        //player.transform.position = bossSlime.transform.position + new Vector3(-4, 0, 0);

        DialogueManager.instance.StartDialogue(JsonManager.instance.Load<Dialogue>(), 11, 12);
        yield return new WaitUntil(() => DialogueManager.instance.dialogueIsEnd);

        fade.FadeIn(3.0f);

        yield return new WaitForSeconds(3.0f);

        // 발판 제거
        stageController.phase01Block.SetActive(false);

        fade.FadeOut(3.0f);

        //bossIsDead = true;
        
    }

    IEnumerator CoFirstCutscene(int dialogueStart, int dialogueEnd)
    {        
        cutsceneIsEnd = false;

        fade.FadeOut(3.0f);

        player.Pause();
        player.ChangeTransform(bossSlime.gameObject.transform.position + new Vector3(-5, 1, 0));
       
        DialogueManager.instance.StartDialogue(JsonManager.instance.Load<Dialogue>(), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => DialogueManager.instance.dialogueIsEnd);
        
        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;

        player.Release();  

        bossSlime.StartBossMove(1);
        cutsceneIsEnd = true;
    }

    IEnumerator CoLastCutscene(int dialogueStart, int dialogueEnd)
    {
        stageController.AllMiniSlimeFalse();

        cutsceneIsEnd = false;

        //fade.FadeIn(3.0f);
        //fade.FadeOut(3.0f);

        bossSlime.transform.position = phase02Pos.position;  
        bossSlime.StopCor();

        yield return new WaitForSeconds(0.5f);    

        player.Pause();
        player.ChangeTransform(bossSlime.transform.position + new Vector3(-5, 1, 0));    

        Camera.main.GetComponent<MultipleTargetCamera>().targets[0] = player.gameObject.transform; 
        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;   

        DialogueManager.instance.StartDialogue(JsonManager.instance.Load<Dialogue>(), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => DialogueManager.instance.dialogueIsEnd);

        player.Release();

        yield return new WaitForSeconds(2.0f);

        bossSlime.StartBossMove(2);
        cutsceneIsEnd = true;
    }
}
