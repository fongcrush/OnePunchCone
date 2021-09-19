using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    public void Begin();
    public void UpdateAction();
    public void End();
}