using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    public Vector2 cooldownRange = new Vector2(2,4);
    public float speed = 5;
    public GameObject carInstance;
    List<GameObject> instances = new List<GameObject>();
    public float maxDist = 100;

    float _cooldown;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _cooldown -= Time.deltaTime;
        if (_cooldown < 0)
        {
            _cooldown = Random.Range(cooldownRange.x, cooldownRange.y);
            GameObject instance = Instantiate(carInstance);
            instances.Add(instance);
            CarMotion car = instance.GetComponent<CarMotion>();
            instance.transform.rotation = Quaternion.LookRotation(transform.forward,Vector3.up);
            instance.transform.position = transform.position;
            if (car)
            {
                car.speed = speed;
            }
        }

        GameObject toRemove = null;
        foreach (GameObject instance in instances)
        {
            if (Vector3.Distance(instance.transform.position, transform.position) > maxDist)
            {
                toRemove = instance;
                Destroy(instance);
            }
        }
        if (toRemove != null)
            instances.Remove(toRemove);
    }
}
