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

    private GameObject loadingIcon;

    private void Awake() 
    {
        jsonManager = GameObject.Find("Json Manager").GetComponent<JsonManager>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        fade = GetComponentInChildren<Fade>();

        saves = jsonManager.Load<SaveData>("SaveData", "Save.json");

        loadingIcon = GameObject.Find("Loading Icon");
        loadingIcon.SetActive(false);        
    }

    private void Start() 
    {
        StartCoroutine(CoFirstFade());
    }

    private IEnumerator CoFirstFade()
    {
        fade.FadeOut(3.0f);
        yield return new WaitForSeconds(3.0f);
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

    public void LoadScene03()
    {
        StartCoroutine(CoLoadScene("Stage 03"));
    }

    private IEnumerator CoLoadScene(string stageName)
    {
        soundManager.PlaySfx(soundManager.EffectSounds[0]);

        fade.transform.SetAsLastSibling();

        fade.FadeIn(3.0f);
        yield return new WaitForSeconds(3.0f);
        
        loadingIcon.SetActive(true); 
        yield return new WaitForSeconds(4.0f); 

        SceneManager.LoadScene(stageName);
    }
}