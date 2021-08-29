using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private void Awake()
    {
        Hp = 700.0f;
        Debug.Log("Enemy Hp: " + Hp);
    }

    public void OnDamage(float Damage)
    {
        Hp -= Damage;
    }
    public float GetEnemyHp()
    {
        return Hp;
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
