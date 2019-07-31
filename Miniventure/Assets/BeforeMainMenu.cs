using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BeforeMainMenu : MonoBehaviour
{
    public Image logoImage;
    public RectTransform titleImage;

    public Button startButton;

    public float fadeInTime, fadeOutTime, betweenWaitTime;

    private Fade fade;
    private bool logoFadeInOutIsEnd = false;

    private SoundManager soundManager;

    private bool moveIsEnd = false;

    void Start()
    {
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        StartCoroutine(FadeInOutLogo(fadeInTime, fadeOutTime, betweenWaitTime));
    }

    private IEnumerator FadeInOutLogo(float fadeInTime, float fadeOutTime, float betweenWaitTime)
    {
        logoFadeInOutIsEnd = false;

        fade.FadeIn(fadeInTime, null, logoImage);
        yield return new WaitForSeconds(fadeInTime);

        soundManager.PlaySfx(soundManager.EffectSounds[0]);

        yield return new WaitForSeconds(betweenWaitTime);

        fade.FadeOut(fadeOutTime, null, logoImage);
        yield return new WaitForSeconds(fadeOutTime);

        fade.FadeOut(fadeOutTime);
        fade.transform.SetAsFirstSibling();

        soundManager.PlaySfx(soundManager.BGMSounds[0], true);

        //Vector3 pos = startButton.gameObject.transform.position;
        //Debug.Log(pos);
        MoveTitleImage(250, 6.0f);

        logoFadeInOutIsEnd = true;
    }

    private void MoveTitleImage(float moveDistance, float moveTime)
    {  
        moveIsEnd = false;

        float endPos = moveDistance;
        titleImage.DOAnchorPosY(endPos, moveTime)
            .SetEase(Ease.InOutQuart)
            .OnComplete(EndMove);   
    }

    void EndMove()
    {
        moveIsEnd = true;
    }

    public void OnClick()
    {
        StartCoroutine(CoOnClick());
    }

    private IEnumerator CoOnClick()
    {
        soundManager.PlaySfx(soundManager.EffectSounds[1]);
        fade.transform.SetAsLastSibling();
        fade.FadeIn(fadeInTime);
        yield return new WaitForSeconds(fadeInTime);

        SceneManager.LoadScene("Select Stage");
    }
}
