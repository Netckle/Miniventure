using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    private SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        soundManager.PlaySfx(soundManager.BGMSounds[0], true);
    }
}
