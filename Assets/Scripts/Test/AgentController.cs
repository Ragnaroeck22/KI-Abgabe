using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentController : Agent
{
    [SerializeField] private Transform target;
    
    
    
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0f, 0.03f, 0f);

        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                target.localPosition = new Vector3(-0.4f, 0.03f, 0);
                break;
            
            case 1:
                target.localPosition = new Vector3(0.4f, 0.03f, 0);
                break;
        }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
        float moveSpeed = 0.15f;

        transform.localPosition += new Vector3(move, 0f, 0f) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Pellet")
        {
            AddReward(10f);
            EndEpisode();
        }
        if (other.gameObject.tag == "Wall")
        {
            AddReward(-5f);
            EndEpisode();
        }
    }
}
