using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationAudio : MonoBehaviour
{
    [SerializeField] List<AnimationAudioBlueprint> blueprints = new List<AnimationAudioBlueprint>();

    public void InvokeAnimationAudio(int _index)
    {
        if (blueprints[_index].PlayRandomClip) { AudioManager.Instance.PlayRandomClip(blueprints[_index].RandomClips, blueprints[_index].Volume); }
        else { AudioManager.Instance.PlayClip(blueprints[_index].SingleClip, blueprints[_index].Volume); }
    }
}

[System.Serializable]
public class AnimationAudioBlueprint
{
    [SerializeField] string name;
    public bool PlayRandomClip;
    [Range(0,1)] public float Volume = 1;
    public AudioClip SingleClip;
    public AudioClip[] RandomClips;
}
