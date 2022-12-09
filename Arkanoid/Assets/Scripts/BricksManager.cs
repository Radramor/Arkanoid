using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    private static BricksManager _instance;

    public static BricksManager Instance => _instance;

    public event Action OnLevelLoaded;

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
    // максимальное количество строк и колонн в уровнях
    private int maxRows = 10;
    private int maxCols = 18;
    private GameObject bricksContainer;

    //начальное положение первого кирпича
    private float initialBrickSpawnPositionX = -7.7f;
    private float initialBrickSpawnPositionY = 3.24f;

    //расстояние смещения между блоками
    private float shiftAmountX = 0.9f;
    private float shiftAmountY = 0.5f;

    public Brick brickPrefab;

    public Sprite[] Sprites;

    public Color[] BrickColors;

    public List<Brick> RemainingBricks { get; set; }

    public List<int[,]> LevelsData { get; set; }

    public int InitialBricksCount { get; set; }

    public int CurrentLevel;

    private void Start()
    {
        bricksContainer = new GameObject("BricksContainer");
        LevelsData = LoadLevelsData();
        GenerateBricks();        
    }

    public void LoadLevel(int level)
    {
        CurrentLevel = level;
        ClearRemainingBricks();
        GenerateBricks();
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;

        if (CurrentLevel >= LevelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
            
        }
        else
        {
            LoadLevel(CurrentLevel);
        }
    }
    //очистка при рестарте
    private void ClearRemainingBricks()
    {
        foreach (Brick brick in RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }
    private void GenerateBricks()
    {
        RemainingBricks = new List<Brick>();
        int[,] currentLevelData = LevelsData[CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        //чтобы кирпичи не перекрывались
        float zShift = 0;

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxCols; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, Sprites[brickType - 1], BrickColors[brickType], brickType);
                    
                    RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmountX;
                //достигли ли конца столбцов
                if (col + 1 == maxCols)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmountY;
        }

        InitialBricksCount = RemainingBricks.Count;
        OnLevelLoaded?.Invoke();
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                // конец текущего уровня
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }

        return levelsData;
    }
}
