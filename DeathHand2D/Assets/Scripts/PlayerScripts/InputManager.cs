using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class InputManager : MonoBehaviour
{
    public static float hAxis, vAxis;

    static void UpdateInput()
	{
        // 이동
        if(Input.GetKey(KeyCode.LeftArrow))
            currentArrowKey = CurrentArrowKey.Left;

        if(Input.GetKey(KeyCode.RightArrow))
            currentArrowKey = CurrentArrowKey.Right;

        if(Input.GetKey(KeyCode.UpArrow))
            currentArrowKey = CurrentArrowKey.Up;

        if(Input.GetKey(KeyCode.DownArrow))
            currentArrowKey = CurrentArrowKey.Down;

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // 액션
        if(Input.GetKeyDown(KeyCode.Z))
            currentActionKey = CurrentActionKey.Z;

        if(Input.GetKeyDown(KeyCode.X))
            currentActionKey = CurrentActionKey.X;

        if(Input.GetKeyDown(KeyCode.C))
            currentActionKey = CurrentActionKey.C;

    }

	private void Update()
	{
        UpdateInput();
	}
}
