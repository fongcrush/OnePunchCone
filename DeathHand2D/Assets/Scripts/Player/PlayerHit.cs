using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;
using static PlayerStatesData;

public class PlayerHit : MonoBehaviour
{
    private Animator anim;
    private PlayerMove move;
    public bool superArmorOn;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        move = GetComponent<PlayerMove>();
        superArmorOn = false;
    }

    // Update is called once per frame
    void Update()
	{
        if(playerState == PlayerState.Action || playerState == PlayerState.Hit)
            superArmorOn = true;
        else
            superArmorOn = false;
    }

	public void Hit(float damage)
	{
        if(playerState == PlayerState.Hit || playerState == PlayerState.Dead) return;

        GM.pcStat.ChangeHP(damage);

        if(superArmorOn) return;

        StartCoroutine(StartHit());
	}

    public void AirBorne(float damage)
	{
        if(playerState == PlayerState.Hit|| playerState == PlayerState.Dead) return;

        GM.pcStat.ChangeHP(damage);

        if(superArmorOn) return;

        StartCoroutine(StartAirBorne());
    }
    
    private IEnumerator StartAirBorne()
    {
        playerState = PlayerState.Hit;
        anim.SetTrigger("AirBorne");
        yield return new WaitForSeconds(3.0f);
	}

    private IEnumerator StartHit()
    {
        playerState = PlayerState.Hit;
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(2.0f);
        playerState = PlayerState.None;
    }
    

    
}
