using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Environment
{
    public void TriggerEnterBush(Collider2D collision, bool enter)
    {
        if (enter)
        {
            if (environmentName == "Bush" && collision.gameObject.tag == "Player")
            {
                collision.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                collision.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            if (environmentName == "Bush" && collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder - 1;
                // if(collision.GetComponent<EnemyController>().CurrentState == new EnemyTraceState() || collision.GetComponent<EnemyController>().CurrentState == new EnemyAttackState()|| collision.GetComponent<EnemyController>().CurrentState == new EnemyHitState()) 
                // {
                    // collision.GetComponent<EnemyController>().ChangeState(new EnemyTraceState());
                    // 5초 지나면
                    // collision.GetComponent<EnemyController>().ChangeState(new EnemyIdleState());
                // }
            }
        }
        else
        {
            if (environmentName == "Bush" && collision.gameObject.tag == "Player")
            {
                collision.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                collision.GetComponent<SpriteRenderer>().sortingOrder = PlayerLayer;
            }
            if (environmentName == "Bush" && collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<SpriteRenderer>().sortingOrder = EnemyLayer;
            }
        }
    }
}
