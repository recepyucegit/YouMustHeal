using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int startHealth;
    private int _currentHealth;
    public bool isDead;
    

    public GameDirector gameDirector;
    public HealthBar healthBar;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetHit(1);
        }
    }


    public void RestartPlayer()
    {
        
        transform.position = Vector3.zero;
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        _playerMovement.ChangeAnimationState("Idle");
        isDead = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            other.gameObject.SetActive(false);
            _playerMovement.ChangeAnimationState("Win");
            gameDirector.LevelCompleted();
            
        }
    }
    public void GetHit(int damage)
    {
        if (isDead)
        {
            return;
        }
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        _playerMovement.ChangeAnimationState("Die");
        gameDirector.PlayerDied();

    }
}
