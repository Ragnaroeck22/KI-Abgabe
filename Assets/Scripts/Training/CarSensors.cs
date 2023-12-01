using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarSensors : MonoBehaviour
{
    public Transform _car;
    
    // Update is called once per frame
    void Update()
    {
        // Follow car
        transform.SetLocalPositionAndRotation(_car.position, _car.rotation);
    }
}
