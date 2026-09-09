using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refill : MonoBehaviour
{
    

    private TankBody body;

    private void Start()
    {
        LevelManager.Instance.RegisterRefills(this);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            body = other.GetComponent<TankBody>();
            
            body.animator.SetBool("Reload", true);
            

            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
