using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraceState : EnemyBaseState
{
    public override void Begin(EnemyController ctrl)
    {
        Debug.Log("EnemyTraceState Begin");
    }
    public override void Update(EnemyController ctrl)
    {
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
        if(!ctrl.CheckInTraceRange())
        {
            ctrl.ChangeState(ctrl.IdleState);
            return;
        }
        if(ctrl.CheckInAttackRange())
        {
            ctrl.ChangeState(ctrl.AttackState);
            return;
        }

        ctrl.moveTarget();
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        Debug.Log("EnemyTraceState OnCollisionEnter");
    }
    public override void End(EnemyController ctrl)
    {
        Debug.Log("EnemyTraceState End");
    }
}
