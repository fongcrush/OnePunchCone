using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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
                // collision.GetComponent<MeshRenderer>().sortingOrder = transform.GetComponent<MeshRenderer>().sortingOrder - 1;
                collision.GetComponent<EnemyController>().isInBush = true;
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
                // collision.GetComponent<SpriteRenderer>().sortingOrder = EnemyLayer;
                collision.GetComponent<EnemyController>().isInBush = false;
            }
        }
    }
}
