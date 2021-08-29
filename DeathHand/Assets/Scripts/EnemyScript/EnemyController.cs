using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyBaseState currentState;
    private EnemyBaseState prevState;

    public float TraceRange = 5.0f;
    public float AttackRange = 2.0f;
    public float Speed = 0.75f;
    public Transform targetTransform;

    public EnemyBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly EnemyIdleState IdleState = new EnemyIdleState();
    public readonly EnemyTraceState TraceState = new EnemyTraceState();
    public readonly EnemyAttackState AttackState = new EnemyAttackState();
    public readonly EnemyDeadState DeadState = new EnemyDeadState();

    private void Awake()
    {
        currentState = IdleState;
    }
    private void Start()
    {
        ChangeState(IdleState);
    }
    private void Update()
    {
        currentState.Update(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this);
    }
    public void ChangeState(EnemyBaseState state)
    {
        currentState.End(this);
        prevState = currentState;
        currentState = state;
        currentState.Begin(this);
    }
    public float CalcTargetDistance()
    {
        return (targetTransform.position - transform.position).magnitude;
    }
    public bool CheckInTraceRange()
    {
        return ((CalcTargetDistance() < TraceRange) ? true : false);
    }
    public bool CheckInAttackRange()
    {
        return ((CalcTargetDistance() < AttackRange) ? true : false);
    }
    public bool IsAlive()
    {
        return false;
    }
    public bool IsAliveTarget()
    {
        if (targetTransform == null) return false;

        return true;
    }
    public void moveTarget()
    {
        Vector3 velo = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position, ref velo, Speed);
    }
}
