using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float fallSpeedBonus;
    private Rigidbody _rb;
    public LayerMask jumpLayers;
    public LayerMask lookLayers;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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

       

        if (Input.GetKeyDown(KeyCode.Space) && CheckIfLanded())
        {
            Jump();
        }


        MovePlayer(direction, speed);

        LookAtMouse();
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
        _rb.linearVelocity = dir.normalized * speed + yVelocity;
    }
}
