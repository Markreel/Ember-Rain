using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameStates { Playing, TitleScreen, GameOver, GameOverFinal, }
    private GameStates CurrentGameState = GameStates.TitleScreen;

    private bool hasStartedGame = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        switch (CurrentGameState)
        {
            default:
            case GameStates.Playing:
                break;
            case GameStates.TitleScreen:
                if (Input.GetKeyDown(KeyCode.Space) && !hasStartedGame) { StartGame(); }
                break;
            case GameStates.GameOver:
                break;
            case GameStates.GameOverFinal:
                if (Input.GetKeyDown(KeyCode.Space)) { ReplayGame(); }
                break;
        }
    }

    public void StartGame()
    {
        StartCoroutine(IEStartGame());
    }

    private IEnumerator IEStartGame()
    {
        hasStartedGame = true;
        UIManager.Instance.Flash(0.5f);
        AudioManager.Instance.PlayClip(AudioManager.Instance.HitByFireClip);
        UIManager.Instance.ChangeTitleScreenActivity(false);

        yield return new WaitForSeconds(0.25f);
        ScrollingTextureManager.Instance.DeactivateTitleScreenTexture();
        yield return new WaitForSeconds(0.25f);

        CurrentGameState = GameStates.Playing;
        UIManager.Instance.ChangeScoreActivity(true);
        PlayerBehaviour.Instance.StartIntro();
        ObstacleManager.Instance.StartSpawning();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.DefaultMusicClip);

        yield return null;
    }

    public void GameOver()
    {
        CurrentGameState = GameStates.GameOver;

        ObstacleManager.Instance.StopSpawning();
        ScrollingTextureManager.Instance.SlowlyStopScrolling();
    }

    public void GameOverFinal()
    {
        CurrentGameState = GameStates.GameOverFinal;
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Donate()
    {
        Application.OpenURL("https://www.wires.org.au/donate/emergency-fund");
    }
}
