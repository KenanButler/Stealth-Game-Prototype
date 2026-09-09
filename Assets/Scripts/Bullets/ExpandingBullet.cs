using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEngine.Rendering.RendererUtils;

public class ExpandingBullet : Bullet
{
    [SerializeField] private float expandsize = 3f;
    [SerializeField] private float expandspeed;
    
    private Rigidbody rb;
    [SerializeField] ParticleSystem smoke;

    public bool guardInSmoke = false;
    
    List<GuardController> guards = new List<GuardController>();


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

   


    private void FixedUpdate()
    {
        Debug.Log("guards:  "+guards.Count);
        Vector3 movedirect = direct * speed * Time.fixedDeltaTime;
        //telling the rigidbody to move position
        rb.MovePosition(transform.position + movedirect);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            //making bullet stop its movement and physics
            direct = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
            smoke.Play();

            //start coroutine
            StartCoroutine(Expand());

        }
        if (other.CompareTag("Enemy"))
        {

            GuardController guard = other.GetComponent<GuardController>();
            guards.Add(guard);
            guardInSmoke = true;
            guard.SmokeCheck(guardInSmoke);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GuardController guard = other.GetComponent<GuardController>();
            guards.Remove(guard);
            if (guards.Count == 1)
            {
                guardInSmoke = false;
            }
            
            guard.SmokeCheck(guardInSmoke);
        }
    }

    

    IEnumerator Expand()
    {
        //while loop to expand bullet till it reaches certain size

        Vector3 ogscale = transform.localScale;
        float currentscale = ogscale.x;

        while(currentscale < expandsize)
        {
            //moving the value towards expand size based on time
            currentscale = Mathf.MoveTowards(currentscale, expandsize, expandspeed * Time.deltaTime);
            
            //multipling the alue back into a vector
            transform.localScale = currentscale * Vector3.one;
            yield return null;
        }
        yield return StartCoroutine(Fade());

        smoke.Stop();
        if (guardInSmoke)
        {
            foreach(GuardController guard in guards)
            {
                guard.SmokeCheck(false);
            }
        }
        Destroy(gameObject);
    }

    IEnumerator Fade(float fadeamount = .025f, float fadewait = .025f)
    {
        Renderer renderer = GetComponent<Renderer>();

        float alpha = renderer.material.color.a;
        
        for( ; alpha >=0; alpha -= fadeamount)
        {
            
            Color c = renderer.material.color;
            c.a = alpha;
            renderer.material.color = c;

            yield return new WaitForSeconds(fadewait);

        }

    }
    
}
