using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
	float timer = 0f;
	float delayTime = 0.3f;
	float randomValue;
    public override void Begin(EnemyController ctrl)
	{
		timer = 0;
		randomValue = Random.Range(1f, 100f);
	}
	public override void Update(EnemyController ctrl)
	{
		timer += Time.deltaTime;

		if (!ctrl.IsAlive())
		{
			ctrl.ChangeState(ctrl.DeadState);
			return;
		}
		if (timer > delayTime)
        {
			if(randomValue <= ctrl.GetEnemyStateProbability().hitToEscapeProbability)
            {
				ctrl.ChangeState(ctrl.EscapeState);
				return;
            }
			else
            {
				ctrl.ChangeState(ctrl.IdleState);
				return;
            }
		}
	}
    public override void OnCollisionEnter(EnemyController ctrl)
	{

	}
    public override void End(EnemyController ctrl)
	{

	}
}
