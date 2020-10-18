using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBehaviour : MonoBehaviour
{
    [SerializeField] Image shineImage;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float animationDuration;
    [SerializeField] float yOffset = 5;

    private Vector3 startPos;
    private Coroutine animationRoutine;

    private void Start()
    {
        StartAnimation();
    }

    private void StartAnimation()
    {
        if (animationRoutine != null) StopCoroutine(animationRoutine);
        animationRoutine = StartCoroutine(IEAnimate());
    }

    private IEnumerator IEAnimate()
    {
        float _lerpTime = 0;

        Color _startCol = shineImage.color;
        Color _endCol = _startCol;

        _startCol.a = 0;
        _endCol.a = 1;

        startPos = transform.localPosition;
        Vector3 _endPos = startPos + new Vector3(0, yOffset, 0);

        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / animationDuration;
            float _lerpKey = animationCurve.Evaluate(_lerpTime);

            transform.localPosition = Vector3.Lerp(startPos, _endPos, _lerpKey);
            shineImage.color = Color.Lerp(_startCol, _endCol, _lerpKey);
            yield return null;
        }

        StartAnimation();

        yield return null;
    }
}
