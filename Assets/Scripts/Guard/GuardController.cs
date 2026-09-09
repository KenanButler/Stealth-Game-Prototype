using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;




[System.Serializable]
public abstract class GuardProperties
{
    public float speed = 3.0f;
    public float rotSpeed = 150.0f;

    

}
[System.Serializable]
public  class PatrolProperties : GuardProperties
{
    public bool isloop;
    public Transform[] Path;
}
[System.Serializable]
public class FoundProperties : GuardProperties
{
    
    //public GameObject player;
    public GameObject bullet;
    public float fireinterval;
}
[System.Serializable]
public class SearchProperties : GuardProperties
{
    //public GameObject player;
}

public class GuardController : AdvancedFSM
{
    
    [SerializeField]
    public  PatrolProperties patrolProperties;
    [SerializeField]
    public FoundProperties foundproperties;
    [SerializeField] 
    public SearchProperties searchproperties;
    public bool IsDead = false;
    [SerializeField]
    private Transform spawn;
    
    

    [SerializeField]
    private bool debugdraw;
    [SerializeField]
    //private Text statetext;
    //[SerializeField]
    //private Text healthtext;
    //[SerializeField]
    private GameObject bulletprefab;
    [SerializeField]
    private Transform gun;
    [SerializeField] GameObject vision;
    [SerializeField] GameObject visionmesh;

    //public bool isSmoke = false;

    [SerializeField] ParticleSystem rifleburst;


    private PatrolState patrolState;
    private FoundState foundState;
    private DeathState deathState;
    private SearchState searchState;

    EnemySounds sounds;


    


    
    

    private float health;

    public float Health
    {
        get { return health; }
      
    }

    public void decrhealth(float amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health <= 0)
        {
            sounds.PlayDeathSound();
        }
    }

    public void incrhealth(float amount)
    {
        health = Mathf.Min(100, health + amount);
    }

    

    private string getstatestring()
    {
        string state = "NONE";
        if (CurrentState.ID == FSMStateID.Dead)
        {
            state = "DEAD";
        }
        else if (CurrentState.ID == FSMStateID.Patrol)
        {
            state = "PATROL";
        }
        else if (CurrentState.ID == FSMStateID.Found)
        {
            state = "FOUND";
        }
        else if (CurrentState.ID == FSMStateID.Search)
        {
            state = "SEARCH";
        }

        //Debug.Log(state);
        return state;
    }

    protected override void Initialize()
    {
        Transform objplayer = GameManager.Instance.playerPoint;
        LevelManager.Instance.RegisterEnemy(this);
        sounds = GetComponent<EnemySounds>();

        playerTransform = objplayer.transform;
        
        
        health = 100;
        ConstructFSM();

        

    }

    protected override void FSMUpdate()
    {
        
        if (CurrentState != null)
        {
            getstatestring();
            CurrentState.Reason(playerTransform, transform);
            CurrentState.Act(playerTransform, transform);
            
            
        }

        

        if (debugdraw)
        {
            Debug.DrawRay(transform.position, transform.forward * 5.0f, Color.red);
        }
    }

    private void ConstructFSM()
    {

        patrolState = new PatrolState(this, transform, patrolProperties);

        patrolState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        patrolState.AddTransition(Transition.Spotted, FSMStateID.Found);

        foundState = new FoundState(this, transform, foundproperties);

        foundState.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        foundState.AddTransition(Transition.Lostsight, FSMStateID.Search);

        searchState = new SearchState(this, transform, searchproperties);

        searchState.AddTransition(Transition.NoHealth,FSMStateID.Dead);
        searchState.AddTransition(Transition.Spotted, FSMStateID.Found);
        searchState.AddTransition(Transition.Notfound, FSMStateID.Patrol);

        deathState = new DeathState(this);
        deathState.AddTransition(Transition.Respawn, FSMStateID.Patrol);

        AddFSMState(patrolState);
        AddFSMState(foundState);
        AddFSMState(searchState);
        AddFSMState(deathState);
        
    }

    

    public void StartDeath()
    {
        
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        
        Renderer r = GetComponent<Renderer>();
        r.enabled = false;
        

        
        yield return new WaitForSeconds(1.0f);
        IsDead = true;
        gameObject.SetActive(false);
        


    }

    public void spotted()
    {
        
        if (CurrentState.ID == FSMStateID.Patrol)
        {
            sounds.PlaySpottedSound();
            patrolState.isSpotted();
        }
        else if(CurrentState.ID == FSMStateID.Search)
        {
            sounds.PlaySpottedSound();
            searchState.isSpotted();
        }

        
    }

    public void notSpotted()
    {
        
        if (CurrentState.ID == FSMStateID.Found)
        {
            
            foundState.isOutofsight();
        }
        
    }

    public void Fire(Transform fireloc)
    {
        Vector3 firedir = fireloc.position - gun.position;
        
        firedir.Normalize();
        GameObject bullet = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.Enemy);

        bullet.transform.position = gun.position;
        bullet.transform.rotation = Quaternion.identity;

        EnemyBullet b = bullet.GetComponent<EnemyBullet>();
        b.Fire(firedir);
        bullet.SetActive(true);
        sounds.PlayFireSound();
        rifleburst.Play();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            decrhealth(100);
        }
        
        
        
    }
    

    

    

    public void Reset()
    {
        
        IsDead = false;
        gameObject.transform.position = spawn.position;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        health = 100;
        notSpotted();
        
        if(CurrentStateID == FSMStateID.Search)
        {
            searchState.stopsearch();
        }
        if (CurrentState.ID == FSMStateID.Dead)
        {
            deathState.respawn();
        }
        if (CurrentStateID == FSMStateID.Patrol)
        {

            patrolState.moving = true;
            patrolState.onpath = false;
            patrolState.returning = false;
            patrolState.travelledpoints = 0;
            patrolState.destPos = patrolState.GetClosestWaypoint(gameObject.transform).position;
        }
        
        gameObject.SetActive(true);
        
    }

    public void SmokeCheck(bool inSmoke)
    {
        if (inSmoke)
        {
            vision.SetActive(false);
            visionmesh.SetActive(false);
        }
        else if (!inSmoke)
        {
            vision.SetActive(true);
            visionmesh.SetActive(true); ;    
            notSpotted();
        }
        
    }







}
