using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    float timer;
    float randomValue;
    public override void Begin(EnemyController ctrl)
    {
        randomValue = Random.Range(1f, 100f);
    }
    public override void Update(EnemyController ctrl)
    {
        if(!ctrl.IsAlive())
        {
            ctrl.ChangeState(ctrl.DeadState);
            return;
        }
        if (ctrl.GetIsAttackActivation())
            return;
        if(ctrl.GetIsChangeState())
        {
            return;
        }    
        if (!ctrl.IsAliveTarget())
        {
            ctrl.ChangeState(ctrl.IdleState);
            return;
        }
        if(ctrl.CheckTargetInBush() && !ctrl.CheckEnemyInBush())
        {
            ctrl.ChangeState(ctrl.TraceState);
            return;
        }
        if(!ctrl.CheckInAttackRange())
        {
            ctrl.ChangeState(ctrl.TraceState);
            return;
        }
        if (randomValue <= ctrl.GetEnemyStateProbability().attackToEscapeProbability)
        {
            ctrl.ChangeState(ctrl.EscapeState);
        }
        ctrl.Attack();
        randomValue = Random.Range(1f, 100f);
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        ctrl.ChangeState(ctrl.HitState);
    }
    public override void End(EnemyController ctrl)
    {
        
    }
}
