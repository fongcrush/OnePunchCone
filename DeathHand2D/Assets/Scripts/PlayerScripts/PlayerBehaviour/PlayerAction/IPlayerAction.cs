using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPlayerAction
{
    public void Begin();
    public void Update();
    public void End();
}
