using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public StatusManager stat;
    private void Awake()
    {
        stat = new StatusManager(700, 0, 10);
        //Debug.Log("Enemy Hp: " + stat.curHP);
    }
    public float GetEnemyHp()
    {
        return stat.curHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
