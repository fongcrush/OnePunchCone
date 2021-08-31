using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
	bool isHit = false;
    public override void Begin(EnemyController ctrl)
	{
		Debug.Log("Enemy is hit");
		
		isHit = true;
	}
	public override void Update(EnemyController ctrl)
	{

	}
    public override void OnCollisionEnter(EnemyController ctrl)
	{
		
	}
    public override void End(EnemyController ctrl)
	{
		isHit = false;
	}
}
