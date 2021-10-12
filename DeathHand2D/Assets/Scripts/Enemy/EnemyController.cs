using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static GameMgr;

enum AttackType
{
    MELEE,
    RANGED,
    MELEE_ELITE,
    RANGED_ELITE
}
enum PlayerDirectionX
{
    LEFT,
    RIGHT
}
public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rigid;

    private Enemy enemy;
    private EnemyBaseState currentState;
    private EnemyBaseState prevState;

    private SkeletonAnimation skeletonAnime;

    public int attTypeValue;
    private AttackType attType;

    private PlayerDirectionX playerDirectionX;
    private int playerDirectionXValue;

    private Transform targetTransform;

    public bool isInBush = false;
    private bool isHitCheck = false;
    //public bool skill_02_Check = false;

    public EnemyBaseState CurrentState
    {
        get { return currentState; }
    }

    public static int enemyCount = 0;

    public readonly EnemyIdleState IdleState = new EnemyIdleState();
    public readonly EnemyTraceState TraceState = new EnemyTraceState();
    public readonly EnemyAttackState AttackState = new EnemyAttackState();
    public readonly EnemyDeadState DeadState = new EnemyDeadState();
    public readonly EnemyHitState HitState = new EnemyHitState();


    private void Awake()
    {
        currentState = IdleState;
        playerDirectionX = PlayerDirectionX.LEFT;

        skeletonAnime = GetComponent<SkeletonAnimation>();

        attType = (AttackType)attTypeValue;

        player = GameObject.Find("Player");
        targetTransform = player.transform;

        ++enemyCount;
    }
    private void Start()
    {
        if (attType == AttackType.MELEE)
            enemy = GetComponent<MeleeEnemy>();
        else if (attType == AttackType.RANGED)
            enemy = GetComponent<RangedEnemy>();
        else if (attType == AttackType.MELEE_ELITE)
            enemy = GetComponent<EliteMeleeEnemy>();
        else if (attType == AttackType.RANGED_ELITE)
            enemy = GetComponent<EliteRangedEnemy>();

        ChangeState(IdleState);
    }
    private void Update()
    {
        currentState.Update(this);
        if (attType == AttackType.RANGED || attType == AttackType.RANGED_ELITE)
        {
            if (!isHitCheck && enemy.GetChaseRange() < 10)
                CheckEnemysHit();
        }
    }
    public void Hit(int damage)
    {
        enemy.stat.curHP -= damage;
        Debug.Log("Hit! Enemy Hp : " + enemy.stat.curHP);
        currentState.OnCollisionEnter(this);
        GM.SetEnemyHit(true);
    }

    private void CheckEnemysHit()
    {
        if (GM.GetEnemyHit() == true)
        {
            enemy.SetChaseRange(enemy.GetChaseRange() * 3f);
        }
        isHitCheck = true;
        Invoke("HitCheckReset", 0.5f);
    }
    private void HitCheckReset()
    {
        isHitCheck = false;
    }
    public void ChangeState(EnemyBaseState state)
    {
        currentState.End(this);
        prevState = currentState;
        currentState = state;
        currentState.Begin(this);

        // 나중에 변경

        if (prevState == AttackState)
        {
            StopCoroutine(enemy.Attack());
            enemy.AttCollsSetActiveFalse();
        }

        if (currentState == IdleState)
        {
            if (attType != AttackType.RANGED_ELITE)
                skeletonAnime.AnimationName = "idle";
        }
        else if (currentState == TraceState)
        {
            if (attType != AttackType.RANGED_ELITE)
                skeletonAnime.AnimationName = "walking";
        }
        else if (currentState == AttackState)
        {
            if (attType == AttackType.MELEE || attType == AttackType.RANGED)
                skeletonAnime.AnimationName = "attack";
        }
        else if (currentState == HitState)
        {
            if (attType != AttackType.RANGED_ELITE)
                skeletonAnime.AnimationName = "shot";
        }
        else if (currentState == DeadState)
        {
            if (attType != AttackType.RANGED_ELITE)
                skeletonAnime.AnimationName = "dead";
        }
    }
    public float CalcTargetDistance()
    {
        return (player.transform.position - transform.position).magnitude;
    }
    public bool CheckInTraceRange()
    {
        return (CalcTargetDistance() < enemy.GetChaseRange()) ? true : false;
    }
    public bool CheckInAttackRange()
    {
        return (CalcTargetDistance() < enemy.GetAttackRange() && Mathf.Abs(transform.position.y - player.transform.position.y) < 0.3f) ? true : false;
    }
    public bool CheckTargetInBush()
    {
        if (player.GetComponent<PlayerController>().InBush)
        {
            if (enemy.GetChaseRange() > 10)
                enemy.SetChaseRange(enemy.GetChaseRange() / 3);
            GM.SetEnemyHit(false);
            return true;
        }
        return false;
    }
    public bool CheckEnemyInBush()
    {
        if (CheckInTraceRange())
            return isInBush;
        else
            return false;
    }
    public bool IsAlive()
    {
        return (enemy.stat.curHP > 0) ? true : false;
    }
    public bool IsAliveTarget()
    {
        if (player.transform == null) return false;

        return true;
    }
    public void EnemyDead()
    {
        --enemyCount;
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public int GetPlayerDirectionX()
    {
        playerDirectionXValue = (int)playerDirectionX;
        return playerDirectionXValue;
    }
    public Vector3 GetTargetTransformPosition()
    {
        return targetTransform.position;
    }
    public void CheckPlayerDirectionX()
    {
        if (transform.position.x - targetTransform.position.x > 0)
            playerDirectionX = PlayerDirectionX.LEFT;
        else if (transform.position.x - targetTransform.position.x < 0)
            playerDirectionX = PlayerDirectionX.RIGHT;
    }
    public void ChangeRotation()
    {
        if (playerDirectionX == PlayerDirectionX.LEFT)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            enemy.ChangeEnemyWarningAreaZRotation(Quaternion.Euler(0f, 0f, 0f));
        }
        else if (playerDirectionX == PlayerDirectionX.RIGHT)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            enemy.ChangeEnemyWarningAreaZRotation(Quaternion.Euler(0f, 0f, 180f));
        }
    }
    public void CheckDistanceX()
    {
        if (Mathf.Abs(transform.position.x - targetTransform.position.x) <= enemy.GetMinDistance())
        {
            // X축 거리는 유지하고 Z축만 이동
            if (playerDirectionX == PlayerDirectionX.LEFT)
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(+(transform.position.x - targetTransform.position.x), 0f), enemy.GetEnemySpeed() * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(-(transform.position.x - targetTransform.position.x), 0f), enemy.GetEnemySpeed() * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, enemy.GetEnemySpeed() * Time.deltaTime);
        }
    }
    public void TraceTarget()
    {
        if (enemy.GetChaseRange() < 10)
            enemy.SetChaseRange(enemy.GetChaseRange() * 3);

        CheckPlayerDirectionX();
        ChangeRotation();

        CheckDistanceX();
    }
    public void Attack()
    {
        CheckPlayerDirectionX();
        ChangeRotation();

        StartCoroutine(enemy.Attack());
    }
    public bool GetIsAttackActivation()
    {
        return enemy.GetIsAttackActivation();
    }
}