using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    private LevelManager  _levelManager;
    public void StartLevel(LevelManager levelManager)
    {
        _levelManager = levelManager;
        foreach (var e in GetComponentsInChildren<Enemy>())
        {
            e.StartEnemy(_levelManager.gameDirector.player);
        }
    }

   
}
