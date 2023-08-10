using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool torchEquipt;

    [Header("Movement Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float slowMovement;
    [SerializeField] float playerHeight;
    [SerializeField] float groundedAllowance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDrag;


    [Header("References")]
    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        GetInput();

        if (IsGrounded())
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        SpeedClamp();

    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        if (torchEquipt)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            moveDirection.x = 0;
            moveDirection.y = 0;
        }
        else
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        if (torchEquipt)
        {
            rb.AddForce(moveDirection.normalized * slowMovement * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
    }

    bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (playerHeight / 2) + groundedAllowance, groundLayer);

        return isGrounded;
    }

    void SpeedClamp()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 clampedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
        }
    }

    public void MoveToPos(Transform targetPos)
    {
        transform.position = new Vector3(targetPos.position.x, transform.position.y, targetPos.position.z);
    }
}
