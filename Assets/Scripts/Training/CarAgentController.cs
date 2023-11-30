using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarAgentController : Agent
{
    [SerializeField] private CarWrapper _wrapper;

    public override void OnEpisodeBegin()
    {
        
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_wrapper.GetMovementDirection());
        sensor.AddObservation(_wrapper.GetVelocity());
        sensor.AddObservation(_wrapper.GetForwardDirection());
        sensor.AddObservation(_wrapper.GetAngleMovementToForward());
        sensor.AddObservation(_wrapper.GetEngineRpm());
        sensor.AddObservation(_wrapper.GetCurrentGear());
        sensor.AddObservation(_wrapper.GetAngularVelocity());
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-5f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            
        }

        if (other.CompareTag("Goal"))
        {
            
        }
    }
}
