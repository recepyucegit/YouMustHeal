using System;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float fallSpeedBonus;
    private bool _isJumping;

    private Rigidbody _rb;
    private Animator _animator;
    public Camera mainCamera;

    public LayerMask jumpLayers;
    public LayerMask lookLayers;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        var speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }

        _isJumping = !CheckIfLanded();

        if (Input.GetKeyDown(KeyCode.Space) && CheckIfLanded())
        {
            Jump();
        }


        MovePlayer(direction, speed);

        LookAtMouse();

        var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        SetWalkDirection(angle);
    }
    void SetWalkDirection(float angle)
    {
        _animator.SetFloat("WalkDirection", angle);
    }



    private void LookAtMouse()
    {
        
        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.ScreenPointToRay(Input.mousePosition).direction, out var hit, 50, lookLayers))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(hit.point);
        }

    }

    private bool CheckIfLanded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, .3f,jumpLayers))
        {
            return true;
        }

        
        return false;
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce);
        _isJumping = true;
        ChangeAnimationState("Jump");
    }

    void MovePlayer(Vector3 dir, float speed)
    {
        var yVelocity = _rb.linearVelocity;
        yVelocity.x = 0;
        yVelocity.z = 0;

        if (yVelocity.y <0)
        {
            yVelocity.y -= fallSpeedBonus * Time.deltaTime; 
        }

        if (!_isJumping)
        {
            if (dir.magnitude > 0)
            {
                ChangeAnimationState("Run");
            }
            else
            {
                ChangeAnimationState("Idle");
            }
        }
       

        _rb.linearVelocity = dir.normalized * speed + yVelocity;
    }

    void ChangeAnimationState(string key)
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool(key, true);
    }
}
