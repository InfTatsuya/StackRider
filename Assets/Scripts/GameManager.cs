using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Pause,
        Win,
        Lose
    }

    public static GameManager Instance { get; private set; }

    [SerializeField] Player player;
    [SerializeField] Background background;

    [SerializeField] List<GameObject> levelPrefabs;

    private int currentLevel;
    private GameObject levelInstance;

    public Player MainPlayer => player;

    private GameState gameState;
    public GameState CurrentGameState
    {
        get => gameState;
        set
        {
            gameState = value;
            switch (gameState)
            {
                case GameState.MainMenu:
                    isPlaying = false;
                    break;

                case GameState.Playing:
                    isPlaying = true;
                    Time.timeScale = 1f;
                    break;

                case GameState.Pause:
                    isPlaying = false;
                    Time.timeScale = 0f; 
                    break;

                case GameState.Win:
                    isPlaying = false;
                    OnGameOver(true);
                    break;

                case GameState.Lose:
                    isPlaying = false;
                    OnGameOver(false);
                    break;

                default:
                    break;
            }

        }
    }

    private int coinCount;

    private bool isPlaying;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        levelInstance = Instantiate(levelPrefabs[0]);
        currentLevel = 0;
        coinCount = 0;
    }

    private void Update()
    {
        //if (CurrentGameState == GameState.MainMenu && Input.GetKeyDown(KeyCode.Space))
        //{
        //    OnNewGame();
        //}

        //if((CurrentGameState == GameState.Win || CurrentGameState == GameState.Lose) && Input.GetKeyDown(KeyCode.R))
        //{
        //    ResetLevel();
        //}
    }

    public void ResetLevel()
    {
        player.ResetLevel();
        CurrentGameState = GameState.MainMenu;
        background.ChangeBackground(currentLevel);

        Destroy(levelInstance);
        levelInstance = Instantiate(levelPrefabs[currentLevel]);
    }

    public void NextLevel()
    {
        player.ResetLevel();
        CurrentGameState = GameState.MainMenu;
        currentLevel = ++currentLevel % levelPrefabs.Count;
        background.ChangeBackground(currentLevel);

        Destroy(levelInstance);
        levelInstance = Instantiate(levelPrefabs[currentLevel]);
    }

    public void OnNewGame()
    {
        CurrentGameState = GameState.Playing;
        player.OnNewGame();
        
    }

    public void AddCoin()
    {
        coinCount++;
        UIManager.Instance.SetCoinText(coinCount);
    }

    public void AddCoin(int amt)
    {
        coinCount += amt;
        UIManager.Instance.SetCoinText(coinCount);
    }

    //public void AddCoinWhenWin(int amt)
    //{
    //    coinCount += ballAmt * 5;
    //    UIManager.Instance.SetCoinText(coinCount);
    //}

    private void OnGameOver(bool isWin)
    {
        if (isWin)
        {
            //TODO: open win panel;
            player.OnWin();
            StartCoroutine(OpenWinPanelRoutine());
        }
        else
        {
            player.OnLose();
            UIManager.Instance.OpenLosePanel();
        }
    }

    private IEnumerator OpenWinPanelRoutine()
    {
        yield return new WaitForSeconds(player.BallCount * 0.6f);

        UIManager.Instance.OpenWinPanel();
    }
}
