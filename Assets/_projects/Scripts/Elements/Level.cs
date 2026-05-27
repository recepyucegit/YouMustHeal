using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    private LevelManager  _levelManager;
    private List<Enemy> _enemies = new List<Enemy>();
    public void StartLevel(LevelManager levelManager)
    {
        _enemies = GetComponentsInChildren<Enemy>().ToList();
        _levelManager = levelManager;
        foreach (var e in _enemies)
        {
            e.StartEnemy(_levelManager.gameDirector.player);
        }
    }

    public void StopLevel()
    {
        foreach (var e in _enemies)
        {
            e.SetPlayerDead();
        }
    }
}
