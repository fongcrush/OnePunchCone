using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadManHand : Environment
{
    bool isDone;
    [SerializeField]
    byte stack;

    private void Start()
    {
        isDone = true;
        stack = 0;
    }

    public void TriggerEnterDeadManHand(Collider2D collision, bool enter)
    {
        if (enter)
        {
            if (environmentName == "DeadManHand" && collision.gameObject.tag == "Player")
            {
                collision.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder - 1;
                if (isDone) 
                {
                    StartCoroutine(DarkDebuff());
                }
            }
            if (environmentName == "DeadManHand" && collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }
        else
        {
            if (environmentName == "DeadManHand" && collision.gameObject.tag == "Player")
            {
                collision.GetComponent<SpriteRenderer>().sortingOrder = PlayerLayer;
            }
            if (environmentName == "DeadManHand" && collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<SpriteRenderer>().sortingOrder = EnemyLayer;
            }
        }
    }

    IEnumerator DarkDebuff() 
    {
        StartCoroutine(Timer(10));
        isDone = false;
        if(stack < 3)
            stack++;
        PlayerEffectController effectController = GameObject.Find("Player").GetComponent<PlayerEffectController>();
        effectController.DarkDebuff = true;
        yield return new WaitForSeconds(15 * stack);
        if (isDone)
        {
            stack = 0;
            effectController.DarkDebuff = false;
        }
    }
    IEnumerator Timer(int time)
    {
        yield return new WaitForSeconds(time);
        isDone = true;
    }
}
