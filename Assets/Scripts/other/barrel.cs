using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrel : MonoBehaviour
{

    [SerializeField] GameObject explosion;
    [SerializeField] private float maxsize = 7f;
    [SerializeField] private float speed = 10f;
    private float currentsize;
    private float originalsize;
    
    
    private void Start()
    {
        originalsize = explosion.transform.localScale.x;
        currentsize = explosion.transform.localScale.x;
        LevelManager.Instance.RegisterBarrel(this);
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            explosion.SetActive(true);
            StartCoroutine(explode());
        }
        if (other.CompareTag("Explosion"))
        {
            
            explosion.SetActive(true) ;
            StartCoroutine(explode());
        }
        
        
            
    }

    IEnumerator explode()
    {
        

        currentsize = explosion.transform.localScale.x;

        while (currentsize < maxsize)
        {
            currentsize = Mathf.MoveTowards(currentsize,maxsize,Time.deltaTime*speed);

            explosion.transform.localScale = currentsize * Vector3.one;
            yield return null;
        }
        
        yield return new WaitForSeconds(0.25f);

        explosion.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Reset()
    {

       
        explosion.transform.localScale = originalsize*Vector3.one;
        gameObject.SetActive(true);
    }
}
