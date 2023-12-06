using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarRewardController : MonoBehaviour
{
    private CarAgentController _agent;
    private CarWrapper _wrapper;
    
    // Config
    [SerializeField] private float _punishmentWall = -0.8f;
    [SerializeField] private float _punishmentHighDriftAngle = -0.2f;
    [SerializeField] private float _punishmentSpinOut = -0.7f;
    [SerializeField] private float _rewardCheckpoint = 0.6f;
    [SerializeField] private float _rewardGoal = 0.8f;
    [SerializeField] private float _rewardVelocityModifier = 0.1f;

    
    [SerializeField] private bool _enableTimeout = false;
    [SerializeField] private float _timeoutSeconds = 60f;
    private float _timeoutTimer = 0f;
    
    [SerializeField] private Checkpoint _goal;

    private float _lowestDistanceToGoal;
    
    private void Start()
    {
        _agent = GetComponent<CarAgentController>();
        _wrapper = GetComponent<CarWrapper>();
    }

    private void Update()
    {
        float distanceToGoal = Vector3.Distance(transform.position, _goal.transform.position);
        if (distanceToGoal < _lowestDistanceToGoal)
        {
            //Debug.Log($"New best Distance To Goal: {distanceToGoal}");
            _agent.AddReward(_lowestDistanceToGoal - distanceToGoal);
            _lowestDistanceToGoal = distanceToGoal;
        }
        
        //_agent.AddReward(-distanceToGoal * 0.05f);
        
        if (!_enableTimeout)
            return;
        
        if (_timeoutTimer > _timeoutSeconds)
        {
            Debug.Log("Timeout");
            _timeoutTimer = 0;
            EndAgentEpisode();
            return;
        }
        
        _timeoutTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //_agent.AddReward(_wrapper.GetVelocity() * _rewardVelocityModifier);
        
        // Evaluate drift angle
        if (_wrapper.GetVelocity() < 1)
            return;

        float driftAngle = _wrapper.GetAngleMovementToForward();
        
        // Temporary: punish light drifting as well! (value was 100 instead of 30)
        if (driftAngle >= 15)
        {
            if (driftAngle >= 170)
                _agent.AddReward(_punishmentSpinOut);
            else
                _agent.AddReward(_punishmentHighDriftAngle);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Collision with wall");
            _agent.AddReward(_punishmentWall);
            EndAgentEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Debug.Log("Checkpoint reached");
            _agent.AddReward(_rewardCheckpoint);
            other.GetComponent<Checkpoint>().CheckpointReached();
        }

        if (other.CompareTag("Goal"))
        {
            Debug.Log("Goal reached");
            _agent.AddReward(_rewardGoal);
            other.GetComponent<Checkpoint>().CheckpointReached();
            EndAgentEpisode();
        }
    }

    private void EndAgentEpisode()
    {
        _agent.EndEpisode();
        _goal.ResetCallback();
        _timeoutTimer = 0;
    }

    public void Reset()
    {
        _lowestDistanceToGoal = Vector3.Distance(transform.position, _goal.transform.position);
    }
}
