using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingsounds : MonoBehaviour
{
   public AudioSource footStepSource;

    [SerializeField]
    public List<AudioClip> footStepSounds;
    private void Start()
    {
        footStepSource = GetComponent<AudioSource>();
        playFootSteps();
    }
    void playFootSteps()
    {if (footStepSounds.Count > 0)
        {
            footStepSource.loop = true;
            footStepSource.clip = footStepSounds[Random.Range(0, footStepSounds.Count)];
            footStepSource.Play();
        }
    }
}
