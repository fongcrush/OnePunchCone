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
    private Player player;

    private Enemy enemy;
    private EnemyBaseState currentState;
    private EnemyBaseState prevState;

    public GameObject enemyAttackCollider;
    public GameObject enemyWarningBox;
    public GameObject arrow;

    private GameObject arrowObject;
    private Rigidbody arrowRigid;
    private Rigidbody enemyRigid;

    private MeshRenderer enemyWarningBoxMesh;

    public int attTypeValue;
    private AttackType attType;

    private PlayerDirectionX playerDirectionX;

    public float traceRange = 5.0f;
    public float attackRange = 2.0f;
    public float speed = 0.75f;
    public float attackDelay = 1f;
    public float attackSpeed = 1f;

    private float range = 0.5f;

    public Transform targetTransform;

    private bool isAttackColliderActivation = false;
    private bool isOnDamage = false;

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
        enemyRigid = GetComponent<Rigidbody>();
        playerDirectionX = PlayerDirectionX.LEFT;

        enemyWarningBoxMesh = enemyWarningBox.GetComponent<MeshRenderer>();
        attType = (AttackType)attTypeValue;
        player = GameObject.Find("Player").GetComponent<Player>();
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        if (attType == AttackType.RANGED)
        {
            enemyWarningBox.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z);
            enemyWarningBox.transform.localScale = new Vector3(4f, 1f, 1f);
            range = 1.99f;
        }

        ChangeState(IdleState);
    }
    private void Update()
    {
        currentState.Update(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerAttackCollider")
        {
            // 데미지 조정 필요
            //gameObject.GetComponent<Enemy>().OnDamage(50.0f);

            enemy.stat.ChangeHP(-player.stat.Power);
            Debug.Log("Hit!");
        }
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
        return (CalcTargetDistance() < traceRange) ? true : false;
    }
    public bool CheckInAttackRange()
    {
        return (CalcTargetDistance() < attackRange && Mathf.Abs(transform.position.z - targetTransform.position.z) < 0.3f) ? true : false;
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
            Debug.Log(1);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (playerDirectionX == PlayerDirectionX.RIGHT)
        {
            Debug.Log(2);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
    public void CheckDistanceX()
    {
        // 플레이어와 적의 거리가 Range 값보다 작을 경우
        if (Mathf.Abs(transform.position.x - targetTransform.position.x) <= range)
        {
            // X축 거리는 유지하고 Z축만 이동
            if (playerDirectionX == PlayerDirectionX.LEFT)
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position + new Vector3(+(transform.position.x - targetTransform.position.x), 0f, 0f), speed * Time.deltaTime);
            else
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position + new Vector3(-(transform.position.x - targetTransform.position.x), 0f, 0f), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
        }
    }
    public void TraceTarget()
    {
        CheckPlayerDirectionX();
        ChangeRotation();

        CheckDistanceX();

        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }
    public void Attack()
    {
        CheckPlayerDirectionX();
        ChangeRotation();

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
        c.a = 0.2f;

        yield return new WaitForSeconds(attackDelay);

        // 적 공격 범위 박스 비활성화, 적 공격 히트 박스 활성화 및 Damage
        enemyWarningBox.SetActive(false);

        if (attType == AttackType.MELEE)
            enemyAttackCollider.SetActive(true);
        else if (attType == AttackType.RANGED)
        {
            if(playerDirectionX == PlayerDirectionX.LEFT)
                arrowObject = Instantiate(arrow, new Vector3(transform.position.x - 0.5f, 1.5f, transform.position.z), Quaternion.identity);
            else
                arrowObject = Instantiate(arrow, new Vector3(transform.position.x + 0.5f, 1.5f, transform.position.z), Quaternion.identity);
            
            arrowRigid = arrowObject.GetComponent<Rigidbody>();

            if (playerDirectionX == PlayerDirectionX.LEFT)
                arrowRigid.AddForce(Vector3.left * 20f, ForceMode.Impulse);
            else
                arrowRigid.AddForce(Vector3.right * 20f, ForceMode.Impulse);
        }

        // n초 후 공격 종료
        // yield return new WaitForSeconds();

        // 적 공격 박스 비활성화
        yield return new WaitForSeconds(0.01f);

        enemyAttackCollider.SetActive(false);

        yield return new WaitForSeconds(attackSpeed);

        isAttackColliderActivation = false;
    }
}