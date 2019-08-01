using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene02 : MonoBehaviour
{
    public Fade fade;

    public PlayerMovement player;
    public MoveMino mino;

    public DialogueManager dialogueManager;
    public JsonManager jsonManager;

    public bool cutsceneIsEnd;
    public bool phase_02_can_load = true;
    public bool phase_02_end_can_laod = true;
    public bool bossIsDead;

    public Transform phase02Pos;
    public StageController stageController;  

    bool middlePhaseCanOn = true;
    bool endCanOn = true;

    PauseManager pauseManager;

    SoundManager soundManager;

    public TreeMove treeMove;

    void Start()
    {
        player.stage_02_doubleJump = true;
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        if (!cutsceneIsEnd)
        {
            dialogueManager.panel.gameObject.SetActive(false);
            dialogueManager.namePanel.gameObject.SetActive(false);
            StartCoroutine(CoFirstCutscene(2, 5));
        }            
    }
    
    private void Update() 
    {
        if (mino.HP == 4 && phase_02_can_load)
        {
            phase_02_can_load = false;
            StartCoroutine(CoBetweenCutscene(2, 5));
        }
        if (mino.HP <= 0 && phase_02_end_can_laod)
        {
            phase_02_end_can_laod = false;
            //jsonManager.Save(2, true);
            
            mino.StopAllCoroutines();
            mino.Die();
        }
    }

    public StageController02 stage_controller_02;
    public bool phase_02_start;

    IEnumerator CoBetweenCutscene(int dialogueStart, int dialogueEnd)
    {
        player.Pause();
        mino.StopAllCoroutines();
        mino.sprite.color = new Color32(255, 255, 255, 255);
        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        for (int i = 0; i < 3; i++)
        {
            mino.PlayAttack();           
            
            yield return new WaitForSeconds(0.8f);
            soundManager.PlaySfx(soundManager.EffectSounds[0]);

            stage_controller_02.particle.Play();
        }

        soundManager.PlaySfx(soundManager.EffectSounds[4]);

        //treeMove.StopAllTree();
        stage_controller_02.phase_01_floor.SetActive(false);

        mino.FreeRigid();
              
        // 바닥 트리거에 닿았을 경우.

        yield return new WaitUntil(()=>phase_02_start);

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        player.Release();
        mino.StartBossPattern02(); // 2페이즈
    }

    IEnumerator CoFirstCutscene(int dialogueStart, int dialogueEnd)
    {        
        cutsceneIsEnd = false;

        soundManager.SimplePlayBGM(0);

        fade.FadeOut(3.0f);
        yield return new WaitForSeconds(3.0f);
        fade.transform.SetAsFirstSibling();

        player.Pause();
        //player.ForcePlayWalkAnim();
        //player.ChangeTransform(bossSlime.gameObject.transform.position + new Vector3(-5, 1, 0));
       
        dialogueManager.panel.gameObject.SetActive(true);
        dialogueManager.namePanel.gameObject.SetActive(true);

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);
        
        //Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;

        player.Release();  

        mino.StartBossPattern();
        cutsceneIsEnd = true;
    }

    
}
