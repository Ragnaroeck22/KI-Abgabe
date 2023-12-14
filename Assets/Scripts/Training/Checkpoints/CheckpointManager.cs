using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour, ICpNotifier
{
    private List<CheckpointSingle> _checkpoints;
    private int _nextCheckpointIndex = 0;

    private List<ICpListener> _listeners = new List<ICpListener>();
    
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

    public void AddListener(ICpListener listener)
    {
        _listeners.Add(listener);
    }

    public void NotifyCorrectCheckpoint(bool isGoal)
    {
        foreach (var listener in _listeners)
        {

            listener.OnNotifyCorrectCheckpoint(isGoal);
        }
    }

    public void NotifyWrongCheckpoint()
    {
        foreach (var listener in _listeners)
        {
            listener.OnNotifyWrongCheckpoint();
        }
    }


    public void CheckpointReached(CheckpointSingle checkpoint)
    {
        if (_checkpoints.IndexOf(checkpoint) == _nextCheckpointIndex)
        {
            _nextCheckpointIndex = (_nextCheckpointIndex + 1) % _checkpoints.Count;
            checkpoint.gameObject.SetActive(false);
            NotifyCorrectCheckpoint(checkpoint.CompareTag("Goal"));
        }
        else
        {
            NotifyWrongCheckpoint();
        }
    }

    public void Reset()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        _nextCheckpointIndex = 0;
    }
}
