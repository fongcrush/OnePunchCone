using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class InputManager : MonoBehaviour
{
    [HideInInspector]
    public static float hAxis, vAxis;

    private void Awake()
	{
        hAxis = vAxis = 0;
    }

	private void Update()
    {
        if(playerState != PlayerState.Dead)
        {
            if(actionState == ActionState.None)
                UpdateMoveInput();
            UpdateActionInput();
        }
    }

    void UpdateMoveInput()
	{
        // 이동
        if(Input.GetKey(KeyCode.LeftArrow))
            curArrowKey = ArrowKey.Left;

        else if(Input.GetKey(KeyCode.RightArrow))
            curArrowKey = ArrowKey.Right;

        else if(Input.GetKey(KeyCode.UpArrow))
            curArrowKey = ArrowKey.Up;

        else if(Input.GetKey(KeyCode.DownArrow))
            curArrowKey = ArrowKey.Down;

        else
            curArrowKey = ArrowKey.None;

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
    }

    void UpdateActionInput()
    {
        // 액션
        if(Input.GetKeyDown(KeyCode.Z)) { curActionKey = ActionKey.Z; actionState = ActionState.Ready; playerState = PlayerState.Action; }

        if(Input.GetKeyDown(KeyCode.X)) { curActionKey = ActionKey.X; actionState = ActionState.Ready; playerState = PlayerState.Action; }

        if(Input.GetKeyDown(KeyCode.C)) { curActionKey = ActionKey.C; actionState = ActionState.Ready; playerState = PlayerState.Action; }

        if(Input.GetKeyDown(KeyCode.LeftShift)) { curActionKey = ActionKey.LeftShift; actionState = ActionState.Ready; playerState = PlayerState.Action; }
    }
}
