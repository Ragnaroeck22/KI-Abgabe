using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpListener
{
    // Obsolete. Leaving it in for now
    public void OnNotifyWrongCheckpoint();
    public void OnNotifyCorrectCheckpoint(bool isGoal);
}
