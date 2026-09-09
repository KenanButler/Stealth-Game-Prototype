using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;

public class TankBody : MonoBehaviour
{
    //public so other scripts ca set values
    [HideInInspector] public float verticaldirect;
    [HideInInspector] public float horizontaldirect;
    [HideInInspector] public float rotatedirect;

    [SerializeField] private float movementspeed = 5f;
    

    public enum BulletTypes { Default, Expanding }
    [SerializeField] private GameObject[] bullets;

    [SerializeField] private Transform turret;
    public Transform Turret {  get { return turret; } }

    private Transform fireLocation;

    private Rigidbody rb;
    public Animator animator;
    //private Collider collider;

    [SerializeField] 
    public int maxhealth;

    public int health;
    [SerializeField] public bool invicible=false;
    [SerializeField] public float invincibleinterval;
    [SerializeField] public float time;
    

    [SerializeField] public int maxbullet = 6;
    [SerializeField] public int currentbullet = 6;

    [SerializeField] public int maxbomb = 3;
    [SerializeField] public int currentbomb = 3;

    [SerializeField] 
    ParticleSystem RifleBurst;
    [SerializeField]
    ParticleSystem burning;

    PlayerSounds sounds;

    

    
    private void Awake()
    {
        //finding ridgidbody attached to this object
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sounds = GetComponent<PlayerSounds>();
        
        
        
    }

    private void Start()
    {
        health = maxhealth;
        time = invincibleinterval;
        fireLocation = turret.transform.Find("FireLocation");
        
        
    }

    private void FixedUpdate()
    {
        if (animator.GetBool("Died") == false)
        {
            Movement();
            time += Time.deltaTime;
        }
        
        
        
        

        

        
    }

    private void Movement()
    {
        
        //                      using blue axis   * getting from input * setting in inspector * time to make is smooth 
        Vector3 zDirection = transform.forward * movementspeed * verticaldirect  * Time.fixedDeltaTime;
        Vector3 yDirection = transform.right * movementspeed * horizontaldirect * Time.fixedDeltaTime;
        //Vector3 horizontalDirection = transform.right * movementspeed * horizontaldirect * Time.fixedDeltaTime;
        

        // add our desired direction vector to our current position so we can
        // determine where we are trying to move each fixed interval
        rb.MovePosition(transform.position + zDirection+yDirection);
    }

    public void UpdateAimDir(Vector3 aimdir)
    {
        aimdir.Normalize();

        turret.transform.rotation = Quaternion.LookRotation(aimdir);
    }

    

    

    public void Fire(BulletTypes bullettypes)
    {
        
        GameObject smokeUsed = GameObject.FindGameObjectWithTag("Smoke");
        animator.SetTrigger("Fire");
        //check if the bulelt being fired is the default or expanding
        //if the current amount of bullets for that type are equal to zero the bullet doesn't fire
        //if there ar emroe than zero bullet it fires and subtratced one from the current bullet of that type
        if (bullettypes.ToString() == "Default")
        {
            if (currentbullet == 0)
            {
                return;
            }
            else if (currentbullet >0)
            {
                
                currentbullet--;
               
            }
        }

        else if (bullettypes.ToString() == "Expanding")
        {
            if (currentbomb == 0)
            {
                return;
            }
            else if(smokeUsed != null)
            {
                return;
            }
            else if (currentbomb > 0)
            {
                currentbomb--;

            }
        }
        
        
        //direction for the bullet to travel
        Vector3 direct = fireLocation.position - transform.position;

        if (bullettypes.ToString() == "Default")
        {
            direct.y = 0;
        }
        else if (bullettypes.ToString() == "Expanding")
        {
            direct.y = 1.2f;
        }
            
        //if (bullettypes = BulletTypes.Expanding)

        direct.Normalize();

        //instantiate one of our bullet prefabs
        //Bullet bullet = Instantiate(bullets[(int)bullettypes], fireLocation.position, Quaternion.identity).GetComponent<Bullet>();
        

        if (bullettypes.ToString() == "Default")
        {
            GameObject bullet = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.Bullet);
            bullet.transform.position = fireLocation.position;
            bullet.transform.rotation = Quaternion.identity;

            Bullet bulletscript = bullet.GetComponent<Bullet>();

            RifleBurst.Play();

            sounds.PlayGunShot();
            //set it active
            bullet.gameObject.SetActive(true);
            //run it's fire() function
            bulletscript.Fire(direct);
        }
        else if (bullettypes.ToString() == "Expanding")
        {
            GameObject bullet = Instantiate(bullets[0], fireLocation.position, Quaternion.identity);

            

            Bullet bulletscript = bullet.GetComponent<Bullet>();

            RifleBurst.Play();
            
            //set it active
            bullet.gameObject.SetActive(true);
            //run it's fire() function
            
            bulletscript.Fire(direct);
        }


        
        

        
        
            
        
    }

    

    
    public void damage(int amount)
    {
        if (!invicible)
        {
            
            if(invincibleinterval <= time)
            {
                health -= amount;
                time = 0;
            }
            
            
        }
        if (health <= 0)
        {
            sounds.PlayerDeathSound();
            animator.SetBool("Died", true);
        }

    }

    public IEnumerator Burning()
    {
        

        
        burning.Play();
        
        yield return new WaitForSeconds(4.0f);
        
        burning.Stop();
        
    }

    public void Die()
    {
        
        GameManager.Instance.PlayerDied();
    }

    public void ResetGame()
    {
        animator.SetBool("Died",false);
        sounds.GameReset();
        health = maxhealth;
        currentbullet = maxbullet;
        currentbomb = maxbomb;
        
    }





    public void finishreload()
    {
        animator.SetBool("Reload", false);
        
    }
    
    
}
