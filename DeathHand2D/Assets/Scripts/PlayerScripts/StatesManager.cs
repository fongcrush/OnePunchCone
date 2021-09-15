using UnityEngine;

public class StatesManager
{
    public static PlayerState playerState;
    
    public static CharacterDirection characterDirection;
    
    public static ArrowKey currentArrowKey;

    public static ActionKey currentActionKey;
    
    public static MoveMode moveMode;

    public static ActionState actionState;

    public enum PlayerState
    {
        Normal,
        Action,
		Dead
	}

	public enum MoveMode
	{
        Idle,
        Walk,
        Run
	}

	public enum CharacterDirection
    {
        Left,
        Right
    }

    public enum ArrowKey
    {
        None,
        Left,
        Right,
        Up,
		Down
	}

    public enum ActionKey
    {
        Z,
        X,
        C
    }

	public enum ActionState
	{
		Attack,
		Dash
	}
    public static bool LeftOrRight()
    {
        if(characterDirection == CharacterDirection.Left)
            return true;
        else
            return false;
    }
}
