using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float lifetime = 10f;
    //[SerializeField] ParticleSystem impact;

    protected Vector3 direct = Vector3.zero;



    private void OnEnable()
    {
        StartCoroutine(despawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator despawn()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);

    }

    private void Update()
    {
        
        //moving the bullet via it's transform(nto using psysics)
        transform.Translate(direct * speed * Time.deltaTime,Space.World);
    }

    //this function is to set the direction of the bullet 
    // the param name dir direction for the bullet to travel
    public void Fire(Vector3 dir)
    {
        
        direct = dir;
        
    }

    // this event is raised by unity when a collision occurs
    private void OnTriggerEnter(Collider other)
    {
        //checking the object that collided with our bullet and destroying if true
        if (other.CompareTag("Wall"))
        {
            
            
            gameObject.SetActive(false);
            
        }

        else if (other.CompareTag("Player"))
        {
            
            TankBody player = other.GetComponent<TankBody>();
            player.damage(1);
            
            gameObject.SetActive(false);
            
            
            

        }



    }





}
