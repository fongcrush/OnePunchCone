using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static GameMgr;

public class Bush : IEnvironment
{
    public bool isBerryBush = false;
    public GameObject[] BerryObject;
    private PlayerController player;

    Coroutine StartRecoveryCoroutine = null;
    int berryCount;
    bool isDone;
    bool canTrigger;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        berryCount = 5;
        isDone = false;
        canTrigger = true;
    }
    override public void Stay(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if(collision.name=="Player")
                collision.transform.Find("Spine Animation").GetComponent<MeshRenderer>().material.color = (new Color(1, 1, 1, 0.5f));

            player.Bush(true);
            if (isBerryBush && StartRecoveryCoroutine == null && berryCount > 0 && GM.pcStat.curHP < GM.pcStat.MaxHP)
                StartRecoveryCoroutine = StartCoroutine(Recovery());
            if(canTrigger)
                StartCoroutine(SlowDebuff());
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.transform.GetComponent<EnemyController>().isInBush = true;
        }
    }
    override public void Exit(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (collision.name == "Player")
                collision.transform.Find("Spine Animation").GetComponent<MeshRenderer>().material.color = (new Color(1, 1, 1, 1f));
            player.Bush(false);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.transform.GetComponent<EnemyController>().isInBush = false;
        }
    }
    IEnumerator Recovery()
    {
        berryCount--;
        GM.pcStat.ChangeHP(GM.pcStat.MaxHP * 0.05f);
        BerryObject[berryCount].SetActive(false);
        yield return new WaitForSeconds(5.0f);
        StartRecoveryCoroutine = null;
    }

    IEnumerator SlowDebuff()
    {
        StartCoroutine(Timer(10));
        isDone = false;
        canTrigger = false;
        if (buffManager.SlowDebuffCount < 3)
            buffManager.SlowDebuffCount++;
        GameObject playerObject = GameObject.Find("Player");
        PlayerEffectController effectController = playerObject.GetComponent<PlayerEffectController>();
        effectController.SlowDebuff = true;
        yield return new WaitForSeconds(10 * buffManager.SlowDebuffCount - 1);
        if (canTrigger)
        {
            yield return new WaitForSeconds(1);
            isDone = true;
            if (isDone)
            {
                buffManager.SlowDebuffCount = 0;
                effectController.SlowDebuff = false;
            }
        }
    }
    IEnumerator Timer(int time)
    {
        yield return new WaitForSeconds(time);
        canTrigger = true;
    }
}