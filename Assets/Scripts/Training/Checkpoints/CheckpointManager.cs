using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour, ICpListener
{
    private List<Checkpoint> _checkpoints; 

    // Start is called before the first frame update
    void Start()
    {
        PopulateCheckpoints();   
    }

    public void OnNotify(Checkpoint notifier)
    {
        if (!_checkpoints.Contains(notifier))
            return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateCheckpoints()
    {
        foreach (Transform checkpoint in transform)
        {
            _checkpoints.Add(checkpoint.GetComponent<Checkpoint>());
        }
    }
}
