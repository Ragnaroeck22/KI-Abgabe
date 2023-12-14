using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SimpleCarAgentController : Agent
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    
    private Vector3 _rbStartPosition;
    private Quaternion _rbStartRotation;
    
    [SerializeField] private bool _allowRandomRotation = false;
    //[SerializeField] private float _maximumRotationDeviation = 15f;

    private Rigidbody _rigidbody;

    [SerializeField] private float _turnRate = 40f;
    [SerializeField] private float _acceleration = 30f;
    [SerializeField] private float _brakePower = 0.5f;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        _rbStartPosition = _rigidbody.position;
        _rbStartRotation = _rigidbody.rotation;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        _rigidbody.position = _rbStartPosition;
        _rigidbody.rotation = _rbStartRotation;
        _rigidbody.velocity = Vector3.zero;
        //transform.SetLocalPositionAndRotation(_startPosition, _startRotation);

        // Deviate from start rotation here
        if (_allowRandomRotation)
        {

        }

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Movement direction
        sensor.AddObservation(_rigidbody.velocity);
        // Speed
        sensor.AddObservation(_rigidbody.velocity.magnitude);
        // Forward direction
        sensor.AddObservation(transform.forward);
        // Angular velocity
        sensor.AddObservation(_rigidbody.angularVelocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var valSteer = actions.DiscreteActions[0];
        var valAccelerate = actions.DiscreteActions[1];
        var valBrake = actions.DiscreteActions[2];

        // Implement Discrete actions
        switch (valSteer)
        {
            case 1:
                _rigidbody.AddTorque(0, -_turnRate, 0);
                break;
            case 2:
                _rigidbody.AddTorque(0, _turnRate, 0);
                break;
        }
        
        switch (valAccelerate)
        {
            case 1:
                _rigidbody.AddForce(transform.forward * _acceleration, ForceMode.Acceleration);
                break;
        }
        
        switch (valBrake)
        {
            case 1:
                _rigidbody.AddForce(-_rigidbody.velocity * _brakePower, ForceMode.Acceleration);
                break;
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0;

        switch (Input.GetAxisRaw("SteerDpad"))
        {
            case -1:
                discreteActions[0] = 1;
                break;
            case 1:
                discreteActions[0] = 2;
                break;
        }

        // Acceleration / braking
        discreteActions[1] = 0;
        discreteActions[2] = 0;
        if (Input.GetButton("Upshift"))
        {
            discreteActions[1] = 1;
        }
        
        else if (Input.GetButton("Downshift"))
        {
            discreteActions[2] = 1;
        }
    }
}
