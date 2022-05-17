using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    ShipController controller;




    void Start()
    {
        controller = GetComponent<ShipController>();
    }


    void Update()
    {

    }

    public void BeHit(int damage)
    {

    }
}
