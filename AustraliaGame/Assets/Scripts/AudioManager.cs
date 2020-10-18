using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public enum ClipType { Effect, Music, UI, Ambience }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;

    public AudioClip DefaultMusicClip;
    public AudioClip TitleScreenMusicClip;
    public AudioClip GameOverMusicClip;
    [Space]
    public AudioClip HitByFireClip;
    public AudioClip HitByBranchClip;

    private Coroutine fadeMusicRoutine;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        musicSource.loop = true;
        musicSource.ignoreListenerPause = true;
        PlayMusic(TitleScreenMusicClip);
    }

    public void PlayClip(AudioClip _clip)
    {
        effectSource.PlayOneShot(_clip);
    }

    public void PlayClip(AudioClip _clip, float _volume = 1, ClipType _type = ClipType.Effect)
    {
        switch (_type)
        {
            default:
            case ClipType.Effect:
                effectSource.PlayOneShot(_clip, _volume);
                break;
            case ClipType.Music:
                PlayMusic(_clip);
                break;
        }
    }

    public void PlayRandomClip(AudioClip[] _clips, float _volume = 1, ClipType _type = ClipType.Effect)
    {
        if (_clips == null || _clips.Length == 0) return;
        AudioClip _rClip = _clips[Random.Range(0, _clips.Length)];

        switch (_type)
        {
            default:
            case ClipType.Effect:
                effectSource.PlayOneShot(_rClip, _volume);
                break;
            case ClipType.Music:
                PlayMusic(_rClip);
                break;
        }
    }
    public void PlayMusic(AudioClip _clip)
    {
        musicSource.clip = _clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void FadeInMusic(float _duration)
    {
        if (fadeMusicRoutine != null) { StopCoroutine(fadeMusicRoutine); }
        fadeMusicRoutine = StartCoroutine(IEFadeMusic(1f, _duration));
    }

    public void FadeOutMusic(float _duration)
    {
        if (fadeMusicRoutine != null) { StopCoroutine(fadeMusicRoutine); }
        fadeMusicRoutine = StartCoroutine(IEFadeMusic(0f, _duration));
    }

    private IEnumerator IEFadeMusic(float _finalVolume, float _duration)
    {
        float _lerpTime = 0;
        float _startVolume = musicSource.volume;

        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / _duration;

            musicSource.volume = Mathf.Lerp(_startVolume, _finalVolume, _lerpTime);

            yield return null;
        }

        yield return null;
    }
}
