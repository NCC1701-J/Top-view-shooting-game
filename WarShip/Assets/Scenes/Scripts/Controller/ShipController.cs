using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    bool IsDead = false;


    protected Move movement;


    protected virtual void Start()
    {
        movement = transform.GetComponent<Move>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (IsDead) { return; }
        ControlUpdate();
    }

    protected virtual void ControlUpdate()
    {

    }
}
