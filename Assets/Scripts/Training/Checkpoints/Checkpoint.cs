using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Checkpoint _nextCheckpoint;
    [SerializeField] private Checkpoint _previousCheckpoint;

    public void CheckpointReached()
    {
        // Essentially if this checkpoint is the goal
        if (_nextCheckpoint == null)
        {
            if (!gameObject.CompareTag("Goal"))
            {
                Debug.LogWarning($"Warning: Last checkpoint named {gameObject.name} is not properly tagged as Goal");
            }
            
            if (_previousCheckpoint == null)
            {
                Debug.LogWarning($"Warning: Previous Checkpoint of {gameObject.name} is not properly assigned");
                return;
            }
            
            ResetCallback();
            return;
        }
        _nextCheckpoint.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ResetCallback()
    {
        // Essentially if this checkpoint is the first checkpoint
        if (_previousCheckpoint == null)
        {
            gameObject.SetActive(true);
            return;
        }
        
        _previousCheckpoint.ResetCallback();
        gameObject.SetActive(false);
    }
}
