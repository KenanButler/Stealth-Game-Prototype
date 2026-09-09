using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region LazySingleton

    private static LevelManager instance;
    public static LevelManager Instance => instance;

    [SerializeField] private Transform spawn;
    public Transform Spawn => spawn;


    private void Awake()
    {
        if (!instance) instance = this;

        
    }

    #endregion

    private List<GuardController> guards = new List<GuardController>();

    private List<barrel> barrels = new List<barrel>();

    private List<refill> refills = new List<refill>();

    private List<button> buttons = new List<button>();

    [SerializeField] AudioClip levelMusic;
    public AudioClip LevelMusic => levelMusic;


    public void RegisterEnemy(GuardController guard)
    {
        guards.Add(guard);
    }

    public void RegisterBarrel(barrel barrel)
    {
        barrels.Add(barrel);
    }

    public void RegisterRefills(refill refill)
    {
        refills.Add(refill);
    }

    public void RegisterButton(button button)
    {
        buttons.Add(button);
    }

    private bool CheckConditions()
    {
        foreach (var item in guards)
        {
            if (!item.IsDead)
            {
                return false;
            }
                
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject == GameManager.Instance.playerBody))
        {
            
            
            GameManager.Instance.LevelComplete();
            
            
        }
    }

    public void ResetGame()
    {
        
        foreach (var item in guards)
        {
            
            item.Reset();
        }

        foreach (var item in barrels)
        {
            item.Reset();
        }

        foreach (var item in refills)
        {
            item.Reset();
        }

        foreach (var item in buttons)
        {
            item.Reset();
        }
    }

    
}
