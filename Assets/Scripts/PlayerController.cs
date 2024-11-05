using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public Transform CameraContainer;

    Vector3 curMovementInput;
    Vector3 curLook;
    public LayerMask groundLayerMask;

    public float moveSpeed = 3.0f;
    public float jumpForce = 5.0f;

    private float minRotation = -60f;
    private float maxRotation = 80f;
    private float lookSensitivity = 0.045f;

    // Ű �Է½� curMovementInput �� ����
    private void OnMove(InputValue value)
    {
        curMovementInput = value.Get<Vector2>();
    }
    // curMovementInput ���� ���� �÷��̾� �̵�
    private void Move()
    {
        Vector3 dir = ((transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x)).normalized * moveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    // ���콺 �����ӽ� curLook �� ����
    private void OnLook(InputValue value)
    {
        Vector2 delta = value.Get<Vector2>() * lookSensitivity;
        curLook.x += delta.x;
        curLook.y = Mathf.Clamp(curLook.y - delta.y, minRotation, maxRotation);
    }
    // curLook ���� ���� ĳ���� �� ī�޶� ȸ��
    private void CameraLook()
    {
        transform.eulerAngles = new Vector3(0, curLook.x, 0);
        CameraContainer.localEulerAngles = new Vector3(curLook.y, 0, 0);
    }

    // ���� Ű �Է� �� IsGround�� ���ϰ��� ���� ����
    public void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }
    // ȣ�� �� ���� ���� �پ��ִ����� ���� ���θ� ����
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }
}
