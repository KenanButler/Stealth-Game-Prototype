using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class SearchState : FSMState
{

    SearchProperties properties;
    private GuardController guard;
    bool spotted;
    
    bool looked;
    bool giveup;
    
    Quaternion rightangle;
    Quaternion leftangle;



    public SearchState(GuardController controller, Transform trans, SearchProperties props)
    {
        stateID = FSMStateID.Search;
        this.guard = controller;

        this.properties = props;
        curSpeed = props.speed;
        curRotSpeed = props.rotSpeed;
        spotted = false;

        
        destPos = Vector3.zero;
        looked = false;
        giveup = false;
        
        
        


    }

    public override void Reason(Transform player, Transform npc)
    {
        if (guard.Health <= 0)
        {
            guard.PerformTransition(Transition.NoHealth);
            spotted = false;
            destPos = Vector3.zero;
            looked = false;
            giveup = false;
            return;
        }
        if (spotted)
        {
            guard.PerformTransition(Transition.Spotted);
            spotted = false;
            destPos = Vector3.zero;
            looked = false;
            giveup = false;
            return;
        }
        if (giveup)
        {
            guard.PerformTransition(Transition.Notfound);
            spotted = false;
            destPos = Vector3.zero;
            looked = false;
            giveup = false;
            return;
        }
        
    }

    public override void Act(Transform player, Transform npc)
    {
        
        
        if (destPos == Vector3.zero)
        {
            destPos = player.transform.position;
        }
        if (npc.position != destPos)
        {
            Quaternion desiredrot = Quaternion.LookRotation(destPos - npc.position);
            npc.rotation = Quaternion.RotateTowards(npc.rotation, desiredrot, curRotSpeed * Time.deltaTime);

            npc.position = Vector3.MoveTowards(npc.position, destPos, curSpeed * Time.deltaTime);

            rightangle = Quaternion.Euler(npc.rotation.x, npc.rotation.y + 90, npc.rotation.z);
            leftangle = Quaternion.Euler(npc.rotation.x, npc.rotation.y - 90, npc.rotation.z);
        }
        

        else  if (npc.position == destPos)
        {
            if (npc.rotation != rightangle && looked == false)
            {
                npc.rotation = Quaternion.RotateTowards(npc.rotation, rightangle, curRotSpeed * Time.deltaTime);
            }
            else
            {
                looked = true;
                npc.rotation = Quaternion.RotateTowards(npc.rotation, leftangle, curRotSpeed * Time.deltaTime);
                if (npc.rotation == leftangle)
                {
                    giveup = true;
                }
            }
            
        }
        


    }

    public void isSpotted()
    {
        
        spotted = true;
    }
    public void stopsearch()
    {
        giveup = true;
    }
    
}
