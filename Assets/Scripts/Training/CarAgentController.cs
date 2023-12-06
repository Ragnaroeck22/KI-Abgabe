using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using VehiclePhysics;
using Random = UnityEngine.Random;

public class CarAgentController : Agent
{
    // Config
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Quaternion _startRotation;
    [SerializeField] private bool _allowRandomRotation = false;
    //[SerializeField] private float _maximumRotationDeviation = 15f;
    
    [SerializeField] private CarWrapper _wrapper;

    private VPResetVehicle _resetter;
    
    
    // For more responsive heuristics controls because the checks are made between frame updates 
    private bool _hasShifted = false; 
    
    private void Start()
    {
        _resetter = GetComponent<VPResetVehicle>();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    public override void OnEpisodeBegin()
    {
        _resetter.DoReset();
        _wrapper.Reset(_startPosition);
        transform.SetLocalPositionAndRotation(_startPosition, _startRotation);
        
        GetComponent<CarRewardController>().Reset();
        
        // Deviate from start rotation here
        if (_allowRandomRotation)
        {
            
        }
        
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
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Throttle");
        continuousActions[2] = Input.GetAxisRaw("Brake");

        if (Input.GetButton("Handbrake"))
        {
            continuousActions[3] = 1f;
        }
        else
        {
            continuousActions[3] = 0;
        }
        
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if (Input.GetButton("Upshift"))
        {
            if (!_hasShifted)
            {
                _hasShifted = true;
                print("Upshift");
                discreteActions[0] = 2;
            }
        }

        else if (Input.GetButton("Downshift"))
        {
            if (!_hasShifted)
            {
                _hasShifted = true;
                print("Downshift");
                discreteActions[0] = 1;
            }
        }
        else
        {
            _hasShifted = false;
            discreteActions[0] = 0;
        }

    }
    
}
