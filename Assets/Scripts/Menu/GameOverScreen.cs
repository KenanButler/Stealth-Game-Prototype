using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void PlayAgain()
    {
        GameManager.Instance.PlayerRespawn();
        gameObject.SetActive(false);
    }

    public void DontPlayAgain()
    {
        GameManager.Instance.LoadMainMenu();
        gameObject.SetActive(false);
    }
}
