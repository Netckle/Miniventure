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
    public bool bossIsDead;

    public Transform phase02Pos;
    public StageController stageController;  

    bool middlePhaseCanOn = true;
    bool endCanOn = true;

    PauseManager pauseManager;

    public TreeMove treeMove;

    void Start()
    {
        //dialogueManager = GameObject.Find("Dialogue  Manager").GetComponent<DialogueManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();

        if (!cutsceneIsEnd)
        {
            StartCoroutine(CoFirstCutscene(2, 5));
        }            
    }
    
    private void Update() {
        if (mino.HP == 4 && phase_02_can_load)
        {
            phase_02_can_load = false;
            StartCoroutine(CoBetweenCutscene(2, 5));
        }
        
    }

    public StageController02 stage_controller_02;
    public bool phase_02_start;

    IEnumerator CoBetweenCutscene(int dialogueStart, int dialogueEnd)
    {
        player.Pause();
        mino.StopAllCoroutines();
        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>(), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        for (int i = 0; i < 3; i++)
        {
            mino.PlayAttack();
            stage_controller_02.particle.Play();
            yield return new WaitForSeconds(0.8f);
        }

        treeMove.StopAllTree();
        stage_controller_02.phase_01_floor.SetActive(false);
              
        // 바닥 트리거에 닿았을 경우.

        yield return new WaitUntil(()=>phase_02_start);

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>(), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        player.Release();
        //mino.StartBossPattern(); // 2페이즈
    }

    IEnumerator CoFirstCutscene(int dialogueStart, int dialogueEnd)
    {        
        cutsceneIsEnd = false;

        fade.FadeOut(3.0f);

        player.Pause();
        //player.ForcePlayWalkAnim();
        //player.ChangeTransform(bossSlime.gameObject.transform.position + new Vector3(-5, 1, 0));
       
        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>(), dialogueStart, dialogueEnd);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);
        
        //Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;

        player.Release();  

        mino.StartBossPattern();
        cutsceneIsEnd = true;
    }
}
