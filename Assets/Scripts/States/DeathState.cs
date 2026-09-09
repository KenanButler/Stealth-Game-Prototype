using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : FSMState
{
    private bool isdead = true;
    private GuardController guard;

   public DeathState(GuardController controller)
    {
        stateID = FSMStateID.Dead;
        guard = controller;
        curSpeed = 0;
        curRotSpeed = 0;
    }

    public override void Act(Transform player, Transform npc)
    {
        
        if (isdead)
        {
            guard.StartDeath();
        }
    }

    public override void Reason(Transform player, Transform npc)
    {
        if(guard.Health != 0)
        {
            guard.PerformTransition(Transition.Respawn);

            return;
        }
    }

    public void respawn()
    {
        guard.PerformTransition(Transition.Respawn);
    }
}
