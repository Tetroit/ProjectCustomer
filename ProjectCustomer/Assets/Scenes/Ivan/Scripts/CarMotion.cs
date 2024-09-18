using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarMotion : MonoBehaviour
{
    public float wheelRadius;
    public Transform[] frontWheels;
    public Transform[] backWheels;
    public float speed;
    public Vector3 frontAxis = Vector3.right;
    public Vector3 backAxis = Vector3.right;
    float frontRotation = 0;
    float backRotation = 0;
    float backSpeed = 0;

    [Range(-1f,1f)]
    public float steerRotation;

    float baseLength { get { return frontAxisDist + backAxisDist; } }

    public float steerRadius;
    public float backAxisDist;
    public float frontAxisDist;
    public Vector3 rotationPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        backAxis = transform.right;
        speed = Mathf.Sin(Time.time) * 5;
        float step = speed * Time.deltaTime;

        if (steerRotation < -0.01f || steerRotation > 0.01f)
            UpdateSteering(step);
        else
        {
            backSpeed = speed;
            transform.localPosition += step * transform.forward;
        }

        frontRotation += Mathf.Rad2Deg * speed / wheelRadius * Time.deltaTime;
        backRotation += Mathf.Rad2Deg * backSpeed / wheelRadius * Time.deltaTime;
        foreach (Transform t in frontWheels)
        {
            t.localRotation *= Quaternion.AngleAxis(frontRotation, Vector3.right);
        }
        foreach (Transform t in backWheels)
        {
            t.localRotation *= Quaternion.AngleAxis(backRotation, Vector3.right);
        }
    }

    private void UpdateSteering(float step)
    {
        Vector3 backBar = transform.position - backAxisDist * transform.forward; 
        Vector3 frontBar = transform.position + frontAxisDist * transform.forward;
        float centerFac = backAxisDist/baseLength;

        steerRadius = baseLength / Mathf.Tan(steerRotation);
        float frontSteerRadius = Mathf.Sqrt(steerRadius *steerRadius + baseLength*baseLength);
        backSpeed = speed / frontSteerRadius * steerRadius;
        float turnAngle = step / frontSteerRadius;
        rotationPoint = backBar + steerRadius * backAxis;

        transform.RotateAround(rotationPoint, Vector3.up, ((steerRotation > 0) ? turnAngle : -turnAngle) * Mathf.Rad2Deg);

        frontAxis = Quaternion.AngleAxis(steerRotation * Mathf.Rad2Deg, transform.up) * backAxis;
        foreach (Transform t in frontWheels)
        {
            t.localRotation = Quaternion.AngleAxis(steerRotation * Mathf.Rad2Deg, transform.up);
        }
        foreach (Transform t in backWheels)
        {
            t.localRotation = Quaternion.identity;
        }
        //Quaternion turnRotation = Quaternion.Euler(0, turnAngle, 0);

        //Vector3 newFrontBar = turnRotation * (frontBar - rotationPoint) + rotationPoint;
        //Vector3 newBackBar = turnRotation * (backBar - rotationPoint) + rotationPoint;

        //transform.R Vector3.Angle(transform.forward, newFrontBar - newBackBar);
    }
    private void OnDrawGizmos()
    {
        foreach (Transform t in backWheels)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, wheelRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(t.position, t.position + backAxis);
        }
        foreach (Transform t in frontWheels)
        {
            Gizmos.color = new Color(0,0.2f,0.8f);
            Gizmos.DrawWireSphere(t.position, wheelRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(t.position, t.position + frontAxis);
        }

        Gizmos.DrawSphere(rotationPoint,0.1f);
        Gizmos.color = new Color(0.6f, 1, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * frontAxisDist);
        Gizmos.color = new Color(0.9f, 0.9f, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * backAxisDist);

    }
}
