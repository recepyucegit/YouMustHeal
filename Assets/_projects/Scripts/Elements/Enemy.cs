using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;
    private Rigidbody _rb;
    private Player _player;
    public int startHealth;
    private int _currentHealth;
    public float speed;
    public float playerWalkTowardsDistance;
    public ActionState actionState;
    public LayerMask playerSeeLayerMask;
    private Vector3 _playerLastSeenPosition;

    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void StartEnemy(Player player)
    {
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        _player = player;
    }

    private void Update()
    {
        if (actionState == ActionState.Dead)
        {
            return;
        }

        // Decider Logic
        if (GetDistanceFromPlayer() < playerWalkTowardsDistance  )
        {
            if (GetIfEnemySeesPlayer())
            {
                actionState = ActionState.WalkTowardsPlayer;
            }
            else if (_playerLastSeenPosition != Vector3.zero)
            {
                actionState = ActionState.WalkTowardsPlayerLastSeenPos;
            }
            
        }
         

        // Action States
        if (actionState == ActionState.WalkTowardsPlayer)
        {
            WalksTowardsPlayer();
        }
        else if (actionState == ActionState.WalkTowardsPlayerLastSeenPos)
        {
            WalksTowardsPlayerLastSeenPos();
        }
        else if (actionState == ActionState.Standing)
        {
            StopEnemy();

        }


    }

    

    private bool GetIfEnemySeesPlayer()
    {
        if (Physics.Raycast(transform.position + Vector3.up, 
            _player.transform.position - transform.position,
            playerWalkTowardsDistance, playerSeeLayerMask))
        {
            return false;
        }
        _playerLastSeenPosition = _player.transform.position;
        return true;
    }

    private void StopEnemy()
    {
        _rb.linearVelocity = Vector3.zero;
    }

    private float GetDistanceFromPlayer()
    {
        return (transform.position - _player.transform.position).magnitude;
    }

    private void WalksTowardsPlayer()
    {
        var dir = Vector3.zero;
        dir = (_player.transform.position - transform.position).normalized; 
        _rb.linearVelocity = dir * speed;
    }

    private void WalksTowardsPlayerLastSeenPos()
    {
        var dir = Vector3.zero;
        dir = (_playerLastSeenPosition - transform.position).normalized;
        _rb.linearVelocity = dir * speed;
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
        Destroy(gameObject);
    }
}

public enum ActionState
{
    Standing,
    WalkTowardsPlayer,
    WalkTowardsPlayerLastSeenPos,
    Attack,
    Dead,
}