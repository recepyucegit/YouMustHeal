using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Player _player;
    public int startHealth;
    private int _currentHealth;
    public float speed;
    public float playerWalkTowardsDistance;
    public float playerAttackDistance;
    private bool _isAttackInProgress;
    public ActionState actionState;
    public LayerMask playerSeeLayerMask;
    private Vector3 _playerLastSeenPosition;

    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
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
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            actionState = ActionState.Attack;
        }
         else if (GetDistanceFromPlayer() < playerWalkTowardsDistance && !_isAttackInProgress )
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
        else if (actionState == ActionState.Attack)
        {
            AttackPlayer();
        }
        else if (actionState == ActionState.Standing)
        {
            StopEnemy();

        }
        


    }

    private void AttackPlayer()
    {
        if (! _isAttackInProgress)
        {
            _isAttackInProgress = true;
            _navMeshAgent.isStopped = true;
            StartCoroutine(AttackCoroutline(1));
        }
    }

   IEnumerator AttackCoroutline(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            _player.GetHit(1);
        }
        
        _isAttackInProgress = false;
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
        
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.isStopped = false;
    }

    private void WalksTowardsPlayerLastSeenPos()
    {
          
        _navMeshAgent.SetDestination(_playerLastSeenPosition);
        _navMeshAgent.isStopped = false;
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