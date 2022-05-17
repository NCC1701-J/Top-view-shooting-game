﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class AIController : ShipController
{

    [SerializeField]
    List <List<Emitter>> weapons;

    Rigidbody tarRig;
    

    //Range of Search enemy
    public float radiusForSearchTarget;

    //Target distance margin
    public float stopDistence;

    //Target angle margin
    public float stopDegrees;

    //Target
    GameObject target = null;


    protected override void Start()
    {
        base.Start();

        weapons = new List<List<Emitter>>();
        if(Turrets!=null)
        {
            for (int i = 0; i < Turrets.Length; i++)
            {
                List<Emitter> w = new List<Emitter>();
                Emitter[] _w = Turrets[i].GetComponentsInChildren<Emitter>();
                w.AddRange(_w);
                weapons.Add(w);
                //Debug.Log(weapons[i].Count);
            }
        }
       
       
     
         
    }



    protected override void ControlUpdate()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radiusForSearchTarget, 1 << LayerMask.NameToLayer("Player"));

        if(col.Length>0)
        target = col[0].gameObject ?? null;
        if(target)
        {
            tarRig = target.GetComponentInParent<Rigidbody>();
        }
        
        AIControl(target);
    }

    Vector3 GetFixedPos(GameObject tar)
    {
      return  new Vector3(tar.transform.position.x, transform.position.y, tar.transform.position.z);
    }

    //AI fire control calculation
    public Vector3 FiringLeadCalculate(GameObject tar)
    {
        if(!tar)
        {
            return Vector3.zero;
        }
        Vector3 EstimationPos=Vector3.zero;

        Vector3 tarVelocity = tar.GetComponentInParent<Rigidbody>().velocity;


        return EstimationPos;
    }


    void AI_MoveLogic(Vector3 targetPos_fixed)
    {
        //move_to
        float dis = Vector3.Distance(targetPos_fixed, transform.position);

        if (dis > stopDistence)
        {
            movement.speedUp = true;
        }
        else
        {
            movement.speedUp = false;
        }

        //rotate_to
        Vector3 tarDir = (targetPos_fixed - transform.position).normalized;

        bool tarIsRight = Vector3.Cross(transform.forward, tarDir).y > 0;

        float angle = Vector3.Angle(transform.forward, tarDir);
        if (angle > stopDegrees)
        {
            if (tarIsRight)
            {
                movement.turnRight = true;
                movement.turnLeft = false;
            }
            else
            {
                movement.turnRight = false;
                movement.turnLeft = true;
            }
        }
        else
        {
            movement.turnRight = false;
            movement.turnRight = false;
        }
    }


   

    void AI_AimLogic(GameObject target)
    {
        for(int i=0;i<Turrets.Length;i++)
        {
            if(weapons[i].Count>0)
            {
                for (int j = 0; j < weapons[i].Count; j++)
                {

                    Artillery a = weapons[i][j] as Artillery;
                    if (a)
                    {
                        
                    }

                }
            }
           
            
            Turrets[i].Fire();
        }
    }

 


    void AIControl(GameObject target)
    {
        if(!target)
        {
            movement.speedUp = false;
            movement.turnRight = false;
            movement.turnLeft = false;
            for(int i=0;i<Turrets.Length;i++)
            {
                Turrets[i].targetPos=Vector3.zero;
            }
            return;
        }
        AI_MoveLogic(GetFixedPos(target));
        
        
            AI_AimLogic(target);
        
       
        
    }
    //Display enemy search range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusForSearchTarget);
    }
}
