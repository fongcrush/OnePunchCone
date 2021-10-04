using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Bush : IEnvironment
{
    override public void Stay(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            collision.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().isInBush = true;
        }
    }
    override public void Exit(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            collision.GetComponent<SpriteRenderer>().sortingOrder = PlayerLayer;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().isInBush = false;
        }
    }   
}
