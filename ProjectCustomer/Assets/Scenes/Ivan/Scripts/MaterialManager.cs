using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    float recognitionDistance = 10;
    public List<Material> materials = new List<Material>();
    public static MaterialManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        RemoveRecognisable(0);
    }
    public void SetRecognisable(int materialID)
    {
        materials[materialID].SetFloat("_Distance", recognitionDistance);
    }
    public void RemoveRecognisable(int materialID)
    {
        materials[materialID].SetFloat("_Distance", 0);
    }

    public void SetRecognisable(Material material)
    {
        material.SetFloat("_Distance", recognitionDistance);
    }
    public void RemoveRecognisable(Material material)
    {
        material.SetFloat("_Distance", 0);
    }
}
