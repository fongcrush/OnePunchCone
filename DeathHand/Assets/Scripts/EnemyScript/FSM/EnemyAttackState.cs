using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void Begin(EnemyController ctrl)
    {
        
    }
    public override void Update(EnemyController ctrl)
    {
        if (ctrl.GetIsActivation())
            return;

        if(!ctrl.IsAlive())
        {
            ctrl.ChangeState(ctrl.DeadState);
            return;
        }
        if(!ctrl.IsAliveTarget())
        {
            ctrl.ChangeState(ctrl.IdleState);
            return;
        }
        if(!ctrl.CheckInAttackRange())
        {
            ctrl.ChangeState(ctrl.TraceState);
            return;
        }

        ctrl.Attack();
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        
    }
    public override void End(EnemyController ctrl)
    {
        
    }
}
