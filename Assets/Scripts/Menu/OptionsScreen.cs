using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider EffectSlider;
    [SerializeField] Slider MusicSlider;
    
    

    float MasterAudio;
    float EffectAudio;
    float MusicAudio;

    private void Start()
    {
        
        
        MasterAudio = GameManager.Instance.musicData.MasterAudio;
        EffectAudio = GameManager.Instance.musicData.EffectAudio;
        MusicAudio = GameManager.Instance.musicData.MuiscAudio;

        

        MasterSlider.value = MasterAudio;
        EffectSlider.value = EffectAudio;
        MusicSlider.value = MusicAudio;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager.Instance.ExitOption();
        }
    }

    public void ChangeMasterVol()
    {
        GameManager.Instance.musicData.MasterAudio = MasterSlider.value;
        MasterAudio = MasterSlider.value;
        mixer.SetFloat("MasterVol",MasterAudio);
        AudioManager.Instance.masterVol = MasterAudio;
        
        FileHandler.Save(GameManager.Instance.musicData, FileHandler.SaveType.Binary);
    }
    public void ChangeEffectVol()
    {
        GameManager.Instance.musicData.EffectAudio = EffectSlider.value;
        EffectAudio = EffectSlider.value;
        mixer.SetFloat("SFX", EffectAudio);
        
        FileHandler.Save(GameManager.Instance.musicData, FileHandler.SaveType.Binary);
    }

    public void ChangeMusicVol()
    {
        GameManager.Instance.musicData.MuiscAudio = MusicSlider.value;
        MusicAudio = MusicSlider.value;
        mixer.SetFloat("Music", MusicAudio);
        
        FileHandler.Save(GameManager.Instance.musicData, FileHandler.SaveType.Binary);


    }

    



}
