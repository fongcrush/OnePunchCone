 using UnityEngine;

//[CreateAssetMenu(fileName = "StatesData", menuName = "Scriptable Object/Player/States", order = 1)]
public class PlayerStatesData
{
    public static PlayerState playerState = PlayerState.Move;

    public static MoveMode moveMode = MoveMode.Idle;

    public static ActionState actionState = ActionState.None;

    public static AttackState attackState = AttackState.None;
}

public enum PlayerState
{
    Move,
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
    None,
    LeftShift,
    Z,
    X,
    C,
}

public enum AttackState
{
    None,
    AAttack,
    Skill1,
    Skill2,
    Skill3
}

public enum ActionState
{
    None,
    AAttack,
    Skill1,
    Skill2,
    Skill3,
    Dash
}