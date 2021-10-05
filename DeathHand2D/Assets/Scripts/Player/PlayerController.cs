using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using static PlayerStatesData;
using static InputManager;

public class PlayerController : MonoBehaviour
{
    private InputManager inputController;

    private BoxCollider2D boxColl;

    private PlayerActionMgr actionMgr;
    public PlayerActionMgr ActionMgr { get { return actionMgr; } }

    private PlayerMove move;

    [SerializeField]
    private int dashCount;
    public int DashCount { get { return dashCount; } set { dashCount = value; } }

    [SerializeField]
    private bool dashGodMode;
    public bool DashGodMode { get { return dashGodMode; } set { dashGodMode = value; } }

    private bool inBush = false;
    public bool InBush { get { return inBush; } }
    public void Bush(bool tf) { inBush = tf; }

    public PlayerState displayPlayerState;
    public CharacterDirection displayCharacterDir;
    public ArrowKey displayCurArrowKey;
    public ActionKey displayCurActionKey;
    public MoveMode displayCurMoveMode;
    public ActionState displayActionState;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        actionMgr = transform.Find("ActionManager").GetComponent<PlayerActionMgr>();
        boxColl = GetComponent<BoxCollider2D>();
        move = GetComponent<PlayerMove>();
        characterDirection = CharacterDirection.Right;
        dashCount = 2;
        dashGodMode = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
	{
        switch(playerState)
        {
        case PlayerState.Move:
            move.UpdateMove();
            break;
        case PlayerState.Action:
            actionMgr.UpdateAction();
            break;

        case PlayerState.Dead:

            break;
        }
		UpdateDisplayStates();
    }

	private void Update()
    {
        //Debug.Log("boxColl : " + boxColl.bounds);
        switch(playerState)
        {
        case PlayerState.Move:
            move.MoveCheck();
            break;
        case PlayerState.Action:
            break;

        case PlayerState.Dead:

            break;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Arrow")
        {
            Destroy(collision.gameObject);
        }
    }

    private void UpdateDisplayStates()
    {
        displayPlayerState = playerState;
        displayActionState = actionState;
        displayCharacterDir = characterDirection;
        displayCurArrowKey = curArrowKey;
        displayCurActionKey = curActionKey;
        displayCurMoveMode = moveMode;
    }

    public bool LeftOrRight()
    {
        if(characterDirection == CharacterDirection.Left)
            return true;
        else
            return false;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.transform.tag == "Enemy") 
    //    {
    //        if (dashGodMode == false) 
    //        {
    //            //Hp -= 0.5f;
    //        }
    //    }
    //}
}