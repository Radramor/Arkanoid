using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion
    public GameObject GameOverScreen;
    public GameObject victoryScreen;

    public int AvailibleLives = 3;

    public int Lives { get; set; }

    public bool IsGameStarted { get; set; }

    //?????? ?????
    public event Action<int> OnLiveLost;

    private void Start()
    {
        Lives = AvailibleLives;
        Screen.SetResolution(1920, 1080, false);
        Ball.OnBallDeath += OnBallDeath;
    }
   // ???????? ?????? ??????, ????? ????? ???????????
    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {        
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
            BallsManager.Instance.ResetBalls();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            Lives--;

            if (Lives < 1)
            {
                GameOverScreen.SetActive(true);
            }
            else
            {
                OnLiveLost?.Invoke(Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }

    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }
}
