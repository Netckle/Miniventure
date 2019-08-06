using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene03 : MonoBehaviour
{
    public Fade fade;

    public PlayerMovement player;
    // public BatMovement bat;

    private BackgroundScroll backgroundScroll;

    public DialogueManager dialogueManager;

    private JsonManager jsonManager;
    private PauseManager pauseManager;
    private SoundManager soundManager;

    private bool phase02canLoad = true;

    public Transform playerOriginPos;
    public Transform minoOriginPos;

    private BossBatMovement bossBat;

    [Space]
    [Header("전투 전 대화 범위")]
    public int startNum01, endNum01;
    [Space]
    [Header("2 페이즈 전 대화 범위")]
    public int startNum02, endNum02;
    [Space]
    [Header("전투 후 대화 범위")]
    public int startNum03, endNum03;

    private void Start() 
    {
        bossBat = GameObject.Find("Boss Bat").GetComponent<BossBatMovement>();

        backgroundScroll = GameObject.Find("Background Scroll").GetComponent<BackgroundScroll>();

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        jsonManager  = GameObject.Find("Json Manager").GetComponent<JsonManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();

        StartCoroutine(FirstCutscene());
    }

    private IEnumerator FirstCutscene()
    {
        soundManager.SimplePlayBGM(0);
        player.Pause();

        fade.FadeOut(3.0f);
        yield return new WaitForSeconds(3.0f);
        fade.transform.SetAsFirstSibling();

        dialogueManager.SetActivePanel(true);

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), startNum01, endNum01);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        player.Release();
        

        backgroundScroll.Move();
        bossBat.StartBossMove();
    }
}
