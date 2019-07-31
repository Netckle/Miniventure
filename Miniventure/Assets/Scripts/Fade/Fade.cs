using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
	private Image normalImage;

	void Awake() 
	{
		normalImage = GetComponent<Image>();
	}

	// 이미지

    public void FadeIn(float fadeInTime, System.Action nextEvent = null, Image image = null)
	{
		StartCoroutine(CoFadeIn(fadeInTime, nextEvent, image));
	}

	public void FadeOut(float fadeOutTime, System.Action nextEvent = null, Image image = null)
	{
		StartCoroutine(CoFadeOut(fadeOutTime, nextEvent, image));
	}

	IEnumerator CoFadeIn(float fadeInTime, System.Action nextEvent = null, Image image = null)
	{
		Image img = (image == null ? normalImage : image);
		
		Color tempColor = img.color;

		while (tempColor.a < 1f)
		{
			tempColor.a += Time.deltaTime / fadeInTime;
			img.color = tempColor;

			if(tempColor.a >= 1f) tempColor.a = 1f;

			yield return null;
		}

		img.color = tempColor;
		if(nextEvent != null) nextEvent();
	}

	IEnumerator CoFadeOut(float fadeOutTime, System.Action nextEvent = null, Image image = null)
	{
		Image img = (image == null ? normalImage : image);
		Debug.Log(img);

		Color tempColor = img.color;

		while (tempColor.a > 0f)
		{
			tempColor.a -= Time.deltaTime / fadeOutTime;
			img.color = tempColor;

			if(tempColor.a <= 0f) tempColor.a = 0f;

			yield return null;
		}

		img.color = tempColor;
		if(nextEvent != null) nextEvent();
	}
	
	// 스프라이트

	public void FadeInSprite(SpriteRenderer renderer, float fadeInTime, System.Action nextEvent = null)
	{
		StartCoroutine(CoFadeOutSprite(renderer, fadeInTime, nextEvent));
	}

	public void FadeOutSprite(SpriteRenderer renderer, float fadeOutTime, System.Action nextEvent = null)
	{
		StartCoroutine(CoFadeOutSprite(renderer, fadeOutTime, nextEvent));
	}

	IEnumerator CoFadeInSprite(SpriteRenderer renderer, float fadeInTime, System.Action nextEvent = null)
    {
		Color tempColor = renderer.color;

		while (tempColor.a < 1f)
		{
			tempColor.a += Time.deltaTime / fadeInTime;
			renderer.color = tempColor;

			if(tempColor.a >= 1f) tempColor.a = 1f;

			yield return null;
		}

		renderer.color = tempColor;
		if(nextEvent != null) nextEvent();
    }

	IEnumerator CoFadeOutSprite(SpriteRenderer renderer, float fadeOutTime, System.Action nextEvent = null)
    {
		Color tempColor = renderer.color;

		while(tempColor.a > 0f)
		{
			tempColor.a -= Time.deltaTime / fadeOutTime;
			renderer.color = tempColor;

			if(tempColor.a <= 0f) tempColor.a = 0f;

			yield return null;
		}

		renderer.color = tempColor;
		if(nextEvent != null) nextEvent();
    }
}
