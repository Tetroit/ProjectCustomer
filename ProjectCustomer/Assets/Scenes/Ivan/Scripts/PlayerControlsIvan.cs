using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInputDevice
{
    Keyboard,
    Joystick
}
public class PlayerControlsIvan : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float minAngle = 20f;
    public float maxAngle = 35f;
    [Range(0f, 1f)]
    public float cameraSmoothness = 0.5f;
    public bool isMoving;

    public InputDevice inputDevice;

    Vector2 turn;
    Vector2 turnTarget;

    public Transform cameraTransform;

    private void Start()
    {
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (GameBrain.main.gameState == GameState.GAME)
        {
            Move();
            LookAround();
        }
    }
    private void FixedUpdate()
    {
    }

    private void LookAround()
    {
        turnTarget.x += Input.GetAxis("Mouse X") * Settings.main.mouseSensitivity.x * Time.deltaTime;
        turnTarget.y += Input.GetAxis("Mouse Y") * Settings.main.mouseSensitivity.y * Time.deltaTime;

        turnTarget.y = Mathf.Clamp(turnTarget.y, minAngle, maxAngle);
        turn = Vector2.Lerp(turnTarget, turn, cameraSmoothness);

        cameraTransform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);
        transform.rotation = Quaternion.Euler(0f, turn.x, 0f);
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.zero;

        if (inputDevice == InputDevice.Joystick)
        {
            moveDirection += Input.GetAxisRaw("Vertical") * transform.forward;
            moveDirection += Input.GetAxisRaw("Horizontal") * transform.right;
        }
        if (inputDevice == InputDevice.Keyboard)
        {
            if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
            if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
            if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;
            if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;
        }

        if (moveDirection.magnitude > 0.1f) isMoving = true;
        else isMoving = false;

        moveDirection = moveDirection.normalized * movementSpeed * Time.deltaTime;

        transform.position += moveDirection;
        cameraTransform.position = transform.position;
    }
}