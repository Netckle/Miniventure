using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectStage : MonoBehaviour 
{
    public Button[] buttons;

    private JsonManager jsonManager;
    private SoundManager soundManager;

    private SaveData[] saves;

    private Fade fade;

    private WaitForSeconds waitTime = new WaitForSeconds(2.0f);

    private void Start() 
    {
        jsonManager = GameObject.Find("Json Manager").GetComponent<JsonManager>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        fade = GetComponentInChildren<Fade>();

        saves = jsonManager.Load<SaveData>("SaveData", "Save.json");

        StartCoroutine(CoFirstFade());
    }

    private IEnumerator CoFirstFade()
    {
        fade.FadeOut(2.0f);
        yield return waitTime;
        fade.transform.SetAsFirstSibling();
    }

    private void Update() 
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(saves[i].stageClear)
            {
                ColorBlock cb = buttons[i].colors;
                Color newColor = Color.gray;

                cb.normalColor = newColor;
                buttons[i].colors = cb;
            }
            else if (!saves[i].stageClear)
            {
                ColorBlock cb = buttons[i].colors;
                Color newColor = Color.white;

                cb.normalColor = newColor;
                buttons[i].colors = cb;
            }
        }
    }

    public void LoadScene01()
    {
        StartCoroutine(CoLoadScene("Stage 01"));
    }

    public void LoadScene02()
    {
        StartCoroutine(CoLoadScene("Stage 02"));
    }

    private IEnumerator CoLoadScene(string stageName)
    {
        soundManager.PlaySfx(soundManager.EffectSounds[0]);

        fade.transform.SetAsLastSibling();
        fade.FadeIn(2.0f);

        yield return waitTime;

        SceneManager.LoadScene(stageName);
    }
}