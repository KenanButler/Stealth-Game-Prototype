using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] AudioSource gunSource, DeathSource, spottedSource;
    [SerializeField] AudioClip gunClip;
    [SerializeField] AudioClip spottedClip;
    [SerializeField] AudioClip[] deathClips;
    

    public void PlayFireSound()
    {
        gunSource.clip = gunClip;
        gunSource.Play();
    }

    public void PlayDeathSound()
    {
        DeathSource.clip = deathClips[Random.Range(0,deathClips.Length)];
        
        DeathSource.Play();
    }
    public void PlaySpottedSound()
    {
        spottedSource.clip = spottedClip;
        spottedSource.Play();
    }
}
