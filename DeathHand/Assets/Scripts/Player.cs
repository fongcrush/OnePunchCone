using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    PlayerController playerController;

    private void Awake()
    {        
        Hp = 10.0f;
    }

    // Start is called before the first frame update
    void Start()
    {        
        playerController = GetComponent<PlayerController>();   
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy") 
        {
            Hp -= 0.5f;
            Debug.Log("Player Hp: "+Hp);
        }
    }
}
