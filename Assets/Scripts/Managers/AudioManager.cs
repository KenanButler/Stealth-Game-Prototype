using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    

    private void Awake()
    {
        
        instance = this;
    }

    public void LoadLevelComplete()
    {
        if ( LevelManager.Instance)
        {
            AudioClip music = LevelManager.Instance.LevelMusic;
            if (music)
            {
                musicSource.clip = music;
                musicSource.loop = true;
                musicSource.spatialBlend = 0f;
                musicSource.Play();
            }
            FadeIn();
        }
    }
    public float masterVol;
    [SerializeField] private AudioMixer mixer;

    private void Start()
    {
        mixer.GetFloat("MasterVol", out masterVol);
    }

    public void FadeIn()
    {
        StartCoroutine(LerpVol(-80f, masterVol, 2f));
    }

    public IEnumerator FadeOut()
    {
        
        yield return StartCoroutine(LerpVol(masterVol,-80, 1f));
    }

    IEnumerator LerpVol(float startVol, float endVol, float dur)
    {
        float currVol = startVol;
        float currTime = 0f;

        while(currTime < dur)
        {
            
            currTime += Time.deltaTime;
            currTime = Mathf.Clamp(currTime, 0f, dur);

            currVol = Mathf.Lerp(startVol, endVol, currTime / dur);

            mixer.SetFloat("MasterVol", currVol);
            yield return null;
        }
    }

    public void updateMixer()
    {
        float MasterAudio = GameManager.Instance.musicData.MasterAudio;
        float EffectAudio = GameManager.Instance.musicData.EffectAudio;
        float MusicAudio = GameManager.Instance.musicData.MuiscAudio;

        mixer.SetFloat("MasterVol", MasterAudio);
        mixer.SetFloat("SFX", EffectAudio);
        mixer.SetFloat("Music", MusicAudio);
    }
}
