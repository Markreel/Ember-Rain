using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] string scoreFormat = "m";
    private int score;

    [SerializeField] GameObject titleScreen;

    [Header("Flash Settings: ")]
    [SerializeField] Image flashImage;
    [SerializeField] float flashDuration = 0.5f;
    [SerializeField] AnimationCurve flashCurve;
    private Coroutine flashRoutine;

    [Header("Game Over Screen")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameOverText;
    [SerializeField] AnimationCurve gameOverTransitionCurve;
    [SerializeField] float gameOverTransitionDelay;
    [SerializeField] float gameOverTransitionDuration;
    [Space]
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] float scoreTransitionDuration;
    [SerializeField] float scoreTransitionDelay;
    [Space]
    [SerializeField] GameObject newHighScoreText;
    [SerializeField] AnimationCurve newHighScoreScaleCurve;
    [SerializeField] float newHighScoreScaleDuration;
    [SerializeField] AudioClip newHighScoreAudioClip;

    private bool newHighScore = false;
    private Coroutine gameOverRoutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scoreText.gameObject.SetActive(false);
    }

    public void UpdateScore(int _score)
    {
        score = _score;
        scoreText.text = _score + scoreFormat;
    }

    public void ChangeTitleScreenActivity(bool _value)
    {
        titleScreen.SetActive(false);
    }

    public void ChangeScoreActivity(bool _value)
    {
        scoreText.gameObject.SetActive(_value);
    }

    public void Flash(float _duration = 0)
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(IEFlash(_duration));
    }

    private IEnumerator IEFlash(float _duration = 0)
    {
        float _lerpTime = 0;

        Color _startCol = Color.clear;
        Color _endCol = Color.white;

        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / (_duration == 0 ? flashDuration : _duration);
            float _lerpKey = flashCurve.Evaluate(_lerpTime);

            flashImage.color = Color.Lerp(_startCol, _endCol, _lerpKey);

            yield return null;
        }

        yield return null;
    }

    public void ActivateGameOverScreen()
    {
        if (gameOverRoutine != null) StopCoroutine(gameOverRoutine);
        gameOverRoutine = StartCoroutine(IEGameOverScreen());
    }

    private IEnumerator IEGameOverScreen()
    {
        float _lerpTime = 0;
        gameOverScreen.SetActive(true);

        //Load Highscore and check if new score is higher
        int _highScore = DataManager.LoadHighscore();
        if(score > _highScore)
        {
            newHighScore = true;
            StartCoroutine(IEScaleNewHighScoreText());
            DataManager.SaveHighscore(score);
        }

        finalScoreText.text = "You ran:\n" + scoreText.text;
        highScoreText.text = "Highscore:\n" + (newHighScore ? score : DataManager.LoadHighscore()) + "m";

        //Game Over text appears in screen
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / gameOverTransitionDuration;
            float _lerpKey = gameOverTransitionCurve.Evaluate(_lerpTime);

            gameOverText.transform.localPosition = Vector3.Lerp(new Vector3(0, -600, 0), new Vector3(0, 0, 0), _lerpKey);

            yield return null;
        }
        _lerpTime = 0;

        //Game Over text moves up
        yield return new WaitForSeconds(gameOverTransitionDelay);
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / gameOverTransitionDuration;
            float _lerpKey = gameOverTransitionCurve.Evaluate(_lerpTime);

            gameOverText.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 300, 0), _lerpKey);

            yield return null;
        }
        _lerpTime = 0;

        //Final score appears in screen
        yield return new WaitForSeconds(scoreTransitionDelay);
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / scoreTransitionDuration;
            float _lerpKey = gameOverTransitionCurve.Evaluate(_lerpTime);

            finalScoreText.transform.localPosition = Vector3.Lerp(new Vector3(0, -600, 0), new Vector3(0, 0, 0), _lerpKey);

            yield return null;
        }
        _lerpTime = 0;

        //Highscore appears in screen
        yield return new WaitForSeconds(scoreTransitionDelay);
        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / scoreTransitionDuration;
            float _lerpKey = gameOverTransitionCurve.Evaluate(_lerpTime);

            highScoreText.transform.localPosition = Vector3.Lerp(new Vector3(0, -900, 0), new Vector3(0, -300, 0), _lerpKey);

            yield return null;
        }

        GameManager.Instance.GameOverFinal();

        if (newHighScore) { newHighScoreText.SetActive(true); AudioManager.Instance.PlayClip(newHighScoreAudioClip); }

        yield return null;
    }

    IEnumerator IEScaleNewHighScoreText()
    {
        float _lerpTime = 0;

        while (_lerpTime < 1)
        {
            _lerpTime += Time.deltaTime / newHighScoreScaleDuration;
            float _lerpKey = newHighScoreScaleCurve.Evaluate(_lerpTime);

            newHighScoreText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, _lerpKey);

            yield return null;
        }

        StartCoroutine(IEScaleNewHighScoreText());
        yield return null;
    }

}
