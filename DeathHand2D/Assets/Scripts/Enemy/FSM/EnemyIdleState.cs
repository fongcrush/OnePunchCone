public class EnemyIdleState : EnemyBaseState
{
	public override void Begin(EnemyController ctrl)
	{

	}
	public override void Update(EnemyController ctrl)
	{
		if(!ctrl.IsAlive())
		{
			ctrl.ChangeState(ctrl.DeadState);
			return;
		}
		if(ctrl.CheckTargetInBush())
        {
			return;
        }
		if(ctrl.CheckInTraceRange())
		{
			ctrl.ChangeState(ctrl.TraceState);
			return;
		}
	}
	public override void OnCollisionEnter(EnemyController ctrl)
	{
		ctrl.ChangeState(ctrl.HitState);
	}
	public override void End(EnemyController ctrl)
	{

	}
}
