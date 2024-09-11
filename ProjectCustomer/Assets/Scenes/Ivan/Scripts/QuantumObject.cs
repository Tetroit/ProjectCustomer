
using UnityEngine;

public class QuantumObject : MonoBehaviour
{
    public Renderer renderer;
    bool isVisible;
    public Bounds teleportBounds;
    public BoxCollider objectArea;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void TeleportRandomly()
    {
        if (Camera.main != null)
        {
            Debug.Log("telepot");
            Vector3 possibleLocation = RandomVector(teleportBounds);
            Bounds newBounds = new Bounds(objectArea.center + possibleLocation, objectArea.size);

            if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), newBounds))
            {
                transform.position = possibleLocation;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    void OnBecameInvisible()
    {
        TeleportRandomly();
    }
    Vector3 RandomVector(Bounds bounds)
    {
        Vector3 res = Vector3.zero;
        res.x = Random.Range(bounds.min.x, bounds.max.x);
        res.y = Random.Range(bounds.min.y, bounds.max.y);
        res.z = Random.Range(bounds.min.z, bounds.max.z);
        return res;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //foreach (Plane p in GeometryUtility.CalculateFrustumPlanes(Camera.main))
        //{
        //    Vector3 point = Camera.main.transform.forward + Camera.main.transform.position;
        //    Vector3 projection = p.ClosestPointOnPlane(point);
        //    Gizmos.DrawLine(point, projection);
            
        //}
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(teleportBounds.center, teleportBounds.size);
    }
}
