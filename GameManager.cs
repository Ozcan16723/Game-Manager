using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singeleton
    public static GameManager _Instance;
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject StartP, InGameP, NextP, GameOverP;
    public float countDown = 2f;

    [SerializeField] private int asynSceneIndex = 1;

    public enum GameState
    {
        Start,
        InGame,
        Next,
        GameOver
    }
    public GameState gamestate;


    public enum Panels
    {
        Startp,
        Nextp,
        GameOverp,
        InGamep
    }
    private void Start()
    {
        gamestate = GameState.Start;
    }
    private void Update()
    {
        switch (gamestate)
        {
            case GameState.Start: GameStart();
                break;
            case GameState.InGame: GameInGame();
                break;
            case GameState.Next: Gamefinish();
                break;
            case GameState.GameOver: GameOver();
                break;
        }
    }

    void PanelController(Panels currentPanel)
    {
        StartP.SetActive(false);
        InGameP.SetActive(false);
        NextP.SetActive(false);
        GameOverP.SetActive(false);

        switch (currentPanel)
        {
            case Panels.Startp:
                StartP.SetActive(true);
                break;
            case Panels.InGamep:
                InGameP.SetActive(true);
                break;
            case Panels.Nextp:
                NextP.SetActive(true);
                break;
            case Panels.GameOverp:
                GameOverP.SetActive(true);
                break;
        }


    }

    void GameStart()
    {
        PanelController(Panels.Startp);
        if (Input.anyKeyDown)
        {
            gamestate = GameState.InGame;
        }
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
        }
    }
    void GameInGame()
    {
        PanelController(Panels.InGamep);
    }
    void Gamefinish()
    {
        PanelController(Panels.Nextp);
    }
    void GameOver()
    {
        countDown -= Time.deltaTime;
        if (countDown < 0)
        {
            PanelController(Panels.GameOverp);
        }
    }

    public void RestartButton() 
    {
        SceneManager.UnloadSceneAsync(asynSceneIndex);
        SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
        gamestate = GameState.Start;
    }
    public void NextLevelButton() 
    {
        if (SceneManager.sceneCountInBuildSettings == asynSceneIndex + 1)
        {
            SceneManager.UnloadSceneAsync(asynSceneIndex);
            asynSceneIndex = 1;
            SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
        }
        else
        {
            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync(asynSceneIndex);
                asynSceneIndex++;
            }
            SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
        }
        gamestate = GameState.Start;
    }
}
