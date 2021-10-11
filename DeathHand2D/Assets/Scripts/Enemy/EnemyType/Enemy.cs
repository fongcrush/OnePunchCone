using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyData;

public class Enemy : MonoBehaviour
{
    public Status stat;

    protected EnemyController enemyController;
    protected EnemyInfo enemyInfo;

    [SerializeField]
    protected GameObject enemyAttackCollider;
    [SerializeField]
    protected GameObject enemyAttackWarningArea;
    [SerializeField]
    protected GameObject enemyAttackTimingBox;

    protected bool isAttackActivation = false;

    private void Awake()
    {
        ReadAttackData();
        enemyController = GetComponent<EnemyController>();
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
}
