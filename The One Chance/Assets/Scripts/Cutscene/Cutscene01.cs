using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene01 : Cutscene
{
    public Fade fade;

    public PlayerMovement player;
    public NPCMovement npc;
    public BossMovement bossSlime;

    public GameObject niddle;

    bool bossIsDead = false;
    bool cutscne01IsEnd, cutscne02IsEnd;   

    void Start()
    {
        if (!cutscne01IsEnd)
        {
            StartCoroutine(CoFirstCutscene(0, 6));
        }            
    }

    public void BossIsDead()
    {
        bossIsDead = true;
    }

    void Update()
    {
        if (bossIsDead)
        {            
            StartCoroutine(CoLastCutscene(7, 10));
            bossIsDead = false;
        }
    }

    IEnumerator CoFirstCutscene(int dialogueStart, int dialogueEnd)
    {        
        fade.FadeOut(3.0f);

        player.Pause();
        player.ChangeTransform(npc.gameObject.transform.position + new Vector3(-5, 1, 0));
       
        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), dialogueStart, dialogueEnd);

        yield return new WaitUntil(() => dialogueIsEnd);
        
        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;

        player.Release();  

        yield return new WaitForSeconds(2.0f);

        bossSlime.StartBossMove(1);

        cutscne01IsEnd = true;
    }

    IEnumerator CoLastCutscene(int dialogueStart, int dialogueEnd)
    {
        // Fade In Out 

        player.Pause();
        player.ChangeTransform(npc.gameObject.transform.position + new Vector3(-5, 1, 0));

        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), dialogueStart, dialogueEnd);

        yield return new WaitUntil(() => dialogueIsEnd);

        // Fade In Out 
    }
}
