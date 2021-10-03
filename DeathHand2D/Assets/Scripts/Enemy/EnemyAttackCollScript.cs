using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollScript : MonoBehaviour
{
    GameMgr gm;
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameMgr>();
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            gm.pcStat.ChangeHP(-enemy.stat.Power);
        }
    }
}
