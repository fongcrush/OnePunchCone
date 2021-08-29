using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private void Awake()
    {
        Hp = 10.0f;
        Debug.Log("Enemy Hp: " + Hp);
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
