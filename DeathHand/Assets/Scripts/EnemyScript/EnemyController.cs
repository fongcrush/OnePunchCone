using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AttackType
{
    MELEE,
    RANGED
}

public class EnemyController : MonoBehaviour
{
    private EnemyBaseState currentState;
    private EnemyBaseState prevState;

    public GameObject EnemyAttackCollider;
    public GameObject EnemyWarningBox;
    public GameObject Arrow;
    private MeshRenderer EnemyWarningBoxMesh;

    public int AttTypeValue;
    private AttackType AttType;

    public float TraceRange = 5.0f;
    public float AttackRange = 2.0f;
    public float Speed = 7.5f;

    private float Range = 0.5f;

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

    private void Awake()
    {
        EnemyWarningBoxMesh = EnemyWarningBox.GetComponent<MeshRenderer>();
        AttType = (AttackType)AttTypeValue;
        Debug.Log(AttType);
        currentState = IdleState;
    }
    private void Start()
    {
        if (AttType == AttackType.RANGED)
        {
            EnemyWarningBox.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z);
            EnemyWarningBox.transform.localScale = new Vector3(4f, 1f, 1f);
            Range = 1.99f;
        }

        ChangeState(IdleState);
    }
    private void Update()
    {
        currentState.Update(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerAttackCollider")
        {
            // �ǰ� ������ �߰� �� ����
            if (isOnDamage)
                return;
            // ������ ���� �ʿ�
            gameObject.GetComponent<Enemy>().OnDamage(50.0f);
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
        return (CalcTargetDistance() <= TraceRange) ? true : false;
    }
    public bool CheckInAttackRange()
    {
        return (CalcTargetDistance() <= AttackRange && Mathf.Abs(transform.position.z - targetTransform.position.z) < 0.3f) ? true : false;
    }
    public bool IsAlive()
    {
        return (gameObject.GetComponent<Enemy>().GetEnemyHp() > 0) ? true : false;
    }
    public bool IsAliveTarget()
    {
        if (targetTransform == null) return false;

        return true;
    }
    public void CheckTargetPosition()
    {
        // Ÿ���� ����
        if (transform.position.x - targetTransform.position.x > 0 && transform.rotation != Quaternion.Euler(0f, 0f, 0f))
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        // Ÿ���� ������
        else if (transform.position.x - targetTransform.position.x < 0 && transform.rotation != Quaternion.Euler(0f, 180f, 0f))
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);
    }
    public void CheckXPosition()
    {
        // �÷��̾�� ���� �Ÿ��� Range ������ ���� ���
        if (Mathf.Abs(transform.position.x - targetTransform.position.x) <= Range)
        {
            // X�� �Ÿ��� �����ϰ� Z�ุ �̵�
            if (transform.position.x - targetTransform.position.x > 0)
                transform.position = new Vector3(targetTransform.position.x + Range, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(targetTransform.position.x - Range, transform.position.y, transform.position.z);
        }
    }
    public void TraceTarget()
    {
        Vector3 velo = Vector3.zero;
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Speed * Time.deltaTime);

        CheckXPosition();
        CheckTargetPosition();

        // Y�� ����
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }
    public void Attack()
    {
        CheckTargetPosition();

        StartCoroutine("AttackActivation");
    }
    public bool GetIsAttackColliderActivation()
    {
        return isAttackColliderActivation;
    }
    IEnumerator AttackActivation()
    {
        // �� ���� ���� �ڽ� Ȱ��ȭ
        EnemyWarningBox.SetActive(true);
        isAttackColliderActivation = true;

        // �� ���� �ڽ��� ������ 0���� 0.5���� ����
        for (var f = 0f; f <= 0.5f; f += 0.05f)
        {
            var c = EnemyWarningBoxMesh.material.color;
            c.a = f;
            EnemyWarningBoxMesh.material.color = c;
            yield return new WaitForSeconds(.1f);
        }
        // �� ���� ���� �ڽ� ��Ȱ��ȭ, �� ���� ��Ʈ �ڽ� Ȱ��ȭ �� Damage
        EnemyWarningBox.SetActive(false);
        
        if(AttType == AttackType.MELEE)
            EnemyAttackCollider.SetActive(true);
        else if(AttType == AttackType.RANGED)
        {
            GameObject arrow = Instantiate(Arrow, new Vector3(transform.position.x - 0.5f, 1.5f, transform.position.z), Quaternion.identity);
            Rigidbody rigid = arrow.GetComponent<Rigidbody>();

            if (transform.position.x - EnemyWarningBox.transform.position.x > 0)
                rigid.AddForce(Vector3.left * 20f, ForceMode.Impulse);
            else
            {
                arrow.transform.position = new Vector3(transform.position.x + 0.5f, 1.5f, transform.position.z);
                rigid.AddForce(Vector3.right * 20f, ForceMode.Impulse);
            }
        }

        // n�� �� ���� ����
        // yield return new WaitForSeconds();

        // 0.2�� �� �ڷ�ƾ ���� �� �� ���� �ڽ� ��Ȱ��ȭ
        yield return new WaitForSeconds(0.2f);

        EnemyAttackCollider.SetActive(false);
        isAttackColliderActivation = false;
    }
}
