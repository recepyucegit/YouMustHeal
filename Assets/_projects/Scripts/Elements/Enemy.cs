using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startHealth;
    private int _currentHealth;
   public void StartEnemy()
    {
        _currentHealth = startHealth;
    }
    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        print(_currentHealth);
      if (_currentHealth <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
