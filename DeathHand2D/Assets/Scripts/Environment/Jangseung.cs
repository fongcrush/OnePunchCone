using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;

public class Jangseung : IEnvironment
{
    float Hp;
    float RespownTime;
    bool isAlive;

    private void Start()
    {
        Hp = EnvironmentData.EnvironmentTable["Jangseung"].durability;
        RespownTime = EnvironmentData.EnvironmentTable["Jangseung"].respawn;
        isAlive = true;
    }

    public override void Stay(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {

        }
        if (collision.gameObject.tag == "Enemy")
        {

        }
    }

    public override void Exit(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {

        }
        if (collision.gameObject.tag == "Enemy")
        {

        }
    }

    public void Hit(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(FadeOut(1));
            StartCoroutine(Respawn(EnvironmentData.EnvironmentTable["Jangseung"].respawn));
        }
    }

    IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);
        isAlive = true;
        GetComponent<BoxCollider2D>().enabled = true;
        Hp = EnvironmentData.EnvironmentTable["Jangseung"].durability;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1.0f);
    }

    IEnumerator FadeOut(float time)
    {
        float intensity = 1;

        while (intensity > 0f)
        {
            intensity -= Time.deltaTime / time;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, intensity);
            if (GetComponent<SpriteRenderer>().color.a <= 0f) new Color(1, 1, 0, 0);
            yield return null;
        }
    }
}
