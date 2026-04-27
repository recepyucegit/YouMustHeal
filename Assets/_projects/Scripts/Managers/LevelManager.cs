using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public List<Level> levelPrefabs;
    public int currentLevelNo;
    private Level _currentlevel;
    public void RestartLevelManager()
    {
        DeleteCurrentLevel();
        CreateNewLevel();
    }

    private void CreateNewLevel()
    {
        _currentlevel = Instantiate(levelPrefabs[currentLevelNo -1]);
        _currentlevel.transform.position = Vector3.zero;
        _currentlevel.StartLevel(this );
    }

    private void DeleteCurrentLevel()
    {
        if (_currentlevel)
        {
            Destroy(_currentlevel.gameObject);
        }
    }
}
