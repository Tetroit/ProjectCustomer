using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Playables;
using UnityEngine.Events;

public enum EInputDevice
{
    Keyboard,
    Joystick
}
public class PlayerControlsIvan : MonoBehaviour, ISaveData
{
    public DialogueManager dialogueManager;

    public float movementSpeed = 5f;
    public float minAngle = 20f;
    public float maxAngle = 35f;
    [Range(0f, 1f)]
    public float cameraSmoothness = 0.5f;
    public bool isMoving;
    public bool lockControls = false;
    public bool isGrounded;
    public bool isSafe;
    public Vector3 lastSafePos;

    public float criticalAngle = 30f;
    public float jumpHeight = 5f;

    public List<ContactPoint> contacts = new List<ContactPoint>();

    public InputDevice inputDevice;

    Vector2 turn;
    Vector2 turnTarget;
    
    public Vector3 castOffset;
    public float castRadius;

    public Transform cameraTransform;
    Rigidbody rb;

    public Transform candlePlayerPos;
    public GameObject candle;

    public UnityEvent OnCandleAnimationEnd;

    private void Start()
    {
        isSafe = true;

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = transform.rotation;
        }

        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isSafe)
            lastSafePos = transform.position;
        if (GameBrain.main.gameState == GameState.GAME)
        {
            LookAround();

            if (dialogueManager != null && !dialogueManager.isDialogueFinished && Input.GetKeyUp(KeyCode.F))
            {
                dialogueManager.DisplayNextSentence();
            }


        }
    }

    private void FixedUpdate()
    {
        if (GameBrain.main.gameState == GameState.GAME)
        {
            lockControls = false;
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
        turnTarget.x += Input.GetAxis("Mouse X") * Settings.main.mouseSensitivity;
        turnTarget.y += Input.GetAxis("Mouse Y") * Settings.main.mouseSensitivity;

        turnTarget.y = Mathf.Clamp(turnTarget.y, minAngle, maxAngle);
        turn = Vector2.Lerp(turnTarget, turn, cameraSmoothness);

        if (cameraTransform)
            cameraTransform.localRotation = Quaternion.Euler(-turn.y, 0f, 0f);

        transform.rotation = Quaternion.Euler(0f, turn.x, 0f);
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
    public void LoadData(GameData gameData)
    {
        transform.position = gameData.playerPos;
        Debug.Log("player loaded");
    }
    public void SaveData(ref GameData gameData)
    {
        gameData.playerPos = transform.position; 
    }

    public void ResetData()
    {
        
    }


    public IEnumerator CandleAnimation()
    {
        candle.GetComponent<Candle>().Extinguish();
        candle.SetActive(false);

        GameBrain.main.ChangeGameState(GameState.CUTSCENE);
        float animationTime = 6;
        float currentAnimationTime = 0;
        float lerpKeyframe = 2;

        bool candleLit = false;

        float candleLightKeyframe = 3f;

        lockControls = true;
        rb.isKinematic = true;

        turn.y = -candlePlayerPos.rotation.eulerAngles.x;
        turn.x = candlePlayerPos.rotation.eulerAngles.y;
        turnTarget = turn;

        while (currentAnimationTime < lerpKeyframe)
        {
            float fac = currentAnimationTime / lerpKeyframe;
            transform.position = Vector3.Lerp(transform.position, candlePlayerPos.position, 0.02f);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, candlePlayerPos.rotation, 0.02f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, candlePlayerPos.rotation.eulerAngles.y, 0), 0.02f);

            currentAnimationTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        candle.SetActive(true);
        Animation anim = candle.GetComponent<Animation>();
        anim.Play();

        while (currentAnimationTime < animationTime)
        {
            float fac = (currentAnimationTime - lerpKeyframe) / currentAnimationTime;
            //transform.position = Vector3.Lerp(transform.position, Vector3.zero, 0.01f);

            if (currentAnimationTime > candleLightKeyframe && !candleLit)
            {
                candleLit = true;
                candle.GetComponent<Candle>().LightUp();
            }

            currentAnimationTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //candle.transform.position = candlePlacePos.transform.position;
        //candle.transform.rotation = candlePlacePos.transform.rotation;
        rb.isKinematic = false;
        lockControls = false;
        GameBrain.main.ChangeGameState(GameState.GAME);
        GlobalData.instance.UpdateStory(GlobalData.MainScriptState.CHURCH_CANDLE_LIT);

        OnCandleAnimationEnd?.Invoke();
        yield return null;
    }

    public void PlayCandleAnimation()
    {
        if (GlobalData.instance.storyProgress == GlobalData.MainScriptState.CHURCH)
            StartCoroutine(CandleAnimation());
    }
}