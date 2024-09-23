using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Candle : MonoBehaviour
{
    public ParticleSystem fire;
    public bool isLit;

    void Start()
    {
        
    }
    private void OnValidate()
    {
        if (isLit)
            LightUp();
        else
            Extinguish();
    }
    public void LightUp()
    {
        isLit = true;
        ParticleSystem.EmissionModule em = fire.emission;
        em.enabled = true;
    }
    public void Extinguish()
    {
        isLit = false;
        ParticleSystem.EmissionModule em = fire.emission;
        em.enabled = false;
    }
}
