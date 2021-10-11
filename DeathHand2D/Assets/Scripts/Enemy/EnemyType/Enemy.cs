using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyData;
using static GameMgr;

public class Enemy : MonoBehaviour
{
    public Status stat;

    protected GameMgr gm;

    protected EnemyController enemyController;
    protected EnemyInfo enemyInfo;

    [SerializeField]
    protected GameObject enemyAttackCollider;
    [SerializeField]
    protected GameObject enemyAttackWarningArea;
    [SerializeField]
    protected GameObject enemyAttackTimingBox;

    protected bool isAttackActivation = false;

    [SerializeField]
    protected BoxCollider2D enemyAttackBoxColl;

    private void Awake()
    {
        gm = GM;
        ReadAttackData();
        enemyController = GetComponent<EnemyController>();
    }
    public void AttCollsSetActiveFalse()
    {
        enemyAttackCollider.SetActive(false);
        enemyAttackTimingBox.SetActive(false);
        enemyAttackWarningArea.SetActive(false);
    }
    public float GetEnemyDamage()
    {
        return enemyInfo.monster_Damage;
    }
    public float GetEnemySpeed()
    {
        return enemyInfo.monster_Speed;
    }
    public bool GetIsAttackActivation()
    {
        return isAttackActivation;
    }
    public void SetChaseRange(float value)
    {
        enemyInfo.chase_Range = value;
    }
    public float GetChaseRange()
    {
        return enemyInfo.chase_Range;
    }
    public float GetAttackRange()
    {
        return enemyInfo.attack_Range;
    }
    public float GetMinDistance()
    {
        return enemyInfo.monster_MinDistance;
    }
    public void ChangeEnemyWarningAreaZRotation(Quaternion rotation)
    {
        enemyAttackWarningArea.transform.rotation = rotation;
    }
    virtual public IEnumerator Attack()
    {
        Debug.Log("Enemy Attack");
        yield return 0;
    }

    protected void CheckCollider()
    {
        RaycastHit2D[] hitResults = new RaycastHit2D[100];
        for (int i = 0; i < enemyAttackBoxColl.Cast(Vector2.left, hitResults, 0); i++)
        {
            if (hitResults[i].collider.gameObject.tag == "Jangseung")
            {
                Debug.Log(hitResults[i].collider.gameObject.name);
                hitResults[i].collider.gameObject.transform.GetComponent<Jangseung>().Hit(enemyInfo.monster_Damage);
                break;
            }
            if (hitResults[i].collider.gameObject.tag == "PowderKeg")
            {
                Debug.Log(hitResults[i].collider.gameObject.name);
                hitResults[i].collider.gameObject.transform.GetComponent<PowderKeg>().Hit(enemyInfo.monster_Damage);
                break;
            }
            if (hitResults[i].collider.gameObject.tag == "Player")
            {
                Debug.Log(hitResults[i].collider.gameObject.name);
                gm.pcStat.curHP -= enemyInfo.monster_Damage;
            }
        }
    }
}
