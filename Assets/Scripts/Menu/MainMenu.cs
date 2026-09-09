using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button continueBtn;

    private void OnEnable()
    {
        if (GameManager.Instance.saveData.currLevel != 0)
        {
            continueBtn.interactable = FileHandler.SavePresent;
        }
        
    }

    public void NewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void Continue()
    {
        GameManager.Instance.Continue();
    }

    public void Options()
    {
        GameManager.Instance.OpenOptions();
    }

    public void Exit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
