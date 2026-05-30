using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public Player player;

    public GameState gameState;

    private void Start()
    {
        RestartLevel();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            LoadPreviousLevel();
        }

    }

    private void LoadPreviousLevel()
    {
        levelManager.currentLevelNo--;
        if (levelManager.currentLevelNo < 1)
        {
            levelManager.currentLevelNo = 1;
        }
        RestartLevel();
    }

    private void LoadNextLevel()
    {
        levelManager.currentLevelNo++;
        if ( levelManager.currentLevelNo >= levelManager.levelPrefabs.Count)
        {
            levelManager.currentLevelNo = levelManager.levelPrefabs.Count;
        }
        RestartLevel();
    }

    public void RestartLevel()
    {
        gameState = GameState.GamePlay;
        levelManager.RestartLevelManager();
        player.RestartPlayer();
    }

    public void PlayerDied()
    {
        levelManager.StopLevel();
        LevelFailed();
    }

    public void LevelCompleted()
    {
        gameState = GameState.WinUI;
       Invoke(nameof(LoadNextLevel), 3);
    }

    public void LevelFailed()
    {
        gameState = GameState.LoseUI;
        Invoke(nameof(RestartLevel), 3);
    }
}

public enum GameState
{
    MainMenu,
    GamePlay,
    WinUI,
    LoseUI,
    Inventory,

}