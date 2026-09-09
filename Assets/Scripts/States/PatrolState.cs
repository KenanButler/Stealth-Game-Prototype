using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;

public class PatrolState : FSMState
{
    PatrolProperties properties;
    private GuardController guard;
    public bool moving;
    public bool onpath;
    bool isloop;
    public bool returning;
    public int travelledpoints;
    bool spotted;
    bool singlepoint;
    Quaternion ogrotation;
    

    public  PatrolState(GuardController controller, Transform trans, PatrolProperties props)
    {
        ogrotation = trans.localRotation;
        stateID = FSMStateID.Patrol;
        this.properties = props;
        guard = controller;
        waypoints = props.Path;
        curSpeed = props.speed;
        curRotSpeed = props.rotSpeed;
        destPos = GetClosestWaypoint(trans).position;
        
        moving = true;
        onpath = false;
        isloop = props.isloop;
        returning = false;
        travelledpoints = 0;
        spotted = false;
        singlepoint = false;
        if (props.Path.Length == 1)
        {
            singlepoint = true;
        }
        
        


    }

    public override void Act(Transform player, Transform npc)
    {
        
        if (singlepoint)
        {
            if (npc.position == destPos)
            {
                npc.rotation = Quaternion.RotateTowards(npc.rotation, ogrotation, curRotSpeed * Time.deltaTime);
            }
            else if (npc.position != destPos)
            {
                Quaternion desiredrot = Quaternion.LookRotation(destPos - npc.position);
                npc.rotation = Quaternion.RotateTowards(npc.rotation, desiredrot, curRotSpeed * Time.deltaTime);
                npc.position = Vector3.MoveTowards(npc.position, destPos, curSpeed * Time.deltaTime);
            }
        }

        if (!singlepoint)
        {
            if (!onpath && moving)
            {

                Quaternion desiredrot = Quaternion.LookRotation(destPos - npc.position);
                npc.rotation = Quaternion.RotateTowards(npc.rotation, desiredrot, curRotSpeed * Time.deltaTime);




                npc.position = Vector3.MoveTowards(npc.position, destPos, curSpeed * Time.deltaTime);
                if (npc.position == destPos)
                {
                    onpath = true;
                    for (int x = 0; x < waypoints.Length; x++)
                    {
                        if (waypoints[x].position == npc.position)
                        {
                            travelledpoints = x;
                            moving = false;

                            destPos = Vector3.zero;
                            break;
                        }
                    }

                }


            }


            if (onpath && moving == false)
            {

                if (destPos != Vector3.zero)
                {
                    Quaternion desiredrot = Quaternion.LookRotation(destPos - npc.position);
                    npc.rotation = Quaternion.RotateTowards(npc.rotation, desiredrot, curRotSpeed * Time.deltaTime);
                    if (npc.rotation == desiredrot)
                    {

                        moving = true;
                    }



                }
                else if (destPos == Vector3.zero)
                {
                    if (travelledpoints == waypoints.Length - 1)
                    {
                        if (isloop)
                        {

                            travelledpoints = 0;
                        }
                        if (isloop != true)
                        {
                            returning = true;

                            travelledpoints--;

                        }

                    }
                    else if (returning && travelledpoints == 0)
                    {
                        returning = false;

                        travelledpoints++;
                    }
                    else
                    {

                        if (returning)
                        {
                            travelledpoints--;
                        }
                        if (!returning)
                        {
                            travelledpoints++;
                        }
                    }

                    destPos = waypoints[travelledpoints].position;
                }




            }

            else if (onpath && moving)
            {


                npc.position = Vector3.MoveTowards(npc.position, destPos, curSpeed * Time.deltaTime);
                if (npc.position == destPos)
                {
                    moving = false;
                    destPos = Vector3.zero;
                }

            }
        }

        

    }

    public override void Reason(Transform player, Transform npc)
    {

         if (guard.Health <= 0)
        {
            guard.PerformTransition(Transition.NoHealth);
            spotted = false;
            moving = false;
            return;
        }

         if (spotted)
        {
            guard.PerformTransition(Transition.Spotted);
            spotted = false;
            moving = false;
            
            
            return;
        }

         
    }

    public void isSpotted()
    {
        
        spotted = true;
    }

    public void reset()
    {
        travelledpoints = 0;
    }

    
}
