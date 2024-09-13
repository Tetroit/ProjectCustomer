using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class NoiseManager : MonoBehaviour
{

    public ScriptableRendererFeature _noise;
    public Material _noiseMaterial;
    public bool showInEditor = false;
    void Start()
    {
    }

    void Update()
    {

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
