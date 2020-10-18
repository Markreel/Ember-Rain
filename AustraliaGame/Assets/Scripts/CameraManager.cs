using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] float panDuration = 1;
    [SerializeField] AnimationCurve panCurve;
    [SerializeField] Vector3 endPos;
    [Space]
    [SerializeField] Image fadeImage;

    private Vector3 startPos;

    private void Awake()
    {
        Instance = this;
    }

    public void PanCameraAndFadeOut(float _delay)
    {
        StartCoroutine(IEPanCameraAndFadeOut(_delay));
    }

    private IEnumerator IEPanCameraAndFadeOut(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        AudioManager.Instance.FadeOutMusic(panDuration);

        float _lerpTime = 0;

        Color _startCol = fadeImage.color;
        Color _endCol =_startCol;
        _endCol.a = 1;

        startPos = transform.position;

        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / panDuration;
            float _lerpKey = panCurve.Evaluate(_lerpTime);

            transform.position = Vector3.Lerp(startPos, endPos, _lerpKey);
            fadeImage.color = Color.Lerp(_startCol, _endCol, _lerpKey);

            yield return null;
        }

        UIManager.Instance.ActivateGameOverScreen();

        yield return null;
    }
}
