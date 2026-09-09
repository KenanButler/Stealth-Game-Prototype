using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    public bool canPause = false;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && canPause)
        {
            
            Pause(isPaused);
        }
    }

    private void Pause(bool pause)
    {
        isPaused = pause;
        canvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }
}
