using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarRewardController : MonoBehaviour, ICpListener
{
    private SimpleCarAgentController _agent;

    [SerializeField] private CheckpointManager _checkpointManager;
    
    // Config
    [SerializeField] private float _punishmentWall = -0.8f;
    [SerializeField] private float _rewardCheckpoint = 0.6f;

    [SerializeField] private bool _enableTimeout = false;
    [SerializeField] private float _timeoutSeconds = 60f;
    private float _timeoutTimer = 0f;

    private void Start()
    {
        _agent = GetComponent<SimpleCarAgentController>();
        _checkpointManager.AddListener(this);
    }

    private void Update()
    {
        //_agent.AddReward(-Time.deltaTime * 0.1f);
        
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

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Collision with wall");
            _agent.AddReward(_punishmentWall);
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
        _agent.AddReward(_rewardCheckpoint);
        if (isGoal)
        {
            print("is goal");
            _agent.AddReward(_rewardCheckpoint * 2);
            EndAgentEpisode();
        }
    }

    public void OnNotifyWrongCheckpoint()
    {
        print("wrong cp");
        _agent.AddReward(-_rewardCheckpoint);
    }

    private void EndAgentEpisode()
    {
        _checkpointManager.Reset();
        _agent.EndEpisode();
        _timeoutTimer = 0;
    }
}
