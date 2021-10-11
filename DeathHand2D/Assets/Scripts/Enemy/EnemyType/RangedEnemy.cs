using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RangedEnemy : Enemy
{
    public GameObject summonCreature;
    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = EnemyData.EnemyTable[2];

        stat = new Status(enemyInfo.monster_Hp, 0, enemyInfo.monster_Damage);
    }
    public override IEnumerator Attack()
    {
        enemyAttackCollider.transform.position = enemyController.GetTargetTransformPosition();
        enemyAttackWarningArea.transform.position = enemyController.GetTargetTransformPosition();
        enemyAttackTimingBox.transform.position = enemyController.GetTargetTransformPosition();

        isAttackActivation = true;

        enemyAttackWarningArea.SetActive(true);
        enemyAttackTimingBox.SetActive(true);

        yield return new WaitForSeconds(enemyInfo.monster_AttackDelay);

        enemyAttackWarningArea.SetActive(false);
        enemyAttackTimingBox.SetActive(false);

        Instantiate(summonCreature, enemyAttackWarningArea.transform.position, Quaternion.identity);

        // rangedEnemyAttackSkeletonAnime.AnimationState.SetAnimation(0, "animation", true).MixDuration = 0f;

        enemyAttackCollider.SetActive(true);

        //Attack
        CheckCollider();

        yield return new WaitForSeconds(enemyInfo.monster_AttackSpeed);

        enemyAttackCollider.SetActive(false);

        isAttackActivation = false;
    }
}
