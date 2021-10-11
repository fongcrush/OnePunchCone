using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;

public class EnemyAttackCollScript : MonoBehaviour
{
    private Enemy enemy;
    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();

        if (enemyController.attTypeValue == (int)AttackType.MELEE)
            enemy = GetComponentInParent<MeleeEnemy>();
        else if (enemyController.attTypeValue == (int)AttackType.RANGED)
            enemy = GetComponentInParent<RangedEnemy>();
        else if (enemyController.attTypeValue == (int)AttackType.MELEE_ELITE)
            enemy = GetComponentInParent<EliteMeleeEnemy>();
        else if (enemyController.attTypeValue == (int)AttackType.RANGED_ELITE)
            enemy = GetComponentInParent<EliteRangedEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GM.pcStat.curHP -= enemy.GetEnemyDamage();
        }
    }
}
