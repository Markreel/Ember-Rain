using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourBACKUP : MonoBehaviour
{
    public static PlayerBehaviourBACKUP Instance;

    public enum States { Idle, Intro, Running, Flying, Falling, Gliding, GameOverSliding, GameOverFinal }
    public States CurrentState = States.Intro;

    private float yPos = 0;
    public float YPos { get { return yPos; } }
    private float xPos = 0;
    private float yVelocity = 0;

    [Header("Intro Settings: ")]
    [SerializeField] Vector3 introStartPos;
    [SerializeField] Vector3 introEndPos;
    [SerializeField] float introDuration = 2;
    private Coroutine introRoutine;

    [Header("Y Velocity Settings: ")]
    [SerializeField] float minVelocity = -10;
    [SerializeField] float minGlideVelocity;
    [SerializeField] float maxVelocity = 10;

    [Space]

    [SerializeField] float groundPos = 0;
    [SerializeField] float ceilingPosition = 5;
    private float heightCap;

    [Space]

    [SerializeField] int scoreModifier = 5;
    [SerializeField] float flightModifier = 0.1f;
    [SerializeField] float gravityModifier = 9.807f;

    [SerializeField] LayerMask obstacleLayerMask;
    [SerializeField] LayerMask interactableLayerMask;

    [Header("References: ")]
    [SerializeField] GameObject fire;

    private Animator anim;

    private void Awake()
    {
        InitiateSetup();
    }

    private void Start()
    {
        //StartIntro();
    }

    private void FixedUpdate()
    {
        HandleStateBehaviour();
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void InitiateSetup()
    {
        Instance = this;

        CurrentState = States.Idle;

        yPos = groundPos;
        heightCap = ceilingPosition;

        anim = GetComponent<Animator>();
        anim.SetBool("IsGrounded", true);
    }

    private void HandleGameActiveInput()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && CurrentState == States.Running)
        {
            Jump();
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)))
        {
            if (CurrentState == States.Falling) { CurrentState = States.Gliding; return; }

            //if (CurrentState == States.Running || CurrentState == States.Flying) { GoUp(); }

        }

        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)) && (CurrentState == States.Flying || CurrentState == States.Gliding))
        {
            CurrentState = States.Falling;
        }
    }

    private void HandleAnimations()
    {
        if (CurrentState == States.Gliding && yVelocity < 0) { anim.SetBool("IsGliding", true); }
        else { anim.SetBool("IsGliding", false); }

        anim.SetFloat("YVelocity", yVelocity);
    }

    private void HandleStateBehaviour()
    {
        switch (CurrentState)
        {
            case States.Idle:
                break;
            case States.Intro:
                break;
            default:
            case States.Running:
                HandleGameActiveInput();
                ProgressXPos();
                break;
            case States.Flying:
                HandleGameActiveInput();
                ProgressXPos();
                //UpdatePosition();
                break;
            case States.Gliding:
                HandleGameActiveInput();
                ProgressXPos();
                FallDown(true);
                break;
            case States.Falling:
                HandleGameActiveInput();
                ProgressXPos();
                FallDown();
                break;
            case States.GameOverSliding:
                FallDownGameOver();
                break;
        }
    }

    private void ProgressXPos()
    {
        xPos += Time.deltaTime;
        UIManager.Instance.UpdateScore(Mathf.RoundToInt(xPos * scoreModifier));

        if (ObstacleManager.Instance != null)
            ObstacleManager.Instance.XPos = xPos;
    }

    private void Jump()
    {
        if (yPos == groundPos) { anim.SetBool("IsGrounded", false); }
        if (yVelocity < 0) { yVelocity = 0; }
        if (yPos == heightCap) { CurrentState = States.Falling; }
        else
        {
            CurrentState = States.Flying;
            ChangeVelocity(flightModifier);
        }
    }

    private void ChangeVelocity(float _value)
    {
        yVelocity = 0.25f; //0.5f
        UpdatePosition();
        CurrentState = States.Falling;

        //if(yVelocity > 0)
        //{
        //    yVelocity += 1 * Physics2D.gravity.y * (8.2f - 1) * Time.deltaTime;
        //}
        //else if (yVelocity > 0 && !Input.GetKey(KeyCode.Space))
        //{
        //    yVelocity += 1 * Physics2D.gravity.y * (2.1f - 1) * Time.deltaTime;
        //}

        //yVelocity = Mathf.Clamp(yVelocity + (_value + 1) * Time.deltaTime, minVelocity, maxVelocity);
    }

    //OLD FLY BEHAVIOUR
    //private void GoUp()
    //{
    //    if (yPos == groundPos) { anim.SetBool("IsGrounded", false); }
    //    if (yVelocity < 0) { yVelocity = 0; }
    //    if (yPos == heightCap) { CurrentState = States.Falling; }
    //    else
    //    {
    //        CurrentState = States.Flying;
    //        ChangeVelocity(flightModifier);
    //    }
    //}

    //private void ChangeVelocity(float _value)
    //{
    //    yVelocity = Mathf.Clamp(yVelocity + (_value + 1) * Time.deltaTime, minVelocity, maxVelocity);
    //}

    private void FallDown(bool _gliding = false)
    {
        //Debug.Log("GLIDING: " + _gliding);

        if (yPos == heightCap && yVelocity > 0) { yVelocity = 0; }
        //yVelocity = Mathf.Clamp(yVelocity + 1f * Physics2D.gravity.y * (gravityModifier - 1) * Time.deltaTime, _gliding ? minGlideVelocity : minVelocity, maxVelocity);    

        if (yVelocity < 0)
        {
            yVelocity = Mathf.Clamp(yVelocity + 1f * Physics2D.gravity.y * (gravityModifier - 1) * Time.deltaTime, _gliding ? minGlideVelocity : minVelocity, maxVelocity);
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            yVelocity = Mathf.Clamp(yVelocity + 1f * Physics2D.gravity.y * (gravityModifier - 1) * Time.deltaTime, _gliding ? minGlideVelocity : minVelocity, maxVelocity);
        }
        else
        {
            yVelocity = Mathf.Clamp(yVelocity + -2f * (gravityModifier - 1) * Time.deltaTime, _gliding ? minGlideVelocity : minVelocity, maxVelocity);
        }

        //if (yVelocity < 0 && !_gliding)
        //{
        //    yVelocity += -0.09f * 0.01f * Time.deltaTime;
        //}
        //else
        //{
        //    yVelocity += -0.09f * 0.2f * Time.deltaTime;
        //}

        //Debug.Log(yVelocity);
        UpdatePosition();
        if (yPos <= groundPos && CurrentState != States.GameOverSliding) { CurrentState = States.Running; anim.SetBool("IsGrounded", true); }
    }

    private void FallDownGameOver()
    {

        if (yPos == heightCap && yVelocity > 0) { yVelocity = 0; }
        //yVelocity = Mathf.Clamp(yVelocity + 1f * Physics2D.gravity.y * (gravityModifier - 1) * Time.deltaTime, _gliding ? minGlideVelocity : minVelocity, maxVelocity);    
        //yVelocity = Mathf.Clamp(yVelocity + 1f * Physics2D.gravity.y * (gravityModifier - 1) * Time.deltaTime, minVelocity, maxVelocity);

        yVelocity = Mathf.Clamp(yVelocity + -2f * (gravityModifier - 1) * Time.deltaTime, minVelocity, maxVelocity);

        float xVelocity = Mathf.Abs(transform.position.x) > 1 ? 0.0075f *  Mathf.Abs(transform.position.x) : 0.0075f;
        transform.position += new Vector3(xVelocity,0, 0);

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        //if (yPos == heightCap) { yVelocity = 0; }
        yPos = Mathf.Clamp(yPos + yVelocity, groundPos, heightCap);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -500,0), yPos, transform.position.z);
        //if(transform.position.y > groundPos) { Debug.Log(yVelocity); }
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (obstacleLayerMask == (obstacleLayerMask | (1 << _col.gameObject.layer)))
        {
            GetHit(_col.GetComponent<Obstacle>());
        }
    }

    private void GetHit(Obstacle _obstacle)
    {
        _obstacle.PlayAudio();
        UIManager.Instance.Flash(0.25f);
        GameOver();
    }

    private void GameOver()
    {
        CurrentState = States.GameOverSliding;
        anim.SetBool("IsDead", true);
        GameManager.Instance.GameOver();
    }

    public void StartIntro()
    {
        CurrentState = States.Intro;

        if (introRoutine != null) { StopCoroutine(introRoutine); }
        introRoutine = StartCoroutine(IEIntro());
    }

    private IEnumerator IEIntro()
    {
        float _lerpTimer = 0;

        while (_lerpTimer < 1)
        {
            _lerpTimer += Time.deltaTime / introDuration;

            transform.position = Vector3.Lerp(introStartPos, introEndPos, _lerpTimer);
            yield return null;
        }

        anim.SetTrigger("StartRunning");
        fire.transform.parent = CameraManager.Instance.gameObject.transform;
        CurrentState = States.Running;
        ScrollingTextureManager.Instance.SetAllActive(true);

        yield return null;
    }
}
