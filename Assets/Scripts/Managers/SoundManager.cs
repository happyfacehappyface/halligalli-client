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
    [SerializeField] private AudioClip[] _sfxButtonClick;

    [SerializeField] private AudioClip[] _sfxStartGame;
    [SerializeField] private AudioClip[] _sfxEndGame;
    [SerializeField] private AudioClip[] _sfxCorrect;
    [SerializeField] private AudioClip[] _sfxWrong;
    [SerializeField] private AudioClip[] _sfxCardOpen;
    [SerializeField] private AudioClip[] _sfxCardMove;
    [SerializeField] private AudioClip[] _sfxTada;
    [SerializeField] private AudioClip[] _sfxWinning;
    

    
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


    public void PlaySfxBell(float delay) => PlayRandomSfx(_sfxBellRing, delay);
    public void PlaySfxButtonClick(float delay) => PlayRandomSfx(_sfxButtonClick, delay);
    public void PlaySfxStartGame(float delay) => PlayRandomSfx(_sfxStartGame, delay);
    public void PlaySfxEndGame(float delay) => PlayRandomSfx(_sfxEndGame, delay);
    public void PlaySfxCorrect(float delay) => PlayRandomSfx(_sfxCorrect, delay);
    public void PlaySfxWrong(float delay) => PlayRandomSfx(_sfxWrong, delay);
    public void PlaySfxCardOpen(float delay) => PlayRandomSfx(_sfxCardOpen, delay);
    public void PlaySfxCardMove(float delay) => PlayRandomSfx(_sfxCardMove, delay);
    public void PlaySfxTada(float delay) => PlayRandomSfx(_sfxTada, delay);
    public void PlaySfxWinning(float delay) => PlayRandomSfx(_sfxWinning, delay);

    private void PlayRandomSfx(AudioClip[] clips, float delay)
    {
        if (clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            if (delay <= 0f)
            {
                PlaySfx(clips[randomIndex]);
            }
            else
            {
                StartCoroutine(CO_PlaySfxWithDelay(clips[randomIndex], delay));
            }
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }

    private IEnumerator CO_PlaySfxWithDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySfx(clip);
    }
}
