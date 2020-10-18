using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTextureManager : MonoBehaviour
{
    public static ScrollingTextureManager Instance;

    [Header("Settings: ")]
    [SerializeField] float stopDuration = 3;
    [SerializeField] AnimationCurve stopCurve;

    [SerializeField] GameObject titleScreenTexture;
    [SerializeField] List<ScrollingTextureScript> scrollingTextures = new List<ScrollingTextureScript>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetAllActive(bool _value)
    {
        foreach (var _sT in scrollingTextures)
        {
            _sT.isScrolling = _value;
            //if(_value == false) { _sT.ResetTextureOffset(); }
        }
    }

    public void SlowlyStopScrolling()
    {
        foreach (var _sT in scrollingTextures)
        {
            _sT.SlowlyStopScrolling(stopDuration, stopCurve);
            CameraManager.Instance.PanCameraAndFadeOut(stopDuration + 0.25f);
        }
    }

    public void DeactivateTitleScreenTexture()
    {
        titleScreenTexture.SetActive(false);
    }
}
