using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void Begin(EnemyController ctrl)
    {
        Debug.Log("EnemyDeadState Begin");
    }
    public override void Update(EnemyController ctrl)
    {
        Debug.Log("EnemyDeadState Update");
    }
    public override void OnCollisionEnter(EnemyController ctrl)
    {
        Debug.Log("EnemyDeadState OnCollisionEnter");
    }
    public override void End(EnemyController ctrl)
    {
        Debug.Log("EnemyDeadState End");
    }
}
