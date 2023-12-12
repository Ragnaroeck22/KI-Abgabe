using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarRewardController : MonoBehaviour, ICpListener
{
    private CarAgentController _agent;
    private CarWrapper _wrapper;

    [SerializeField] private CheckpointManager _checkpointManager;
    
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
    
    //[SerializeField] private CheckpointSingle _goal;

    private float _lowestDistanceToGoal;
    
    private void Start()
    {
        _agent = GetComponent<CarAgentController>();
        _wrapper = GetComponent<CarWrapper>();
        
        _checkpointManager.AddListener(this);
    }

    private void Update()
    {

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
        
        /*
        if (driftAngle >= 100)
        {
            if (driftAngle >= 170)
                _agent.AddReward(_punishmentSpinOut);
            else
                _agent.AddReward(_punishmentHighDriftAngle);
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Collision with wall");
            _agent.AddReward(_punishmentWall);
            //EndAgentEpisode();
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Wall"))
        {
            _agent.AddReward(_punishmentWall * 0.2f);
        }
    }

    public void OnNotifyCorrectCheckpoint(bool isGoal)
    {
        print("correct cp");
        _agent.AddReward(1f);
        if (isGoal)
        {
            print("is goal");
            EndAgentEpisode();
        }
            
    }

    public void OnNotifyWrongCheckpoint()
    {
        print("wrong cp");
        _agent.AddReward(-1f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            //Debug.Log("Checkpoint reached");
            //_agent.AddReward(_rewardCheckpoint);
            //other.GetComponent<Checkpoint>().CheckpointReached();
        }

        if (other.CompareTag("Goal"))
        {
            //Debug.Log("Goal reached");
            //_agent.AddReward(_rewardGoal);
            //other.GetComponent<Checkpoint>().CheckpointReached();
            //EndAgentEpisode();
        }
    }

    private void EndAgentEpisode()
    {
        _checkpointManager.Reset();
        _agent.EndEpisode();
        //_goal.ResetCallback();
        _timeoutTimer = 0;
    }
    
}
