using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVisionScrip : MonoBehaviour
{

    [SerializeField]
    private GameObject guard;
    private GuardController controller;
    

    private void Awake()
    {
        controller = guard.GetComponent<GuardController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            controller.spotted();
            
         
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.notSpotted();
        }
        
    }
}
