using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;

public class PowderKeg : IEnvironment
{
    public Sprite[] Sprites;
    public GameObject explosion;
    public GameObject explosionFX;
    float Hp;

    private void Start()
    {
        Hp = EnvironmentData.EnvironmentTable["PowderKeg"].durability;
    }

    public override void Stay(Collider2D collision)
    {

    }

    public override void Exit(Collider2D collision)
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Untagged" && collision.gameObject.layer == 6 && GetComponent<BoxCollider2D>().enabled == false)
        {
            GM.pcStat.ChangeHP(-50);
        }
        if (collision.gameObject.tag == "Enemy" && GetComponent<BoxCollider2D>().enabled == false)
        {
            if(collision.GetComponent<EnemyController>().attTypeValue == 2 || collision.GetComponent<EnemyController>().attTypeValue == 4)
                collision.gameObject.GetComponent<EnemyController>().Hit(500);
            else
                collision.gameObject.GetComponent<EnemyController>().Hit(50);
        }
    }

    public void Hit(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            StartExplosion();
        }
    }

    void StartExplosion()
    {
        StartEffect();
        StartCoroutine(Explosion());

        GetComponent<SpriteRenderer>().sprite = Sprites[1];
    }

    void StartEffect()
    {
        explosionFX.SetActive(true);
        explosionFX.GetComponent<ParticleSystem>().Play();
        //불꽃 파티클 재생
        Destroy(explosionFX, 2f);
    }
    IEnumerator Explosion()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        explosion.SetActive(true);
        yield return null;
        explosion.GetComponent<BoxCollider2D>().enabled = false;
    }
}
