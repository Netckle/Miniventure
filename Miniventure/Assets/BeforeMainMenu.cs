using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class BeforeMainMenu : MonoBehaviour
{
    private Fade fade;
    private SoundManager soundManager;

    public SpriteRenderer playerSprite;

    public TextMeshProUGUI TMP;    
    public string titleText;

    public TextMeshProUGUI tmpForTip; 
    public string[] tips = new string[3];

    void Start()
    {
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        StartCoroutine(BeforeTitleScreen());
    }

    private IEnumerator BeforeTitleScreen()
    {
        TMP.text = "";

        foreach (char letter in titleText)
        {       
            TMP.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        soundManager.PlaySfx(soundManager.EffectSounds[0]);

        yield return new WaitForSeconds(3.0f);

        TMP.text = "";
        
        soundManager.SimplePlayBGM(0);
        
        fade.FadeOut(3.0f);
        yield return new WaitForSeconds(3.0f);
        fade.transform.SetAsFirstSibling();        
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

        playerSprite.gameObject.SetActive(true);

        tmpForTip.gameObject.SetActive(true);
        tmpForTip.text = tips[Random.Range(0, 3)];

        yield return new WaitForSeconds(4.0f);

        SceneManager.LoadScene("Select Stage");
    }
}
