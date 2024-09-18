using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.FullSerializer;

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
    public bool lockControls = false;
    public bool isGrounded;

    public float criticalAngle = 30f;
    public float jumpHeight = 5f;

    public List<ContactPoint> contacts = new List<ContactPoint>();

    public InputDevice inputDevice;

    Vector2 turn;
    Vector2 turnTarget;
    Vector3 groundNormal;
    
    public Vector3 castOffset;
    public float castRadius;

    public Transform cameraTransform;
    Rigidbody rb;

    private void Start()
    {
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position;
            cameraTransform.localRotation = transform.rotation;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
    }
    private void FixedUpdate()
    {

        if (GameBrain.main.gameState == GameState.GAME)
        {
            lockControls = false;
            LookAround();
            Move();
        }

        if (GameBrain.main.gameState == GameState.INVENTORY ||
            GameBrain.main.gameState == GameState.SETTINGS)
        {
            lockControls = true;
            Move();
        }

        contacts.Clear();
    }

    private void LookAround()
    {
        turnTarget.x += Input.GetAxis("Mouse X") * Settings.main.mouseSensitivity * 0.02f;
        turnTarget.y += Input.GetAxis("Mouse Y") * Settings.main.mouseSensitivity * 0.02f;

        turnTarget.y = Mathf.Clamp(turnTarget.y, minAngle, maxAngle);
        turn = Vector2.Lerp(turnTarget, turn, cameraSmoothness);

        if (cameraTransform)
            cameraTransform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);

        rb.rotation = Quaternion.Euler(0f, turn.x, 0f);
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.zero;
        bool shouldJump = false;
        isGrounded = false;
        List<Vector3> constraints = new List<Vector3>();

        //----------------GROUND CHECK-------------------------

        foreach (ContactPoint c in contacts)
        {
            float flatness = Vector3.Dot(Vector3.up, c.normal);
            if (flatness > Mathf.Cos(criticalAngle))
            {
                isGrounded = true;
                groundNormal = c.normal;
            }
        }
        //if (!isGrounded)
        //{
        //    RaycastHit hit;
        //    if (Physics.SphereCast(rb.position + castOffset, castRadius, Vector3.down, out hit))
        //    {
        //        if (hit.distance < castRadius ) isGrounded = true;
        //        Debug.Log(hit.collider.name);
        //    }
        //}

        //----------------INPUT READ--------------------------

        if (!lockControls)
        {
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

                if (Input.GetKey(KeyCode.Space) && isGrounded) shouldJump = true; 
            }
        }

        if (moveDirection.magnitude > 0.1f)
        {
            isMoving = true;
            if (GlobalData.instance.GetUIHintStatus(GlobalData.EHintBit.CONTROLS))
                GlobalData.instance.SetUIHintVisibility((int)GlobalData.EHintBit.CONTROLS, false);
        }
        else isMoving = false;

        //-------------COLLISION RESOLUTION-------------

        moveDirection = moveDirection.normalized * movementSpeed;

        Vector3 rbCopy = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        foreach (ContactPoint c in contacts)
        {
            float dot = Vector3.Dot(c.normal, rbCopy);
            if (dot < 0f)
            {
                constraints.Add(c.normal);
            }
        }

        constraints.Add(rbCopy);

        Orhtogonalise(constraints);

        rb.velocity = rbCopy;
        //rb.velocity = constraints[constraints.Count - 1];


        // -----------------JUMP-----------------
        if (shouldJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
        }

        if (cameraTransform)
            cameraTransform.position = transform.position;
    }

    //to learn more about the thing down here google "Gram Schmidt process"
    public void Orhtogonalise(List<Vector3> vecs)
    {
        for (int i=1; i<vecs.Count; i++)
        {
            for (int j=0; j<i; j++)
            {
                vecs[i] -= Vector3.Project(vecs[i], vecs[j]);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //List<ContactPoint> currentContacts = new List<ContactPoint>();
        List<ContactPoint> currentContacts = new List<ContactPoint>();
        collision.GetContacts(currentContacts);
        contacts.AddRange(currentContacts);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + castOffset, castRadius);

        foreach(var contact in contacts)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(contact.point, contact.point + contact.normal);
        }
    }
}