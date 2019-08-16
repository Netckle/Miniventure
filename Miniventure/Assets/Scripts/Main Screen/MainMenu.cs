using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class MainMenu : MonoBehaviour
{
    private Fade fade;
    private SoundManager soundManager;

    public TextMeshProUGUI titleText;    
    public string content;
    public string afterContent;

    void Start()
    {
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        StartCoroutine(BeforeMainScreen());
    }

    private IEnumerator BeforeMainScreen()
    {
        titleText.text = "";

        foreach (char letter in content)
        {       
            titleText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        soundManager.PlaySfx(soundManager.EffectSounds[0]);

        yield return new WaitForSeconds(3.0f);

        titleText.text = "";
        
        soundManager.SimplePlayBGM(0);
        
        fade.FadeOut(3.0f);
        yield return new WaitForSeconds(3.0f);
        fade.transform.SetAsFirstSibling();   

        foreach (char letter in afterContent)
        {       
            titleText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }     
    }

    public void OnClickStart()
    {
        StartCoroutine(CoOnClickStart());
    }

    private IEnumerator CoOnClickStart()
    {
        soundManager.PlaySfx(soundManager.EffectSounds[1]);

        fade.transform.SetAsLastSibling();
        fade.FadeIn(3.0f);
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("Loading Stage 00");
    }
}
