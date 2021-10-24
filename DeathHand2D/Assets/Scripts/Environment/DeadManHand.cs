using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Rendering.PostProcessing;

public class DeadManHand : IEnvironment
{
   // Vignette vignette;

    bool isDone;
    bool canTrigger;
    int curCount;

    private void Awake()
    {
        isDone = false;
        canTrigger = true;
        curCount = 0;
    }

    public override void Stay(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            // 플레이어가 망자의 손 뒤로 보이도록 레이어 설정해주세요
            if (canTrigger && collision.name == "Player")
            {
                if (buffManager.DarkDebuffCount < 3)
                {
                   // Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out vignette);
                    StartCoroutine(DarkDebuff());
                }
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            // 적이 망자의 손 위로 보이도록 레이어 설정해주세요
        }
    }

    public override void Exit(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            // 레이어를 원상복구시켜주세요 
        }
        if (collision.gameObject.tag == "Enemy")
        {
            // 레이어를 원상복구시켜주세요 
        }
    }

    IEnumerator CenterToPlayer(GameObject playerObject)
    {
        while (!isDone)
        {
            Vector2 pos = Camera.main.WorldToViewportPoint(playerObject.transform.position);
            //vignette.center.value = pos;
            yield return null;
        }
    }

    //IEnumerator FadeOut(float time)
    //{
        //float intensity = vignette.intensity.value;

        //while (intensity > 0f)
        //{
           // vignette.intensity.value = intensity;
        //    intensity -= Time.deltaTime / time;
            //if (intensity <= 0f) vignette.intensity.value = 0;
        //    yield return null;
        //}
    //}

    IEnumerator DarkDebuff()
    {
        StartCoroutine(Timer(10));
        isDone = false;
        canTrigger = false;
        buffManager.DarkDebuffCount++;
        curCount = buffManager.DarkDebuffCount;
        GameObject playerObject = GameObject.Find("Player");
        StartCoroutine(CenterToPlayer(playerObject));
        PlayerEffectController effectController = playerObject.GetComponent<PlayerEffectController>();
        effectController.DarkDebuff = true;
        switch (buffManager.DarkDebuffCount)
        {
            case 1:
               // vignette.intensity.value = 0.55f;
                break;
            case 2:
               // vignette.intensity.value = 0.6f;
                break;
            case 3:
               // vignette.intensity.value = 0.63f;
                break;
        }
        yield return new WaitForSeconds(15 * buffManager.DarkDebuffCount - 1);
        if (canTrigger && curCount == buffManager.DarkDebuffCount)
        {
            //StartCoroutine(FadeOut(1));
            yield return new WaitForSeconds(1);
            isDone = true;
            if (isDone)
            {
                buffManager.DarkDebuffCount = 0;
                effectController.DarkDebuff = false;
            }
        }
    }
    IEnumerator Timer(int time)
    {
        yield return new WaitForSeconds(time);
        canTrigger = true;
    }
}