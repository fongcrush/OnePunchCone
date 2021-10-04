using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Bush : IEnvironment
{
    private PlayerController player;

	private void Awake()
	{
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
	override public void Stay(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.GetComponent<SkeletonAnimation>().skeleton.SetColor(new Color(1, 1, 1, 0.5f));
            collision.GetComponent<MeshRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
            player.Bush(true);
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
            collision.GetComponent<SkeletonAnimation>().skeleton.SetColor(new Color(1, 1, 1, 1));
            collision.GetComponent<MeshRenderer>().sortingOrder = PlayerLayer;
            player.Bush(false);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().isInBush = false;
        }
    }
}
