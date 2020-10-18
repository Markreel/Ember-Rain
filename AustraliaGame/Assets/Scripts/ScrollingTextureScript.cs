using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTextureScript : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] float offsetSpeed = 0.1f;
    private float offset = 0f;

    public bool isScrolling = false;

    [SerializeField] bool flickerAlpha = false;
    [SerializeField] AnimationCurve flickerCurve;
    [SerializeField] float flickerDuration;
    [SerializeField] float minimumFlickerValue = 0f;
    [SerializeField] float maximumFlickerValue = 0.3f;
    private float flickerTimer = 0;
    private float minFlickerValue;
    private float maxFlickerValue;

    private Coroutine slowlyStopScrollingRoutine;

    private void Awake()
    {
        minFlickerValue = minimumFlickerValue;
        maxFlickerValue = maximumFlickerValue;
        meshRenderer = GetComponent<MeshRenderer>();
        ResetTextureOffset();
    }

    private void Update()
    {
        if(isScrolling)
        {
            SetTextureOffset(new Vector2(offset, 0));
            offset += Time.deltaTime * offsetSpeed;

            if(flickerAlpha)
            {
                FlickerAlpha();
            }
        }
    }

    private void FlickerAlpha()
    {
        if (flickerTimer < 1)
        {
            flickerTimer += Time.deltaTime / flickerDuration;
            float _lerpKey = flickerCurve.Evaluate(flickerTimer);

            meshRenderer.sharedMaterial.SetFloat("_Metallic", Mathf.Lerp(minFlickerValue, maxFlickerValue, _lerpKey));
        }
        else
        {
            //previousFlickerValue = randomFlickerValue;
            //randomFlickerValue = Random.Range(0f, 0.5f);

            minFlickerValue = minimumFlickerValue;
            maxFlickerValue = maximumFlickerValue;
            flickerTimer = 0;
        }
    }

    private void SetTextureOffset(Vector2 _offset)
    {
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", _offset);
    }

    public void ResetTextureOffset()
    {
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0,0));
    }

    public void SlowlyStopScrolling(float _duration, AnimationCurve _curve)
    {
        if (slowlyStopScrollingRoutine != null) StopCoroutine(slowlyStopScrollingRoutine);
        slowlyStopScrollingRoutine = StartCoroutine(IESlowlyStopScrolling(_duration, _curve));
    }

    private IEnumerator IESlowlyStopScrolling(float _duration, AnimationCurve _curve)
    {
        float _lerpTime = 0;
        float _originalOffsetSpeed = offsetSpeed;

        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / _duration;

            float _lerpKey = _curve.Evaluate(_lerpTime);
            offsetSpeed = Mathf.Lerp(_originalOffsetSpeed, 0, _lerpKey);

            yield return null;
        }

        isScrolling = false;

        yield return null;
    }
}
