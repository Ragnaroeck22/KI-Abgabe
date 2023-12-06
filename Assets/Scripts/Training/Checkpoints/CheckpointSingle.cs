using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private CheckpointManager _checkpointManager;
    
    private void OnTriggerEnter(Collider other)
    {
        CarRewardController carRewardController = other.GetComponentInParent<CarRewardController>();
        if (carRewardController == null)
            return;
        
        _checkpointManager.CheckpointReached(this);
    }

    public void SetCheckpointManager(CheckpointManager checkpointManager)
    {
        _checkpointManager = checkpointManager;
    }
}
