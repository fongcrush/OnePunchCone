using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonScript : MonoBehaviour
{
    float BombTimer = 0f;

    bool isAttack = false;

    SpriteRenderer summonMesh;
    Enemy summonCreature;

    public GameObject AttackColl;
    public GameObject WarningBox;

    Color c;
    // Start is called before the first frame update
    void Start()
    {
        summonCreature = GetComponent<Enemy>();

        summonMesh = GetComponent<SpriteRenderer>();
        c = summonMesh.material.color;

        summonCreature.stat.MaxHP = 200;
        summonCreature.stat.Power = 80;
    }
    IEnumerator Attack()
    {
        isAttack = true;

        WarningBox.SetActive(false);
        AttackColl.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        AttackColl.SetActive(false);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        BombTimer += Time.deltaTime;

        c = new Color(c.r, c.g, c.b + 0.01f, c.a);
        summonMesh.material.color = c;

        if (BombTimer > 2f || summonCreature.stat.GetHP() < 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }
}
