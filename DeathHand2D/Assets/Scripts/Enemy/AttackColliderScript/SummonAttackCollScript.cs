using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;

public class SummonAttackCollScript : MonoBehaviour
{
    Enemy enemy;
    private void Awake()
    {
        enemy = GetComponentInParent<SummonScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            GM.pcStat.curHP -= enemy.GetEnemyDamage();
    }
}
