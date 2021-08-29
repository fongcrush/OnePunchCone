using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void Begin(EnemyController ctrl)
    {
        Debug.Log("EnemyAttackState Begin");
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
        if(!ctrl.CheckInAttackRange())
        {
            ctrl.ChangeState(ctrl.TraceState);
            return;
        }
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        Debug.Log("EnemyAttackState OnCollisionEnter");
    }
    public override void End(EnemyController ctrl)
    {
        Debug.Log("EnemyAttackState End");
    }
}
