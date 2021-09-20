using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class PlayerEffectController : MonoBehaviour
{
    PlayerController player;

    public GameObject DustEffect;
    bool canCreateDust;

    public GameObject DarkDebuffEffect;
    bool darkDebuff;

    public bool DarkDebuff { set { darkDebuff = value; } }

    private void Awake()
    {
        canCreateDust = true;
        darkDebuff = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMode == MoveMode.Run && canCreateDust == true)
        {
            StartCoroutine("CreateDustEffect");
        }
        DarkDebuffEffect.SetActive(darkDebuff);

    }

    IEnumerator CreateDustEffect()
    {        
        GameObject dust = Instantiate(DustEffect, transform.position, Quaternion.identity);
        canCreateDust = false;
        yield return new WaitForSeconds(1.0f);
        Destroy(dust);
        canCreateDust = true;
    }
}
