using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SimplePlayBGM(0);
    }

    public AudioClip[] BGMSounds;
    public AudioClip[] EffectSounds;
    
    public bool isSfxMute = false;
    [Range(0.0f, 1.0f)]
    public float sfxVolume = 1.0f;

    public Vector3 soundPlayPosition;

    float defaultMinDistance = 10.0f;
    float defaultMaxDistance = 30.0f;

    public void SimplePlayBGM(int index)
    {
        PlaySfx(BGMSounds[index], true);
    }

    // Original
    public void PlaySfx(AudioClip sfx, Vector3 _pos, float _MinDistance,
    float _MaxDistance, float _Volume, bool _Loop = false)
    {
        if (isSfxMute) return;

        GameObject soundPlayObject = new GameObject("Sfx_" + sfx.name);
        soundPlayObject.transform.position = _pos;

        AudioSource _AudioSource = soundPlayObject.AddComponent<AudioSource>();
        _AudioSource.clip = sfx;
        _AudioSource.minDistance = _MinDistance;
        _AudioSource.maxDistance = _MaxDistance;
        _AudioSource.volume = sfxVolume;
        _AudioSource.loop = _Loop;

        _AudioSource.Play();

        if (!_Loop)
        {
            Destroy(soundPlayObject, sfx.length);
        }        
    }

    public void PlaySfx(AudioClip sfx, bool loop)
    {
        PlaySfx(sfx, soundPlayPosition, defaultMinDistance, defaultMaxDistance, sfxVolume, loop);
    }

    public void PlaySfx(AudioClip sfx)
    {
        PlaySfx(sfx, soundPlayPosition, defaultMinDistance, defaultMaxDistance, sfxVolume);
    }

    public void PlaySfx(AudioClip sfx, float _MinDistance, float _MaxDistance)
    {
        PlaySfx(sfx, soundPlayPosition, _MinDistance, _MaxDistance, sfxVolume);
    }

    public void PlaySfx(AudioClip sfx, Vector3 _pos)
    {
        PlaySfx(sfx, _pos, defaultMinDistance, defaultMaxDistance, sfxVolume);
    }

    public void PlaySfx(AudioClip sfx, Vector3 _pos, float _MinDistance, float _MaxDistance)
    {
        PlaySfx(sfx, _pos, _MinDistance, _MaxDistance, sfxVolume);
    }

    public void PlaySfx(AudioClip sfx, Vector3 _pos, float sfxVolume)
    {
        PlaySfx(sfx, _pos, defaultMinDistance, defaultMaxDistance, sfxVolume);
    }
}
