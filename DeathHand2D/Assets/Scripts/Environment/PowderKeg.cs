using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;

public class PowderKeg : IEnvironment
{
    public GameObject explosion;
    float Hp;

    private void Start()
    {
        Hp = EnvironmentData.EnvironmentTable["PowderKeg"].durability;
    }

    public override void Stay(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttackCollider")
        {
            this.ChangeHP(-GM.pcStat.Power);
        }
    }

    public override void Exit(Collider2D collision)
    {

    }

    void ChangeHP(float damage)
    {
        Hp += damage;
        if (Hp <= 0)
        {
            StartExplosion();
        }
    }

    void StartExplosion()
    {
        explosion.SetActive(true);
        StartEffect();

        GetComponent<BoxCollider2D>().enabled = false;
        explosion.GetComponent<BoxCollider2D>().enabled = false;
    }

    void StartEffect()
    {
        //불꽃 파티클 재생
    }
}
