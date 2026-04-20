using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public void StartLevel()
    {
        
        foreach (var e in GetComponentsInChildren<Enemy>())
        {
            e.StartEnemy();
        }
    }

   
}
