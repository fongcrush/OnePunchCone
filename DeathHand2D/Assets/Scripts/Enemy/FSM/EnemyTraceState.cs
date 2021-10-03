using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraceState : EnemyBaseState
{
    float timer = 0f;
    float traceTime = 1f;
    public override void Begin(EnemyController ctrl)
    {
        
    }
    public override void Update(EnemyController ctrl)
    {
        timer += Time.deltaTime;

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
        if(ctrl.CheckTargetInBush() && timer > traceTime && !ctrl.CheckEnemyInBush())
        {
            timer = 0;
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
        ctrl.TraceTarget();
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        ctrl.ChangeState(ctrl.HitState);
    }
    public override void End(EnemyController ctrl)
    {
        
    }
}
