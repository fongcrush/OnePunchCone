using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitColl : MonoBehaviour
{
    EnemyController enemyController;

    private void Awake()
    {
        enemyController = gameObject.GetComponentInParent<EnemyController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnvironmentAttackCollider")
        {
            if (enemyController.attTypeValue == (int)AttackType.RANGED || enemyController.attTypeValue == (int)AttackType.RANGED_ELITE)
                enemyController.Hit(500);
            else
                enemyController.Hit(50);
        }
    }
}
