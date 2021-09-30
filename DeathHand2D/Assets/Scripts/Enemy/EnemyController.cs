using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private Enemy enemy;
    private EnemyBaseState currentState;
    private EnemyBaseState prevState;

    public GameObject enemyAttackCollider;
    public GameObject enemyWarningBox;
    public GameObject enemyTimingBox;
    public GameObject summonCreature;

    private SpriteRenderer enemyWarningBoxMesh;
    private SpriteRenderer enemySpriteRenderer;
    private SpriteRenderer playerSpriteRenderer;

    public int attTypeValue;
    private AttackType attType;

    private PlayerDirectionX playerDirectionX;

    private float traceRange;
    private float attackRange;
    private float speed;
    private float attackDelay; // 공격 자세부터 실제 공격까지 걸리는 시간
    private float attackSpeed; // 공격에 걸리는 시간

    private float range;

    public Transform targetTransform;

    private bool isAttackColliderActivation = false;

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

        enemyWarningBoxMesh = enemyWarningBox.GetComponent<SpriteRenderer>();
        enemySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        attType = (AttackType)attTypeValue;
        player = GameObject.Find("Player");
        enemy = GetComponent<Enemy>();

        ++enemyCount;
    }
    private void Start()
    {
        if(attType == AttackType.MELEE)
        {
            range = 0.5f;

            attackSpeed = 1f;
            attackDelay = 0.8f;
            traceRange = 5f;
            attackRange = 1f;
            speed = 1f;
            enemy.stat.MaxHP = 300;
            enemy.stat.Power = 100;
        }
        else if (attType == AttackType.RANGED)
        {
            enemyAttackCollider.transform.localScale = new Vector3(1f, 1f, 1f);
            enemyWarningBox.transform.localScale = new Vector3(1f, 1f, 1f);
            enemyTimingBox.transform.localScale = new Vector3(1f, 1f, 1f);
            range = 2.99f;

            attackSpeed = 0.4f;
            attackDelay = 0.7f;
            speed = 1.2f;
            attackRange = 3f;
            traceRange = 8f;
            enemy.stat.MaxHP = 500;
            enemy.stat.Power = 150;
        }
        else if (attType == AttackType.MELEE_ELITE)
        {
            enemyAttackCollider.transform.localScale = new Vector3(10f, 1f, 1f);
            enemyWarningBox.transform.localScale = new Vector3(10f, 1f, 1f);
            enemyTimingBox.transform.localScale = new Vector3(10f, 1f, 1f);
            enemyAttackCollider.transform.position = transform.position + new Vector3(-5f, 0f, 0f);
            enemyWarningBox.transform.position = transform.position + new Vector3(-5f, 0f, 0f);
            enemyTimingBox.transform.position = transform.position + new Vector3(-5f, 0f, 0f);

            range = 2.99f;

            attackDelay = 2f;
            attackSpeed = 1f;
            speed = 1.5f;
            attackRange = 3f;
            traceRange = 8f;
            enemy.stat.MaxHP = 700;
            enemy.stat.Power = 200;
        }
        else if (attType == AttackType.RANGED_ELITE)
        {
            enemyAttackCollider.transform.localScale = new Vector3(3f, 2f, 1f);
            enemyWarningBox.transform.localScale = new Vector3(3f, 2f, 1f);
            enemyTimingBox.transform.localScale = new Vector3(3f, 2f, 1f);

            attackDelay = 1f;
            attackSpeed = 1f;
            speed = 1.3f;
            attackRange = 3f;
            traceRange = 8f;
            enemy.stat.MaxHP = 750;
            enemy.stat.Power = 0;
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

            enemy.stat.ChangeHP(-GM.pcStat.Power);
            Debug.Log("Hit!");
            //Debug.Log(gameObject.name + GetComponent<Enemy>().GetEnemyHp());
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
        //return (targetTransform.position - transform.position).magnitude;
        return (player.transform.position - transform.position).magnitude;
    }
    public bool CheckInTraceRange()
    {
        return (CalcTargetDistance() < traceRange) ? true : false;
    }
    public bool CheckInAttackRange()
    {
        //return (CalcTargetDistance() < attackRange && Mathf.Abs(transform.position.y - targetTransform.position.y) < 0.3f) ? true : false;
        return (CalcTargetDistance() < attackRange && Mathf.Abs(transform.position.y - player.transform.position.y) < 0.3f) ? true : false;
    }
    public bool CheckTargetInBush()
    {
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        return (playerSpriteRenderer.color.a == 0.5f) ? true : false;
    }
    public bool IsAlive()
    {
        return (enemy.stat.GetHP() > 0) ? true : false;
    }
    public bool IsAliveTarget()
    {
        //if (targetTransform == null) return false;
        if(player.transform == null) return false;

        return true;
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

	public void OnDestroy()
	{
        --enemyCount;
	}
	public void CheckPlayerDirectionX()
    {
        // 타겟이 왼쪽
        //if (transform.position.x - targetTransform.position.x > 0)
        //    playerDirectionX = PlayerDirectionX.LEFT;
        // 타겟이 오른쪽
        //else if (transform.position.x - targetTransform.position.x < 0)
        //    playerDirectionX = PlayerDirectionX.RIGHT;
        if(transform.position.x - player.transform.position.x > 0)
            playerDirectionX = PlayerDirectionX.LEFT;
        else if(transform.position.x - player.transform.position.x < 0)
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
        //if (Mathf.Abs(transform.position.x - targetTransform.position.x) <= range)
        //{
        //    // X축 거리는 유지하고 Z축만 이동
        //    if (playerDirectionX == PlayerDirectionX.LEFT)
        //        transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(+(transform.position.x - targetTransform.position.x), 0f), speed * Time.deltaTime);
        //    else
        //        transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(-(transform.position.x - targetTransform.position.x), 0f), speed * Time.deltaTime);
        //}
        //else
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
        //}
        if(Mathf.Abs(transform.position.x - player.transform.position.x) <= range)
        {
            // X축 거리는 유지하고 Z축만 이동
            if(playerDirectionX == PlayerDirectionX.LEFT)
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)player.transform.position + new Vector2(+(transform.position.x - player.transform.position.x), 0f), speed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)player.transform.position + new Vector2(-(transform.position.x - player.transform.position.x), 0f), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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

        if (attType == AttackType.RANGED || attType == AttackType.RANGED_ELITE)
        {
            //enemyAttackCollider.transform.position = targetTransform.position;
            //enemyWarningBox.transform.position = targetTransform.position;
            //enemyTimingBox.transform.position = targetTransform.position;
            enemyAttackCollider.transform.position = player.transform.position;
            enemyWarningBox.transform.position = player.transform.position;
            enemyTimingBox.transform.position = player.transform.position;
        }

        StartCoroutine(AttackActivation());
    }
    public bool GetIsAttackColliderActivation()
    {
        return isAttackColliderActivation;
    }
    IEnumerator AttackActivation()
    {
        isAttackColliderActivation = true;

        Color c;
        if (attType == AttackType.MELEE)
        {
            c = enemySpriteRenderer.material.color;
            c = Color.blue;
            c.a = 0.8f;

            enemySpriteRenderer.material.color = c;
        }
        else if (attType == AttackType.RANGED || attType == AttackType.RANGED_ELITE)
        {
            enemyWarningBox.SetActive(true);
        }
        else if (attType == AttackType.MELEE_ELITE)
        {
            c = enemySpriteRenderer.material.color;
            c = Color.blue;
            c.a = 0.8f;

            enemySpriteRenderer.material.color = c;

            enemyWarningBox.SetActive(true);
        }

        // 완벽한 회피 타이밍 활성화
        enemyTimingBox.SetActive(true);

        yield return new WaitForSeconds(attackDelay);

        c = Color.white;
        c.a = 1f;
        enemySpriteRenderer.material.color = c;

        // 완벽한 회피 타이밍 비활성화, 적 공격 범위 박스 비활성화
        enemyWarningBox.SetActive(false);
        enemyTimingBox.SetActive(false);

        enemyAttackCollider.SetActive(true);

        // 돌진
        if (attType == AttackType.MELEE_ELITE)
        {
            Vector2 dir;

            if(playerDirectionX == PlayerDirectionX.LEFT)
            {
                dir = new Vector2(transform.position.x - 10, transform.position.y);
            }
            else
            {
                dir = new Vector2(transform.position.x + 10, transform.position.y);
            }

            dir.x = Mathf.Clamp(dir.x, GM.CurRoomMgr.MapSizeMin.x, GM.CurRoomMgr.MapSizeMax.x);
            dir.y = Mathf.Clamp(dir.y, GM.CurRoomMgr.MapSizeMin.y, GM.CurRoomMgr.MapSizeMax.y);
            
            for(var f = 0f; f <= 1f; f += Time.deltaTime)
            {
                transform.position = Vector2.MoveTowards(transform.position, dir, 10 * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else if (attType == AttackType.RANGED_ELITE)
        {
            float x, y;

            for (int i = 0; i < 4; i++)
            {
                x = Random.Range(-2f, 2f);
                y = Random.Range(-1f, 1f);
                Instantiate(summonCreature, enemyWarningBox.transform.position + new Vector3(x, y, 0f), Quaternion.identity);
            }
        }

        
        // yield return new WaitForSeconds(attackSpeed);


        yield return new WaitForSeconds(attackSpeed);

        enemyAttackCollider.SetActive(false);

        isAttackColliderActivation = false;
    }
}