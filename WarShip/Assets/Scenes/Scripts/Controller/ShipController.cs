using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{   
   protected Move movement;

    [SerializeField]
    protected  TurretsContorl[] Turrets;

   protected virtual void Start()
    {
        movement = transform.GetComponent<Move>();
        Turrets = transform.GetComponentsInChildren<TurretsContorl>(true);
    }

    protected virtual void Update()
    {
       
    }

    protected virtual void FixedUpdate()
    {
        ControlUpdate();
    }

    protected virtual void ControlUpdate()
    {

    }
}
