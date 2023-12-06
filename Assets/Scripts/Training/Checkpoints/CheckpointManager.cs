using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public event EventHandler OnCorrectCheckpoint;
    public event EventHandler OnWrongCheckpoint;
    
    private List<CheckpointSingle> _checkpoints;
    private int _nextCheckpointIndex = 0;
    
    void Awake()
    {
        _checkpoints = new List<CheckpointSingle>();
        foreach (Transform checkpoint in transform)
        {
            CheckpointSingle checkpointSingle = checkpoint.GetComponent<CheckpointSingle>();
            checkpointSingle.SetCheckpointManager(this);
            _checkpoints.Add(checkpointSingle);
        }
    }


    public void CheckpointReached(CheckpointSingle checkpoint)
    {
        if (_checkpoints.IndexOf(checkpoint) == _nextCheckpointIndex)
        {
            _nextCheckpointIndex = (_nextCheckpointIndex + 1) % _checkpoints.Count;
            OnCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnWrongCheckpoint?.Invoke(this, EventArgs.Empty);
        }
    }
}
