using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void Begin(EnemyController ctrl)
    {
        Debug.Log("Enemy Dead");
    }
    public override void Update(EnemyController ctrl)
    {
        ctrl.DestroyEnemy();
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        
    }
    public override void End(EnemyController ctrl)
    {
        
    }
}
