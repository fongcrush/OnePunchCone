using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using static PlayerStatesData;
using static InputManager;

public class PlayerController : MonoBehaviour
{
    public float PlayerMaxHP;

    private InputManager inputController;

    private BoxCollider2D boxColl;

    private PlayerActionMgr actionMgr;
    public PlayerActionMgr ActionMgr { get { return actionMgr; } }

    private PlayerMove move;

    [SerializeField]
    private int dashCount;
    public int DashCount { get { return dashCount; } set { dashCount = value; } }

    [HideInInspector]
    public bool isTiming;

    [SerializeField]
    private bool dashGodMode = false;
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
        actionMgr = transform.Find("ActionManager").GetComponent<PlayerActionMgr>();
        boxColl = GetComponent<BoxCollider2D>();
        move = GetComponent<PlayerMove>();
        characterDirection = CharacterDirection.Right;
        dashCount = 2;
        isTiming = false;
    }

    // Update is called once per frame
    private void Update()
	{
        switch(playerState)
        {
        case PlayerState.Move:
            if(!move.enabled)
                move.enabled = true;
            break;

        case PlayerState.Action:
                actionMgr.UpdateAction();
            break;

        case PlayerState.Dead:

            break;
        }
        UpdateDisplayStates();
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Arrow")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PerfectTiming")
        {
            isTiming = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PerfectTiming")
        {
            isTiming = false;
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
}