using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacherController : ShipController
{
    public Camera cam;

    protected override void ControlUpdate()
    {
        movement.speedUp = Input.GetKey(KeyCode.W);
        movement.turnLeft = Input.GetKey(KeyCode.A);
        movement.turnRight = Input.GetKey(KeyCode.D);
    }
}
