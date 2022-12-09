using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Target;
    public Text ScoreText;
    public Text LivesText;

    public int Score { get; set; }

    private void Start()
    {
        BricksManager.Instance.OnLevelLoaded += OnLevelLoaded;
        GameManager.Instance.OnLiveLost += OnLiveLost;
        OnLiveLost(GameManager.Instance.AvailibleLives);
    }
    //строка жизней
    private void OnLiveLost(int remainingLives)
    {
        LivesText.text = $"LIVES: {remainingLives}";
    }
    //обновление кирпичей после загрузки нового уровня
    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }
    //строка очков
    private void UpdateScoreText(int increment)
    {
        Score += increment;
        //строка оценки + нули в начале
        string scoreString = Score.ToString().PadLeft(5, '0');
        ScoreText.text = $@"SCORE:
{scoreString}";
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10);
    }
    //обновление каждый раз когда кирпич разрушается
    private void UpdateRemainingBricksText()
    {
        //$ - переменная внутри строки
        //@ - перенос строки
        Target.text = $@"TARGET:
{BricksManager.Instance.RemainingBricks.Count} / {BricksManager.Instance.InitialBricksCount}";
    }

    private void OnDisable()
    {
        BricksManager.Instance.OnLevelLoaded -= OnLevelLoaded;
    }
}
