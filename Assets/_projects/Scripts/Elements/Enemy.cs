using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Player _player;
    private Animator _animator;
    private CapsuleCollider _calpsuleCollider;
    public List<Light> eyeLights;
    private Coroutine _attackCoroutine;

    public int startHealth;
    private int _currentHealth;
    public float speed;
    public float playerWalkTowardsDistance;
    public float playerAttackDistance;
    private bool _isAttackInProgress;

    public ActionState actionState;
    public AnimationState currentAnimationState;
    private AnimationState _animationStateBeforeGetHit;

    public LayerMask playerSeeLayerMask;
    private Vector3 _playerLastSeenPosition;

    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _calpsuleCollider = GetComponent<CapsuleCollider>();
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
            SwitchAnimation(AnimationState.Attack ,true);
            _attackCoroutine=StartCoroutine(AttackCoroutline(1.2f));
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
   IEnumerator UpperBodyMaskCoroutine(float delay, float amount)
    {
        _animator.SetLayerWeight(1, amount);
        yield return new WaitForSeconds(delay);
        _animator.SetLayerWeight(1, 0);
        
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
        SwitchAnimation(AnimationState.Idle);
    }

    private void SwitchAnimation(AnimationState desiredAnimationState, bool forcePlayAnimation = false)
    {
        if (desiredAnimationState == AnimationState.Walk && (currentAnimationState != AnimationState.Walk || forcePlayAnimation))
        {
            //_animator.SetTrigger("Walk");
            _animator.CrossFade("Walk", .1f);
            currentAnimationState = AnimationState.Walk;
        }
        else if (desiredAnimationState == AnimationState.Idle && (currentAnimationState != AnimationState.Idle || forcePlayAnimation))
        {
            _animator.SetTrigger("Idle");
            currentAnimationState = AnimationState.Idle;
        }
        else if (desiredAnimationState == AnimationState.Attack && (currentAnimationState != AnimationState.Attack || forcePlayAnimation))
        {
            _animator.SetTrigger("Attack");
            currentAnimationState = AnimationState.Attack;
        }
        else if (desiredAnimationState == AnimationState.GetHit && (currentAnimationState != AnimationState.GetHit || forcePlayAnimation))
        {
            _animator.SetTrigger("GetHit");
            currentAnimationState = AnimationState.GetHit;
            StartCoroutine(UpperBodyMaskCoroutine(.3f, .5f));
        }
        else if (desiredAnimationState == AnimationState.Die && (currentAnimationState != AnimationState.Die || forcePlayAnimation))
        {
            // _animator.SetTrigger("Die");
            _animator.CrossFade("Die", .1f);
            currentAnimationState = AnimationState.Die;
        }
    }

    private float GetDistanceFromPlayer()
    {
        return (transform.position - _player.transform.position).magnitude;
    }

    private void WalksTowardsPlayer()
    {
        if (currentAnimationState != AnimationState.GetHit)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
            _navMeshAgent.isStopped = false;
            SwitchAnimation(AnimationState.Walk);

        }
        
        
    }

    private void WalksTowardsPlayerLastSeenPos()
    {
        if (currentAnimationState != AnimationState.GetHit)
        {
            _navMeshAgent.SetDestination(_playerLastSeenPosition);
            _navMeshAgent.isStopped = false;
            SwitchAnimation(AnimationState.Walk);
        }
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        StartCoroutine(PlayGetHitCoroutine());
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
      if (_currentHealth <=0)
        {
            Die();
        }
    }
    IEnumerator PlayGetHitCoroutine()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        if (currentAnimationState != AnimationState.GetHit)
        {
            _animationStateBeforeGetHit = currentAnimationState;
        }
        
        _navMeshAgent.isStopped = true;
        SwitchAnimation(AnimationState.GetHit);
        yield return new WaitForSeconds(.1f);
        SwitchAnimation(_animationStateBeforeGetHit);
    }

    private void Die()
    {
        CancelAttack();
        actionState = ActionState.Dead;
        _animationStateBeforeGetHit = AnimationState.Die;
        _navMeshAgent.isStopped = true;
        _calpsuleCollider.enabled = false;
        SwitchAnimation(AnimationState.Die);
        foreach (var e in eyeLights)
        {
            e.enabled = false;
        }
        Destroy(gameObject, 3);
    }

    private void CancelAttack()
    {
        _isAttackInProgress = false;

        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
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
public enum AnimationState
{
    Idle,
    Walk,
    Attack,
    GetHit,
    Die,
}