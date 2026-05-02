using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int startHealth;
    private int _currentHealth;
    public HealthBar healthBar;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetHit(1);
        }
    }
    public void RestartPlayer()
    {
        gameObject.SetActive(true);
        transform.position = Vector3.zero;
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
