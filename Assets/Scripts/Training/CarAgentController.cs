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
    // Config
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Quaternion _startRotation;
    [SerializeField] private bool _allowRandomRotation = false;
    
    
    [SerializeField] private CarWrapper _wrapper;

    public override void OnEpisodeBegin()
    {
        transform.SetLocalPositionAndRotation(_startPosition, _startRotation);
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
        // Assign Continuous actions
        var valSteering = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var valThrottle = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        var valBrake = Mathf.Clamp(actions.ContinuousActions[2], -1f, 1f);
        var valHandbrake = Mathf.Clamp(actions.ContinuousActions[3], -1f, 1f);
      
        // Assign Discrete actions
        var valGearshift = actions.DiscreteActions[0];

        
        // Implement Continuous actions
        _wrapper.SetSteeringAngle(valSteering);
        _wrapper.SetThrottle(valThrottle);
        _wrapper.SetBrakes(valBrake);
        _wrapper.SetHandbrake(valHandbrake);
        
        // Implement Discrete actions
        switch (valGearshift)
        {
            case 1:
                _wrapper.SetGearDown();
                break;
            case 2:
                _wrapper.SetGearUp();
                break;
        }
    }
}
