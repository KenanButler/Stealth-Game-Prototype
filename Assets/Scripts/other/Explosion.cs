using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private TankBody body;
    private GuardController controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            body = other.gameObject.GetComponent<TankBody>();
            body.damage(1);
            body.StartCoroutine(body.Burning());
            
            
        }
        if (other.CompareTag("Enemy"))
        {
            controller = other.gameObject.GetComponent<GuardController>();
            controller.decrhealth(100);
        }
    }

}
