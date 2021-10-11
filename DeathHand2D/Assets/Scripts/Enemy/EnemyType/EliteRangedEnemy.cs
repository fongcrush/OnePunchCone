using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteRangedEnemy : Enemy
{
    public GameObject summonCreature;
    private float x, y;
    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = EnemyData.EnemyTable[4];

        stat = new Status(enemyInfo.monster_Hp, 0, enemyInfo.monster_Damage);
    }

    override public IEnumerator Attack()
    {
        enemyAttackCollider.transform.position = enemyController.GetTargetTransformPosition();
        enemyAttackTimingBox.transform.position = enemyController.GetTargetTransformPosition();
        enemyAttackWarningArea.transform.position = enemyController.GetTargetTransformPosition();

        isAttackActivation = true;

        enemyAttackWarningArea.SetActive(true);
        enemyAttackTimingBox.SetActive(true);

        yield return new WaitForSeconds(enemyInfo.monster_AttackDelay);

        enemyAttackWarningArea.SetActive(false);
        enemyAttackTimingBox.SetActive(false);

        enemyAttackCollider.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            x = Random.Range(-2f, 2f);
            y = Random.Range(-1f, 1f);
            Instantiate(summonCreature, enemyAttackWarningArea.transform.position + new Vector3(x, y, 0f), Quaternion.identity);
        }

        yield return new WaitForSeconds(enemyInfo.monster_AttackSpeed);

        enemyAttackCollider.SetActive(false);

        isAttackActivation = false;
    }
}
