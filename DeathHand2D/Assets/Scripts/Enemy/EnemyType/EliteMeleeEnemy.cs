using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMeleeEnemy : Enemy
{
    Vector2 dir;
    PlayerDirectionX playerDirectionX;
    Rigidbody2D playerRigid;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = EnemyData.EnemyTable[3];

        stat = new Status(enemyInfo.monster_Hp, 0, enemyInfo.monster_Damage);

        playerRigid = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    public override IEnumerator Attack()
    {
        isAttackActivation = true;

        enemyController.SetAnimation("Elite_rush_1");

        playerDirectionX = (PlayerDirectionX)enemyController.GetPlayerDirectionX();

        enemyAttackWarningArea.SetActive(true);
        enemyAttackTimingBox.SetActive(true);

        yield return new WaitForSeconds(enemyInfo.monster_AttackDelay * 1.8f);

        enemyAttackWarningArea.SetActive(false);
        enemyAttackTimingBox.SetActive(false);

        enemyAttackCollider.SetActive(true);

        enemyController.SetAnimation("Elite_rush_2");

        if (playerDirectionX == PlayerDirectionX.LEFT)
        {
            dir = new Vector2(transform.position.x - 10, transform.position.y);
        }
        else
        {
            dir = new Vector2(transform.position.x + 10, transform.position.y);
        }

        dir.x = Mathf.Clamp(dir.x, gm.CurRoomMgr.MapSizeMin.x, gm.CurRoomMgr.MapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, gm.CurRoomMgr.MapSizeMin.y, gm.CurRoomMgr.MapSizeMax.y);

        // Á¶Á¤
        for (var f = 0f; f <= 1f; f += Time.deltaTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, dir, 10 * Time.deltaTime);
            if (!isPlayerHit)
            {
                CheckCollider();
                if (isPlayerHit)
                {
                    StartCoroutine(KnockBackPlayer());
                }
            }
                yield return new WaitForFixedUpdate();
        }

        if (isPlayerHit)
        {
            isPlayerHit = false;
        }

        enemyAttackCollider.SetActive(false);

        isAttackActivation = false;
    }
    IEnumerator KnockBackPlayer()
    {
        Vector3 dir;

        if(enemyController.GetTargetTransformPosition().y > transform.position.y)
        {
            dir = enemyController.GetTargetTransformPosition() + new Vector3(0, 3, 0);
        }
        else
        {
            dir = enemyController.GetTargetTransformPosition() + new Vector3(0, -3, 0);
        }
        dir.y = Mathf.Clamp(dir.y, gm.CurRoomMgr.MapSizeMin.y, gm.CurRoomMgr.MapSizeMax.y);

        float curTime = 0;

        while (curTime < 0.5f)
        {
            playerRigid.MovePosition(Vector3.Lerp(playerRigid.position, dir, Time.deltaTime * 20));
            curTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
