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
    public bool cutsceneIsEnd;
    public bool bossIsDead;

    void Start()
    {
        if (!cutsceneIsEnd)
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
        cutsceneIsEnd = false;

        fade.FadeOut(3.0f);

        player.Pause();
        player.ChangeTransform(npc.gameObject.transform.position + new Vector3(-5, 1, 0));
       
        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), dialogueStart, dialogueEnd);

        yield return new WaitUntil(() => dialogueIsEnd);
        
        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;

        player.Release();  

        yield return new WaitForSeconds(2.0f);

        npc.gameObject.SetActive(false);

        bossSlime.StartBossMove(1);

        cutsceneIsEnd = true;
    }

    IEnumerator CoLastCutscene(int dialogueStart, int dialogueEnd)
    {
        // Fade In Out 

        cutsceneIsEnd = false;

        npc.gameObject.SetActive(true);

        player.Pause();
        player.ChangeTransform(npc.gameObject.transform.position + new Vector3(-5, 1, 0));

        

        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), dialogueStart, dialogueEnd);

        yield return new WaitUntil(() => dialogueIsEnd);

        cutsceneIsEnd = true;
        // Fade In Out 

        bossSlime.GetComponent<Collider2D>().isTrigger = true;

        yield return new WaitForSeconds(0.7f);

        bossSlime.GetComponent<Collider2D>().isTrigger = false;

        bossSlime.StartBossMove(2);

        cutsceneIsEnd = true;
    }
}
