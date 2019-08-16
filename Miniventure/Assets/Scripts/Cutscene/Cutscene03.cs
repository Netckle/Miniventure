using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    [Header("전투 후 대화 범위")]
    public int startNum03, endNum03;

    private void Start() 
    {
        bossBat = GameObject.Find("Bat Boss").GetComponent<BossBatMovement>();

        backgroundScroll = GameObject.Find("Background Scroll").GetComponent<BackgroundScroll>();

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        jsonManager  = GameObject.Find("Json Manager").GetComponent<JsonManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();

        StartCoroutine(FirstCutscene());
    }

    bool canFinish = true;

    private void Update() 
    {
        if (bossBat.HP == 0 && canFinish)
        {
            canFinish = false;
            StartCoroutine(LastCutscene());
        }
    }

    

    private IEnumerator LastCutscene()
    {
        player.Pause();
        bossBat.transform.DOPause();
        bossBat.StopAllCoroutines();

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), startNum01, endNum01);
        yield return new WaitUntil(()=> dialogueManager.dialogueIsEnd);

        bossBat.Die();
        yield return new WaitUntil(()=> bossBat.isDead);

        fade.transform.SetAsFirstSibling();
        fade.FadeIn(3.0f);
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("Main");
    }

    private IEnumerator FirstCutscene()
    {
        soundManager.SimplePlayBGM(0);
        player.Pause();

        backgroundScroll.Stop();

        fade.FadeOut(3.0f);
        yield return new WaitForSeconds(3.0f);
        fade.transform.SetAsFirstSibling();

        dialogueManager.SetActivePanel(true);

        dialogueManager.StartDialogue(jsonManager.Load<Dialogue>("JsonData", "Dialogue.json"), startNum03, endNum03);
        yield return new WaitUntil(() => dialogueManager.dialogueIsEnd);

        player.Release();
        

        backgroundScroll.Move();
        bossBat.StartBossMove();
    }
}
