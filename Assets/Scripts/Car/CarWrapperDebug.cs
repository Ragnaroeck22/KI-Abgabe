using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarWrapperDebug : MonoBehaviour
{
    private CarWrapper _wrapper;
    
    // Start is called before the first frame update
    void Start()
    {
        _wrapper = GetComponent<CarWrapper>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawMovementDirection();
        DrawForwardDirection();

        // Test wrapper funcs
        if (Input.GetKey(KeyCode.Keypad1))
        {
            _wrapper.SetSteeringAngle(-1f);
        }

        if (Input.GetKey(KeyCode.Keypad3))
        {
            _wrapper.SetSteeringAngle(1f);
        }

        if (Input.GetKey(KeyCode.Keypad2))
        {
            _wrapper.SetThrottle(0.5f);
        }

        if (Input.GetKey(KeyCode.Keypad5))
        {
            _wrapper.SetThrottle(1f);
        }
        
    }

    private void DrawMovementDirection()
    {
        Vector3 md = _wrapper.GetMovementDirection();
     
        // Determine line color
        float angle = _wrapper.GetAngleMovementToForward();
        Color lineColor = Color.yellow;
        //print(angle);
        if (angle >= 30)
        {
            lineColor = Color.green;
        }
        if (angle >= 100)
        {
            lineColor = Color.red;
        }

        Debug.DrawLine(transform.position,  transform.position + md, lineColor, Time.deltaTime);
    }

    private void DrawForwardDirection()
    {
        Vector3 fwd = _wrapper.GetForwardDirection();
        Debug.DrawLine(transform.position,  transform.position + fwd * 10, Color.magenta, Time.deltaTime);
    }
}