using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //Resistance
    float Resistance
    {
        get
        {
            return ResistanceCurve.Evaluate(CurVeloticy.magnitude);
        }
    }


    public float enginePower = 100;

    float speedAmount = 0f;

    //Acceleration
    float acceleration
    {
        get
        {
            return Mathf.Max(0.1f, enginePower - (ShipMass + Resistance));
        }
    }


    float RotateSpeed = 720;
 

    //EngineState engineState;

    Rigidbody rig;

    public AnimationCurve ResistanceCurve = new AnimationCurve();

    //Ship properties
    public float ShipMass = 80f;

    public float ShipLength = 20f;




    //move
    Vector3 PrevPosition;

    public Vector3 CurVeloticy;

    Vector3 TargetVeloticy;

    Vector3 ActualVeloticy;



    Vector3 ForwardAmount;

    float TurnAmount;

    float ActualTurnAmount;

     public float maxRotateVelotocy=2;
 



    //control
    public bool speedUp = false;

    public bool turnLeft = false;

    public bool turnRight = false;


    void Start()
    {

        rig = GetComponent<Rigidbody>();
        
        PrevPosition = transform.position;
        ActualVeloticy = Vector3.zero;
        ForwardAmount = transform.forward;
        transform.position = new Vector3(transform.position.x, 0, transform.position.y);
    }

    private void FixedUpdate()
    {
        InputProcess();
        InterpolationVeloticy();
        MovementUpdate();
    }

   

    void InterpolationVeloticy()
    {
        ActualVeloticy = Vector3.Lerp(ActualVeloticy, TargetVeloticy, 0.005f);

        ActualTurnAmount = Mathf.Lerp(ActualTurnAmount, TurnAmount, 0.1f);
    }



    void MovementUpdate()
    {
        rig.velocity = Quaternion.AngleAxis(ActualTurnAmount, transform.up) * ActualVeloticy;


        if (rig.velocity.normalized != Vector3.zero)
        {
            transform.forward = rig.velocity.normalized;
        }

        CurVeloticy = transform.position - PrevPosition;
        PrevPosition = transform.position;
    }


    void InputProcess()
    {
        if (speedUp)
        {
            TargetVeloticy = ForwardAmount * (speedAmount + acceleration);
        }
        else
        {
            TargetVeloticy = ForwardAmount * (Mathf.Max(0, speedAmount - Resistance));
        }

        float rotateRate = Mathf.Clamp(CurVeloticy.magnitude / maxRotateVelotocy, 0f, 1f);
      
        float deg = RotateSpeed *rotateRate;
        
        if (turnLeft)
        {     
            TurnAmount -= deg * Time.deltaTime;
        }
        if (turnRight)
        {
            TurnAmount += deg * Time.deltaTime;
        }   
    }
}
