using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayerController : MonoBehaviour
{
    private TankBody body;
    [SerializeField]
    private Camera cam;
    public bool inMenu = false;

    private void Start()
    {
        body = GetComponentInChildren<TankBody>();
    }
    private void Update()
    {
        if (body.animator.GetBool("Died") == false && inMenu == false){
            //body.animator.SetBool("Reload", false);
            HandleMovement();
            Updateaim();
            body.animator.ResetTrigger("Fire");

            if (Input.GetButtonDown("Fire1"))
            {
                body.Fire(TankBody.BulletTypes.Default);
            }
            if (Input.GetButtonDown("Fire2"))
            {
                body.Fire(TankBody.BulletTypes.Expanding);
            }
            
            
        }
        
        

    }

    private void HandleMovement()
    {

        if ((Input.GetAxis("Horizontal") >=0.1 || Input.GetAxis("Horizontal") <= -0.1) || Input.GetAxis("Vertical") >= 0.01 || Input.GetAxis("Vertical") <= -0.1)
        {
            body.animator.SetBool("Moving", true);
        }
        else
        {
            body.animator.SetBool("Moving", false);
        }
        

        float xdirect = Input.GetAxis("Horizontal");
        float ydirect = Input.GetAxis("Vertical");

        body.verticaldirect = ydirect;

        body.horizontaldirect = xdirect;

        
        
        
        
        
    }

    [SerializeField] private bool debug = true;

    private void Updateaim()
    {
        
        //create a ray from main camera into the world via the mouse position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //define a plane in the world using the tanks turret position
        Plane plane = new Plane(body.Turret.up, body.Turret.position);

        float distancetoplane;

        //we ask the plane if the ray we created when cast, hit it and the
        //distance it traveled to do so
        if (plane.Raycast(ray, out distancetoplane))
        {
            Vector3 mouseworldpos = ray.GetPoint(distancetoplane);

            Vector3 aimdirect = mouseworldpos - body.Turret.position;

            aimdirect.y = 0f;

            body.UpdateAimDir(aimdirect);

            if (debug)
            {
                Debug.DrawLine(body.transform.position, mouseworldpos, Color.green);
                Debug.DrawLine(ray.origin, mouseworldpos, Color.magenta);

                Debug.DrawRay(mouseworldpos, Vector3.left, Color.cyan);
                Debug.DrawRay(mouseworldpos, Vector3.back, Color.cyan);
            }
        }
    }

    
}
