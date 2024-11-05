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

    // 키 입력시 curMovementInput 값 변경
    private void OnMove(InputValue value)
    {
        curMovementInput = value.Get<Vector2>();
    }
    // curMovementInput 값에 따라 플레이어 이동
    private void Move()
    {
        Vector3 dir = ((transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x)).normalized * moveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    // 마우스 움직임시 curLook 값 변경
    private void OnLook(InputValue value)
    {
        Vector2 delta = value.Get<Vector2>() * lookSensitivity;
        curLook.x += delta.x;
        curLook.y = Mathf.Clamp(curLook.y - delta.y, minRotation, maxRotation);
    }
    // curLook 값에 따라 캐릭터 및 카메라 회전
    private void CameraLook()
    {
        transform.eulerAngles = new Vector3(0, curLook.x, 0);
        CameraContainer.localEulerAngles = new Vector3(curLook.y, 0, 0);
    }

    // 점프 키 입력 시 IsGround의 리턴값에 따라 점프
    public void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }
    // 호출 시 현재 땅에 붙어있는지에 대한 여부를 리턴
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
