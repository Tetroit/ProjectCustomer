using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[ExecuteInEditMode]
public class NoiseManager : MonoBehaviour
{

    public ScriptableRendererFeature _noise;
    public Material _noiseMaterial;
    public bool showInEditor = false;

    [Range(0f, 1f)]
    public float fac = 0;
    public float intensity
    {
        get { return _noiseMaterial.GetFloat("_NoiseScale"); }
        set { _noiseMaterial.SetFloat("_NoiseScale", value); }
    }
    public float distanceScale
    {
        get { return _noiseMaterial.GetFloat("_DistanceScale"); }
        set { _noiseMaterial.SetFloat("_DistanceScale", value); }
    }
    public float distanceShift
    {
        get { return _noiseMaterial.GetFloat("_DistanceShift"); }
        set { _noiseMaterial.SetFloat("_DistanceShift", value); }
    }
    public float scale
    {
        get { return _noiseMaterial.GetFloat("_NoiseResolution"); }
        set { _noiseMaterial.SetFloat("_NoiseResolution", value); }
    }
    public float speed
    {
        get { return _noiseMaterial.GetFloat("_NoiseSpeed"); }
        set { _noiseMaterial.SetFloat("_NoiseSpeed", value); }
    }
    public bool noisePreview
    {
        get { return _noiseMaterial.GetInt("_NoisePreview") == 1; }
        set { _noiseMaterial.SetFloat("_NoisePreview", value ? 1 : 0); }
    }
    public bool distancePreview
    {
        get { return _noiseMaterial.GetInt("_DistancePreview") == 1; }
        set { _noiseMaterial.SetFloat("_DistancePreview", value ? 1 : 0); }
    }
    public Color color
    {
        get { return _noiseMaterial.GetColor("_NoiseColor"); }
        set { _noiseMaterial.SetColor("_NoiseColor", value); }
    }

    void Start()
    {
        if (Application.isPlaying)
        {
            //option 1
            distanceShift = -0.2f;
            intensity = 0.1f;
            scale = 1000f;
        }
    }
    void Update()
    {
        if (Application.isPlaying)
        {
            intensity = fac * 0.1f;
            //option 1
            distanceScale = fac * (Mathf.Sin(Time.time)*0.1f + 0.11f);
            color = Color.Lerp(Color.gray, Color.black, Mathf.Sin(Time.time));

        }
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            _noise.SetActive(true);
        }
    }
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (_noise)
            {
                if (showInEditor)
                    EnableNoise();
                else
                    DisableNoise();
            }
        }

    }

    public void EnableNoise()
    {
        _noise.SetActive(true);
    }
    public void DisableNoise()
    {
        _noise.SetActive(false);
    }
}
