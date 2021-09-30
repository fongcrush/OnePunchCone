using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadManHand : Environment
{
    bool isDone;
    BuffMgr buffManager;

    float fade;

    private void Start()
    {
        isDone = true;
        buffManager = GameObject.Find("@GM").GetComponent<BuffMgr>();
        buffManager.DarkDebuffCount = 0;
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

    IEnumerator FadeOut(PlayerEffectController effectController, float time) 
    {
        float alpha = 1;
        while (alpha > 0f && isDone)
        {
            effectController.DarkDebuffEffect.GetComponent<SpriteRenderer>().color = new Color(1,1,1,alpha);
            alpha -= Time.deltaTime / time;
            if (alpha <= 0f) effectController.DarkDebuffEffect.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
             yield return null;
        }
    }

    IEnumerator DarkDebuff() 
    {
        StartCoroutine(Timer(10));
        isDone = false;
        if(buffManager.DarkDebuffCount < 3)
            buffManager.DarkDebuffCount++;
        PlayerEffectController effectController = GameObject.Find("Player").GetComponent<PlayerEffectController>();
        effectController.DarkDebuff = true;
        effectController.DarkDebuffEffect.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        yield return new WaitForSeconds(15 * buffManager.DarkDebuffCount - 1);
        StartCoroutine(FadeOut(effectController, 1));
        yield return new WaitForSeconds(1);
        if (isDone)
        {
            buffManager.DarkDebuffCount = 0;
            effectController.DarkDebuff = false;
        }
    }
    IEnumerator Timer(int time)
    {
        yield return new WaitForSeconds(time);
        isDone = true;
        Debug.Log("ready");
    }
}
