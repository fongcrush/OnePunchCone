using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = EnemyData.EnemyTable[1];

        stat = new Status(enemyInfo.monster_Hp, 0, enemyInfo.monster_Damage);
    }
    public override IEnumerator Attack()
    {
        isAttackActivation = true;

        enemyAttackWarningArea.SetActive(true);
        enemyAttackTimingBox.SetActive(true);

        yield return new WaitForSeconds(enemyInfo.monster_AttackDelay);

        enemyAttackWarningArea.SetActive(false);
        enemyAttackTimingBox.SetActive(false);

        enemyAttackCollider.SetActive(true);

        yield return new WaitForSeconds(enemyInfo.monster_AttackSpeed);

        enemyAttackCollider.SetActive(false);

        isAttackActivation = false;
    }
}
