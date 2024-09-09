using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private float mouseSensitivity = 35f;
    private float movementSpeed = 5f;
    private float minAngle = 20f;
    private float maxAngle = 35f;

    Vector2 turn;

    public Transform cameraTransform;

    void Update()
    {
        LookAround();
        Move();
    }

    private void LookAround()
    {
        turn.x += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        turn.y += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        turn.y = Mathf.Clamp(turn.y, -minAngle, maxAngle);

        cameraTransform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0f);
        transform.localRotation = Quaternion.Euler(0f, turn.x, 0f);
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.zero;

        if(Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
        }

        if(Input.GetKey(KeyCode.A))
        {
            moveDirection += -transform.right;
        }

        if(Input.GetKey(KeyCode.S))
        {
            moveDirection += -transform.forward;
        }

        if(Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
        }

        moveDirection = moveDirection.normalized * movementSpeed * Time.deltaTime;

        transform.Translate(moveDirection, Space.Self);
    }
}
