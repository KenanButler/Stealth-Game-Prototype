using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] AudioSource gunSource, deathSource;
    [SerializeField] AudioClip gunClip;
    [SerializeField] AudioClip deathClip;

    public void PlayGunShot()
    {
        gunSource.clip = gunClip;
        gunSource.Play();
    }

    public void PlayerDeathSound()
    {
        deathSource.clip = deathClip;
        deathSource.pitch = Random.Range(0.9f,1.1f);
        deathSource.Play();
    }

    public void GameReset()
    {
        deathSource.Stop();
        gunSource.Stop();
    }
}
