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

    public float moveSpeed = 3.0f;

    private float minRotation = -60f;
    private float maxRotation = 80f;
    private float lookSensitivity = 0.03f;

    private void OnMove(InputValue value)
    {
        curMovementInput = value.Get<Vector2>();
    }
    private void Move()
    {
        Vector3 dir = ((transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x)).normalized * moveSpeed;
        rb.velocity = dir;
    }

    private void OnLook(InputValue value)
    {
        Vector2 delta = value.Get<Vector2>() * lookSensitivity;
        curLook.x += delta.x;
        curLook.y = Mathf.Clamp(curLook.y - delta.y, minRotation, maxRotation);
    }
    private void CameraLook()
    {
        transform.eulerAngles = new Vector3(0, curLook.x, 0);
        CameraContainer.localEulerAngles = new Vector3(curLook.y, 0, 0);
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
