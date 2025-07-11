using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _voiceSource;

    [SerializeField] private AudioClip[] _sfxBellRing;

    
    private bool _isReady = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _isReady = true;
    }

    public bool IsReady()
    {
        return _isReady;
    }


    public void PlaySfxBell() => PlayRandomSfx(_sfxBellRing);

    private void PlayRandomSfx(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            PlaySfx(clips[randomIndex]);
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }
}
