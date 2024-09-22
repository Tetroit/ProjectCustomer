using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class NoiseZone : MonoBehaviour
{
    BoxCollider zone;
    public PlayerControlsIvan player;

    public static List<NoiseZone> collection;
    public float falloff = 5;
    public float value;
    public float fac { private set; get; }

    public static float maxFac;

    static float zonesUpdated = 0;

    private void Awake()
    {
        if (collection == null)
            collection = new List<NoiseZone>();
        if (!collection.Contains(this))
             collection.Add(this);
    }
    private void OnDestroy()
    {
        if (collection != null && collection.Contains(this))
            collection.Remove(this);
    }
    void Start()
    {
        if (player == null)
        {

        }
        player = FindAnyObjectByType<PlayerControlsIvan>();
        zone = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collection.Contains(this))
        {
            float distFac = Mathf.Clamp01(1 - Vector3.Distance(zone.ClosestPoint(player.transform.position), player.transform.position) / falloff);
            fac = distFac * value;
            if (fac > maxFac)
                maxFac = fac;

            zonesUpdated++;
            if (zonesUpdated == collection.Count)
            {
                Debug.Log("" + zonesUpdated + " " + distFac);
                NoiseManager.instance.fac = maxFac;
                zonesUpdated = 0;
                maxFac = 0;
            }
        }
    }
}
