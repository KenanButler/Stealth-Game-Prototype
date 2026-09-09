//using System.Collections;
//using System.Collections.Generic;
//using System.IO.Pipes;
//using UnityEditorInternal;
using UnityEngine;
//using static TankBody;

public class FoundState : FSMState
{

    FoundProperties properties;
    private GuardController guard;
    bool spotted;
    
    float interval;
    float time;
    
    

    public FoundState(GuardController controller, Transform trans, FoundProperties props)
    {
        stateID = FSMStateID.Found;
        this.guard = controller;
        this.properties = props;
        curSpeed = props.speed;
        curRotSpeed = props.rotSpeed;
        spotted = true;

        
        
        interval = props.fireinterval;
        
    }

    public override void Reason(Transform player, Transform npc)
    {
        if (guard.Health <= 0)
        {
            guard.PerformTransition(Transition.NoHealth);
            spotted |= false;
            return;
        }

        
        if (!spotted)
        {
            
            guard.PerformTransition(Transition.Lostsight);
            spotted = true;
            return;
        }
        

    }

    public override void Act(Transform player, Transform npc)
    {
        time += Time.deltaTime;
        
        destPos = player.transform.position;
        float dist = Vector3.Distance(destPos, npc.position);
        

        Quaternion desiredrot = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.RotateTowards(npc.rotation, desiredrot, curRotSpeed * Time.deltaTime);
        


        if (dist > 4)
        {
            npc.position = Vector3.MoveTowards(npc.position, destPos, curSpeed * Time.deltaTime);
        }
        if (dist < 6)
        {
            if(time >= interval)
            {
                guard.Fire(player.transform);
                time = 0;
            }
            
        }
        

        
    }

    public void isOutofsight()
    {
        spotted = false;
    }
    

    
}
