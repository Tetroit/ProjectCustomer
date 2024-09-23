using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Candle : MonoBehaviour
{
    public Light light;
    public ParticleSystem fire;
    public bool isLit;

    void Start()
    {
        if (light == null)
            light = GetComponentInChildren<Light>();
    }
    private void Update()
    {
        if (Application.isPlaying)
            light.intensity = Random.Range(0.8f, 1.2f);
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
        if (light != null)
            light.gameObject.SetActive(true);
        ParticleSystem.EmissionModule em = fire.emission;
        em.enabled = true;
    }
    public void Extinguish()
    {
        isLit = false;

        if (light != null)
            light.gameObject.SetActive(false);
        ParticleSystem.EmissionModule em = fire.emission;
        em.enabled = false;
    }
}
