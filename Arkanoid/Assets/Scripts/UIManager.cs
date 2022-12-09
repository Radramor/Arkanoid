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
    //������ ������
    private void OnLiveLost(int remainingLives)
    {
        LivesText.text = $"LIVES: {remainingLives}";
    }
    //���������� �������� ����� �������� ������ ������
    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }
    //������ �����
    private void UpdateScoreText(int increment)
    {
        Score += increment;
        //������ ������ + ���� � ������
        string scoreString = Score.ToString().PadLeft(5, '0');
        ScoreText.text = $@"SCORE:
{scoreString}";
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10);
    }
    //���������� ������ ��� ����� ������ �����������
    private void UpdateRemainingBricksText()
    {
        //$ - ���������� ������ ������
        //@ - ������� ������
        Target.text = $@"TARGET:
{BricksManager.Instance.RemainingBricks.Count} / {BricksManager.Instance.InitialBricksCount}";
    }

    private void OnDisable()
    {
        BricksManager.Instance.OnLevelLoaded -= OnLevelLoaded;
    }
}
