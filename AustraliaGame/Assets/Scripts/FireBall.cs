using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Obstacle
{
    [SerializeField] float flickerDuration = 3;
    [SerializeField] float flickerPerSecond = 3;
    [SerializeField] Sprite warningSprite1;
    [SerializeField] Sprite warningSprite2;


    [SerializeField] float finalWarningDuration = 1;
    [SerializeField] Sprite warningSpriteFinal;

    [SerializeField] GameObject fireBallProjectilePrefab;

    [SerializeField] AudioClip warningLoopClip;
    [SerializeField] AudioClip spawningFireClip;


    private SpriteRenderer spriteRenderer;
    private bool followPlayer = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = warningSprite1;

        transform.position = new Vector3(8, PlayerBehaviour.Instance.YPos, transform.position.z);
    }

    private void Start()
    {
        StartWarning();
    }

    private void StartWarning()
    {
        StartCoroutine(IEWarning());

        StartCoroutine(IEAudio());
    }

    private void Update()
    {
        if (followPlayer)
        {
            transform.position = new Vector3(transform.position.x, PlayerBehaviour.Instance.YPos, transform.position.z);


        }
    }

    private IEnumerator IEAudio()
    {
        while (followPlayer)
        {
            AudioManager.Instance.PlayClip(warningLoopClip, 0.5f);
            yield return new WaitForSeconds(warningLoopClip.length);
            //yield return null;
        }

        yield return null;
    }

    private IEnumerator IEWarning()
    {
        for (int i = 0; i < flickerDuration * flickerPerSecond; i++)
        {
            spriteRenderer.sprite = warningSprite2;
            yield return new WaitForSeconds(1f / flickerPerSecond / 2);

            spriteRenderer.sprite = warningSprite1;
            yield return new WaitForSeconds(1f / flickerPerSecond / 2);
        }

        spriteRenderer.sprite = warningSpriteFinal;
        followPlayer = false;
        yield return new WaitForSeconds(finalWarningDuration);

        ObstacleManager.Instance.SpawnObstacle(fireBallProjectilePrefab, transform.position.y);
        AudioManager.Instance.PlayClip(spawningFireClip);
        Destroy(gameObject);

        yield return null;
    }
}
