using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AttackType
{
    MELEE,
    RANGED
}
enum PlayerDirectionX
{
    LEFT,
    RIGHT
}

public class EnemyController : MonoBehaviour
{
    private PlayerController player;

    private Enemy enemy;
    private EnemyBaseState currentState;
    private EnemyBaseState prevState;

    public GameObject enemyAttackCollider;
    public GameObject enemyWarningBox;
    public GameObject enemyTimingBox;
    public GameObject arrow;

    private GameObject arrowObject;
    private Rigidbody2D arrowRigid;
    private Rigidbody2D enemyRigid;

    private SpriteRenderer enemyWarningBoxMesh;

    public int attTypeValue;
    private AttackType attType;

    private PlayerDirectionX playerDirectionX;

    public string monsterName;
    public int monsterRank;
    public int attDamage;
    public float traceRange = 5.0f;
    public float attackRange = 2.0f;
    public float speed = 0.75f;
    public float attackDelay = 1f; // 공격 후 다음 공격까지 걸리는 시간
    public float attackSpeed = 1f; // 공격에 걸리는 시간

    private float range = 0.5f;

    public Transform targetTransform;

    private bool isAttackColliderActivation = false;

    public EnemyBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly EnemyIdleState IdleState = new EnemyIdleState();
    public readonly EnemyTraceState TraceState = new EnemyTraceState();
    public readonly EnemyAttackState AttackState = new EnemyAttackState();
    public readonly EnemyDeadState DeadState = new EnemyDeadState();
    public readonly EnemyHitState HitState = new EnemyHitState();

    private void Awake()
    {
        currentState = IdleState;
        enemyRigid = GetComponent<Rigidbody2D>();
        playerDirectionX = PlayerDirectionX.LEFT;

        enemyWarningBoxMesh = enemyWarningBox.GetComponent<SpriteRenderer>();
        attType = (AttackType)attTypeValue;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        if (attType == AttackType.RANGED)
        {
            enemyAttackCollider.transform.localScale = new Vector3(1f, 1f, 1f);
            enemyWarningBox.transform.localScale = new Vector3(1f, 1f, 1f);
            enemyTimingBox.transform.localScale = new Vector3(1f, 1f, 1f);
            range = 2.99f;

            attackSpeed = 0.2f;
            attackDelay = 0f;
            speed = 1.5f;
            attackRange = 3f;
            traceRange = 8f;
            enemy.stat.MaxHP = 500;
            enemy.stat.Power = 30;
        }

        ChangeState(IdleState);
    }
    private void Update()
    {
        currentState.Update(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttackCollider")
        {
            // 데미지 조정 필요
            //gameObject.GetComponent<Enemy>().OnDamage(50.0f);

            enemy.stat.ChangeHP(-player.stat.Power);
            Debug.Log("Hit!");
            Debug.Log(gameObject.name + GetComponent<Enemy>().GetEnemyHp());
            currentState.OnCollisionEnter(this);
        }
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
        return (CalcTargetDistance() < traceRange) ? true : false;
    }
    public bool CheckInAttackRange()
    {
        return (CalcTargetDistance() < attackRange && Mathf.Abs(transform.position.y - targetTransform.position.y) < 0.3f) ? true : false;
    }
    public bool IsAlive()
    {
        return (enemy.stat.GetHP() > 0) ? true : false;
    }
    public bool IsAliveTarget()
    {
        if (targetTransform == null) return false;

        return true;
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public void CheckPlayerDirectionX()
    {
        // 타겟이 왼쪽
        if (transform.position.x - targetTransform.position.x > 0)
            playerDirectionX = PlayerDirectionX.LEFT;
        // 타겟이 오른쪽
        else if (transform.position.x - targetTransform.position.x < 0)
            playerDirectionX = PlayerDirectionX.RIGHT;
    }
    public void ChangeRotation()
    {
        if (playerDirectionX == PlayerDirectionX.LEFT)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            enemyWarningBox.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (playerDirectionX == PlayerDirectionX.RIGHT)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            enemyWarningBox.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }
    public void CheckDistanceX()
    {
        // 플레이어와 적의 거리가 Range 값보다 작을 경우
        if (Mathf.Abs(transform.position.x - targetTransform.position.x) <= range)
        {
            // X축 거리는 유지하고 Z축만 이동
            if (playerDirectionX == PlayerDirectionX.LEFT)
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(+(transform.position.x - targetTransform.position.x), 0f), speed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(-(transform.position.x - targetTransform.position.x), 0f), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
        }
    }
    public void TraceTarget()
    {
        CheckPlayerDirectionX();
        ChangeRotation();

        CheckDistanceX();
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    public void Attack()
    {
        CheckPlayerDirectionX();
        ChangeRotation();

        if(attType == AttackType.RANGED)
        {
            enemyAttackCollider.transform.position = targetTransform.position;
            enemyWarningBox.transform.position = targetTransform.position;
            enemyTimingBox.transform.position = targetTransform.position;
        }

        StartCoroutine(AttackActivation());
    }
    public bool GetIsAttackColliderActivation()
    {
        return isAttackColliderActivation;
    }
    IEnumerator AttackActivation()
    {

        // 적 공격 범위 박스 활성화
        enemyWarningBox.SetActive(true);
        isAttackColliderActivation = true;

        // 적 공격 박스의 투명도 설정
        var c = enemyWarningBoxMesh.material.color;
        c.a = 0.6f;
        enemyWarningBoxMesh.material.color = c;
        yield return new WaitForSeconds(attackSpeed - 0.1f);

        // 완벽한 회피 타이밍 활성화
        enemyTimingBox.SetActive(true);
        c.a = 0.8f;
        enemyWarningBoxMesh.material.color = c;

        yield return new WaitForSeconds(attackSpeed);


        // 완벽한 회피 타이밍 비활성화, 적 공격 범위 박스 비활성화, 적 공격 히트 박스 활성화 및 Damage
        enemyWarningBox.SetActive(false);
        enemyTimingBox.SetActive(false);

        if (attType == AttackType.MELEE)
            enemyAttackCollider.SetActive(true);
        else if (attType == AttackType.RANGED)
        {
            //if (playerDirectionX == PlayerDirectionX.LEFT)
            //    arrowObject = Instantiate(arrow, new Vector2(transform.position.x - 0.5f, transform.position.y + 2.2f), Quaternion.identity);
            //else
            //    arrowObject = Instantiate(arrow, new Vector2(transform.position.x + 0.5f, transform.position.y + 2.2f), Quaternion.identity);

            //arrowRigid = arrowObject.GetComponent<Rigidbody2D>();

            //if (playerDirectionX == PlayerDirectionX.LEFT)
            //    arrowRigid.AddForce(Vector2.left * 20f, ForceMode2D.Impulse);
            //else
            //    arrowRigid.AddForce(Vector2.right * 20f, ForceMode2D.Impulse);
        }

        // n초 후 공격 종료
        // yield return new WaitForSeconds();

        // 적 공격 박스 비활성화
        yield return new WaitForSeconds(0.01f);

        enemyAttackCollider.SetActive(false);

        yield return new WaitForSeconds(attackDelay);

        isAttackColliderActivation = false;
    }
}