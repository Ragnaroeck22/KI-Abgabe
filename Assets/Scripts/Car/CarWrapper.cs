using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class CarWrapper : MonoBehaviour
{
    //https://vehiclephysics.com/advanced/databus-reference/#data-channels
    
    private VPVehicleController _vehicleController;
    private Rigidbody _rigidbody;

    [SerializeField] private float _physicsDelay = 0.01f; // To avoid nasty physics shenanigans
    
    // Start is called before the first frame update
    void Start()
    {
        _vehicleController = GetComponent<VPVehicleController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // === Observations ===
    public Vector3 GetMovementDirection()
    {
        return _rigidbody.velocity;
    }

    public float GetVelocity()
    {
        return _rigidbody.velocity.magnitude;
    }

    public Vector3 GetForwardDirection()
    {
        return transform.forward;
    }

    public float GetAngleMovementToForward()
    {
        return Vector3.Angle(GetMovementDirection(), GetForwardDirection());
    }
    
    public Vector3 GetAngularVelocity()
    {
        return _rigidbody.angularVelocity;
    }
    
    public float GetEngineRpm()
    {
        return _vehicleController.data.Get(Channel.Vehicle, VehicleData.EngineRpm) / 1000.0f;
    }

    public int GetCurrentGear()
    {
        return _vehicleController.data.Get(Channel.Vehicle, VehicleData.GearboxGear);
    }
    
    // Get if engine is stalled??
    
    // === Actions ===
    public void SetThrottle(float value)
    {
        value = (value + 1) * 0.5f; // Format for -1/+1 instead of 0/+1
        _vehicleController.data.Set(Channel.Input, InputData.Throttle, (int)(value * 10000f));
    }

    public void SetBrakes(float value)
    {
        //value = (value + 1) * 0.5f; // Format for -1/+1 instead of 0/+1
        if (value < 0)
            return;
        _vehicleController.data.Set(Channel.Input, InputData.Brake, (int)(value * 10000f));
    }

    private void SetGear(int value)
    {
        _vehicleController.data.Set(Channel.Input, InputData.ManualGear, value);
    }
    
    public void SetGearUp()
    {
        _vehicleController.data.Set(Channel.Input, InputData.GearShift, 1);
    }

    public void SetGearDown()
    {
        _vehicleController.data.Set(Channel.Input, InputData.GearShift, -1);
    }
    
    // Clutch??

    public void SetSteeringAngle(float value)
    {
        _vehicleController.data.Set(Channel.Input, InputData.Steer, (int)(value * 10000f));
    }

    public void SetHandbrake(float value)
    {
        if (value < 0)
            return;
        //value = (value + 1) * 0.5f; // Format for -1/+1 instead of 0/+1
        _vehicleController.data.Set(Channel.Input, InputData.Handbrake, (int)(value * 10000f));
    }
    
    // Start / Stop engine??
    
    
    // RB reset funcs
    public void Reset(Vector3 resetPosition)
    {
        _rigidbody.isKinematic = true;
        _rigidbody.position = resetPosition;
        SetGear(1);
        StartCoroutine(WaitForStupidPhysicsToCalmDown());
    }

    private IEnumerator WaitForStupidPhysicsToCalmDown()
    {
        yield return new WaitForSeconds(_physicsDelay);
        _rigidbody.isKinematic = false;
    }
}
