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

    private void Start()
    {
        _agent = GetComponent<CarAgentController>();
        Reset();
    }

    private void FixedUpdate()
    {
        _agent.AddReward(_wrapper.GetVelocity() * _rewardVelocityModifier);
        
        // Evaluate drift angle
        if (_wrapper.GetVelocity() < 1)
            return;

        float driftAngle = _wrapper.GetAngleMovementToForward();
        if (driftAngle >= 100)
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
            _agent.AddReward(_punishmentWall);
            _agent.EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            _agent.AddReward(_rewardCheckpoint);
            Checkpoint cp = other.GetComponent<Checkpoint>();
            _agent.AddCheckpoint(cp);
            cp.gameObject.SetActive(false);
        }

        if (other.CompareTag("Goal"))
        {
            _agent.AddReward(_rewardGoal);
            _agent.EndEpisode();
        }
    }

    public void Reset()
    {
        _wrapper = GetComponentInChildren<CarWrapper>();
    }
}
