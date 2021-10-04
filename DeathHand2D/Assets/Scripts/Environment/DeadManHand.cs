using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class DeadManHand : IEnvironment
{
    BuffMgr buffManager;
    Vignette vignette;

    bool isDone;
    bool canTrigger;

    private void Start()
    {
        isDone = false;
        canTrigger = true;
        buffManager = GameObject.Find("UI").GetComponent<BuffMgr>();
        buffManager.DarkDebuffCount = 0;

    }
    public override void Stay(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder - 1;
            if (canTrigger)
            {
                Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out vignette);
                StartCoroutine(DarkDebuff());
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

    public override void Exit(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<SpriteRenderer>().sortingOrder = PlayerLayer;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<SpriteRenderer>().sortingOrder = EnemyLayer;
        }
    }

    IEnumerator CenterToPlayer(GameObject playerObject)
    {
        while (!isDone)
        {
            Vector2 pos = Camera.main.WorldToViewportPoint(playerObject.transform.position);
            vignette.center.value = pos;
            yield return null;
        }
    }

    IEnumerator FadeOut(float time)
    {
        float intensity = vignette.intensity.value;

        while (intensity > 0f)
        {
            vignette.intensity.value = intensity;
            intensity -= Time.deltaTime / time;
            if (intensity <= 0f) vignette.intensity.value = 0;
            yield return null;
        }
    }

    IEnumerator DarkDebuff()
    {
        StartCoroutine(Timer(10));
        isDone = false;
        canTrigger = false;
        if (buffManager.DarkDebuffCount < 3)
            buffManager.DarkDebuffCount++;
        GameObject playerObject = GameObject.Find("Player");
        StartCoroutine(CenterToPlayer(playerObject));
        PlayerEffectController effectController = playerObject.GetComponent<PlayerEffectController>();
        effectController.DarkDebuff = true;
        switch (buffManager.DarkDebuffCount)
        {
            case 1:
                vignette.intensity.value = 0.55f;
                break;
            case 2:
                vignette.intensity.value = 0.6f;
                break;
            case 3:
                vignette.intensity.value = 0.63f;
                break;
        }
        yield return new WaitForSeconds(15 * buffManager.DarkDebuffCount - 1);
        StartCoroutine(FadeOut(1));
        yield return new WaitForSeconds(1);
        isDone = true;
        if (isDone)
        {
            buffManager.DarkDebuffCount = 0;
            effectController.DarkDebuff = false;
        }
    }
    IEnumerator Timer(int time)
    {
        yield return new WaitForSeconds(time);
        canTrigger = true;
        //Debug.Log("ready");
    }
}
