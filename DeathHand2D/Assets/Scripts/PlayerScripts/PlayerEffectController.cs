using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class PlayerEffectController : MonoBehaviour
{
    Player player;

    public GameObject DustEffect;
    bool canCreateDust;

    private void Awake()
    {
        canCreateDust = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMode == MoveMode.Run && canCreateDust == true)
        {
            StartCoroutine("CreateDustEffect");
        }
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
