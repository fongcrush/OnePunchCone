using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static EnemyData;
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

    public GameObject enemyAttackCollider;
    public GameObject enemyWarningBox;
    public GameObject enemyTimingBox;
    public GameObject summonCreature;

    private SpriteRenderer enemyWarningBoxMesh;
    private SkeletonAnimation skeletonAnime;
    private SpriteRenderer spriteRenderer;
    public SkeletonAnimation rangedEnemyAttackSkeletonAnime;

    public EnemyInfo enemyInfo;

    public int attTypeValue;
    private AttackType attType;

    private PlayerDirectionX playerDirectionX;

    private Transform targetTransform;

    private bool isAttackColliderActivation = false;
    public bool isInBush = false;
    private bool isHitCheck = false;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        skeletonAnime = GetComponent<SkeletonAnimation>();

        attType = (AttackType)attTypeValue;
        
        player = GameObject.Find("Player");
        targetTransform = player.transform;
        enemy = GetComponent<Enemy>();

        ++enemyCount;

    }
    private void Start()
    {

        EnemyData.ReadAttackData();
        if (attType == AttackType.MELEE)
        {
            enemyInfo = EnemyData.EnemyTable[1];

            enemy.stat.MaxHP = enemyInfo.monster_Hp;
            enemy.stat.Power = enemyInfo.monster_Damage;
        }
        if (attType == AttackType.RANGED)
        {
            enemyWarningBox.transform.localScale = new Vector3(1f, 0.5f, 1f);
            enemyTimingBox.transform.localScale = new Vector3(1f, 0.5f, 1f);

            enemyInfo = EnemyData.EnemyTable[2];

            enemy.stat.MaxHP = enemyInfo.monster_Hp;
            enemy.stat.Power = enemyInfo.monster_Damage;
        }
        else if (attType == AttackType.MELEE_ELITE)
        {
            enemyAttackCollider.transform.localScale = new Vector3(3f, 5f, 1f);
            enemyWarningBox.transform.localScale = new Vector3(10f, 1f, 1f);
            enemyTimingBox.transform.localScale = new Vector3(10f, 1f, 1f);
            enemyAttackCollider.transform.position = transform.position + new Vector3(0f, 3f, 0f);
            enemyWarningBox.transform.position = transform.position + new Vector3(-5f, 0f, 0f);
            enemyTimingBox.transform.position = transform.position + new Vector3(-5f, 0f, 0f);

            enemyInfo = EnemyData.EnemyTable[3];

            enemy.stat.MaxHP = enemyInfo.monster_Hp;
            enemy.stat.Power = enemyInfo.monster_Damage;
        }
        else if (attType == AttackType.RANGED_ELITE)
        {
            enemyAttackCollider.transform.localScale = new Vector3(3f, 2f, 1f);
            enemyWarningBox.transform.localScale = new Vector3(3f, 2f, 1f);
            enemyTimingBox.transform.localScale = new Vector3(3f, 2f, 1f);

            enemyInfo = EnemyData.EnemyTable[4];

            enemy.stat.MaxHP = enemyInfo.monster_Hp;
            enemy.stat.Power = enemyInfo.monster_Damage;
        }

        ChangeState(IdleState);
    }
    private void Update()
    {
        currentState.Update(this);
        Debug.Log(targetTransform.position);
        if (attType == AttackType.RANGED || attType == AttackType.RANGED_ELITE)
        {
            if (!isHitCheck && enemyInfo.chase_Range < 10)
                CheckEnemysHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttackCollider")
        {
            enemy.stat.ChangeHP(-GM.pcStat.Power);
            Debug.Log("Hit!");
            currentState.OnCollisionEnter(this);
            GM.SetEnemyHit(true);
        }

    }

    
    private void CheckEnemysHit()
    {
        if (GM.GetEnemyHit() == true)
        {
            enemyInfo.chase_Range *= 3f;
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
        if(currentState == IdleState)
        {
            if (attType == AttackType.MELEE)
                skeletonAnime.AnimationName = "idle";
            else if (attType == AttackType.RANGED)
                skeletonAnime.AnimationName = "animation";
        }
        else if(currentState == TraceState)
        {
            if (attType == AttackType.MELEE || attType == AttackType.RANGED)
                skeletonAnime.AnimationName = "walking";
        }
        else if(currentState == AttackState)
        {
            if (attType == AttackType.MELEE || attType == AttackType.RANGED)
                skeletonAnime.AnimationName = "attack";
        }
    }
    public float CalcTargetDistance()
    {
        return (player.transform.position - transform.position).magnitude;
    }
    public bool CheckInTraceRange()
    {
        return (CalcTargetDistance() < enemyInfo.chase_Range) ? true : false;
    }
    public bool CheckInAttackRange()
    {
        return (CalcTargetDistance() < enemyInfo.attack_Range && Mathf.Abs(transform.position.y - player.transform.position.y) < 0.3f) ? true : false;
    }
    public bool CheckTargetInBush()
    {
        if(player.GetComponent<PlayerController>().InBush)
        {
            if (enemyInfo.chase_Range > 10)
                enemyInfo.chase_Range /= 3f;
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
        return (enemy.stat.GetHP() > 0) ? true : false;
    }
    public bool IsAliveTarget()
    {
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
        if(transform.position.x - targetTransform.position.x > 0)
            playerDirectionX = PlayerDirectionX.LEFT;
        else if(transform.position.x - targetTransform.position.x < 0)
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
        if(Mathf.Abs(transform.position.x - targetTransform.position.x) <= enemyInfo.monster_MinDistance)
        {
            // X축 거리는 유지하고 Z축만 이동
            if(playerDirectionX == PlayerDirectionX.LEFT)
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(+(transform.position.x - targetTransform.position.x), 0f), enemyInfo.monster_Speed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetTransform.position + new Vector2(-(transform.position.x - targetTransform.position.x), 0f), enemyInfo.monster_Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, enemyInfo.monster_Speed * Time.deltaTime);
        }
    }
    public void TraceTarget()
    {
        if (enemyInfo.chase_Range < 10)
            enemyInfo.chase_Range *= 3f;

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
        isAttackColliderActivation = true;

        if (attType == AttackType.MELEE)
        {
            enemyWarningBox.SetActive(true);
        }
        else if (attType == AttackType.RANGED)
        {
            enemyWarningBox.SetActive(true);
            rangedEnemyAttackSkeletonAnime.AnimationState.SetAnimation(0, "animation", false).MixDuration = 0f;
        }
        else if (attType == AttackType.MELEE_ELITE)
        {
            enemyWarningBox.SetActive(true);
        }
        else if(attType == AttackType.RANGED_ELITE)
        {
            enemyWarningBox.SetActive(true);
        }

        // 완벽한 회피 타이밍 활성화
        enemyTimingBox.SetActive(true);

        yield return new WaitForSeconds(enemyInfo.monster_AttackDelay);

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

        yield return new WaitForSeconds(enemyInfo.monster_AttackSpeed);

        enemyAttackCollider.SetActive(false);

        isAttackColliderActivation = false;
    }
}