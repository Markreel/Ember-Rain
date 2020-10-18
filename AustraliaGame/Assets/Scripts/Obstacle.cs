using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Trajectory Settings: ")]
    [SerializeField] float speed = 5;

    [Header("Rotation Settings: ")]
    [SerializeField] bool randomRotationOnAwake = true;
    [SerializeField] bool rotate;
    [SerializeField] float rotationSpeed;

    [Header("PositionSettings")]
    [SerializeField] bool useOwnYPos = false;
    [SerializeField] float yPos;

    [Header("Audio Settings")]
    [SerializeField] float volume = 1;
    [SerializeField] AudioClip onHitClip;

    private void Awake()
    {
        if (randomRotationOnAwake) { transform.localEulerAngles = new Vector3(0, 0, Random.Range(0,360)); }
        if(useOwnYPos) { transform.position = new Vector3(transform.position.x, yPos, transform.position.z); }
    }

    private void Update()
    {
        if(transform.position.x > -15)
        {
            transform.position -= new Vector3(Time.deltaTime * speed, 0, 0);
        }
        else
        {
            Destroy(this);
        }


        if (rotate) { Rotate(); }
    }

    private void Rotate()
    {
        transform.localEulerAngles += new Vector3(0, 0, rotationSpeed);
    }

    public void PlayAudio()
    {
        AudioManager.Instance.PlayClip(onHitClip);
    }
}
