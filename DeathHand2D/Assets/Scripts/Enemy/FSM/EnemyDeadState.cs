using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    float timer = 0f;
    float delayTime = 3.0f;
    public override void Begin(EnemyController ctrl)
    {
        Debug.Log("Enemy Dead");
        ctrl.EnemyDead();
    }
    public override void Update(EnemyController ctrl)
    {
        timer += Time.deltaTime;


        if (timer > delayTime)
        {
            ctrl.DestroyEnemy();
        }
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        
    }
    public override void End(EnemyController ctrl)
    {
        
    }
}
