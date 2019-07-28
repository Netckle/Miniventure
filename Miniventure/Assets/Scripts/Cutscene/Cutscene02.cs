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
    public bool phase02start;
    public bool bossIsDead;

    public Transform phase02Pos;
    public StageController stageController;  

    bool middlePhaseCanOn = true;
    bool endCanOn = true;

    PauseManager pauseManager;

    void Start()
    {
        //dialogueManager = GameObject.Find("Dialogue  Manager").GetComponent<DialogueManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();

        if (!cutsceneIsEnd)
        {
            StartCoroutine(CoFirstCutscene(2, 5));
        }            
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
