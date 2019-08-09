using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUnBeat : MonoBehaviour
{
    private SoundManager soundManager;
    public bool isUnBeatTime = false;

    private void Awake() 
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    public void UnBeat(SpriteRenderer sprite, float waitTime = 0.1f)
    {
        StartCoroutine(CorUnbeat(sprite, waitTime));
    }

    private IEnumerator CorUnbeat(SpriteRenderer sprite, float waitTime)
    {
        isUnBeatTime = true;
        soundManager.PlaySfx(soundManager.EffectSounds[2]);

        int countTime = 0;

        while (countTime < 10)
        {
            // Alpha Effect
            if (countTime % 2 == 0)
            {
                sprite.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                sprite.color = new Color32(255, 255, 255, 180);
            }

            // Wait Update Frame
            yield return new WaitForSeconds(waitTime);

            countTime++;
        }

        // Alpha Effect End
        sprite.color = new Color32(255, 255, 255, 255);

        isUnBeatTime = false;
        yield return null;
    }
}
