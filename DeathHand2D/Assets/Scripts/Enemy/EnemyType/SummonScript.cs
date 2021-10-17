using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;


public class SummonScript : Enemy
{
    float BombTimer = 0f;

    bool isAttack = false;
    bool isArrival = false;

    SpriteRenderer summonMesh;
    public GameObject explosionEffect;
    public Vector3 targetPosition;

    Color c;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerAttackCollider")
        {
            enemyInfo.monster_Hp -= GM.pcStat.Power;

            if (enemyInfo.monster_Hp < 0)
                Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = EnemyData.EnemyTable[5];

        stat = new Status(enemyInfo.monster_Hp, 0, enemyInfo.monster_Damage);
        summonMesh = GetComponent<SpriteRenderer>();
        c = summonMesh.material.color;

        StartCoroutine(MovePosition(targetPosition));
    }
    public override IEnumerator Attack()
    {
        enemyAttackWarningArea.SetActive(true);
        enemyAttackTimingBox.SetActive(true);
        for (float i = 0; i < enemyInfo.monster_AttackDelay; i += Time.deltaTime)
        {
            c = new Color(c.r, c.g, c.b + i, c.a);
            summonMesh.material.color = c;

            yield return null;
        }

        isAttack = true;

        enemyAttackTimingBox.SetActive(false);
        enemyAttackWarningArea.SetActive(false);

        yield return new WaitForSeconds(enemyInfo.monster_AttackSpeed);

        enemyAttackCollider.SetActive(true);
        
        var explosionObject = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosionObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);

        yield return new WaitForSeconds(0.1f);

        enemyAttackCollider.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        Destroy(explosionObject);
        Destroy(gameObject);
    }
    public IEnumerator MovePosition(Vector3 position)
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, position, enemyInfo.monster_Speed * Time.deltaTime);

            if (((position - transform.position).magnitude) < 0.1f)
            {
                break;
            }
            yield return null;
        }
        StartCoroutine(Attack());
    }
    // Update is called once per frame
}
