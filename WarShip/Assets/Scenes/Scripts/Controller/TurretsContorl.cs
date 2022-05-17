using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsContorl : MonoBehaviour
{
    [Range(30,330)]
    public float LimitAngle=60f;

    public int RotateSpeed=180;

    public Vector3 targetPos;

    public float deviationAngle;

    float currentAngle=359f;

    float fireAngle = 0;
    
    public  Emitter[] m_weapons;


    bool CanRotate2Target = false;

    public bool inShootArea
    {
        get
        {
            
            return currentAngle < deviationAngle&&CanRotate2Target;
        }
    }


    void Start ()
    {
        m_weapons = transform.GetComponentsInChildren<Emitter>();
        
	}
	
	
	void Update ()
    {
        RotateUpdate();
	}


    public void Fire()
    {
        for (int i = 0; i < m_weapons.Length; i++)
        {   
            m_weapons[i].Fire();
        }
    }

    public void RotateUpdate()
    {
        
        Vector3 aixs;
        float angle;
        //------------------------------------------------------------------------
        Vector3 limit_dir = GetMouseDir_limit(LimitAngle,targetPos);

        //The relationship between the target vector and the positive direction of the Z axis
        bool tar_is_right = Vector3.Cross(limit_dir, transform.parent.forward).y > 0;

        //The relationship between the current orientation of the turret and the target vector
        bool cur_is_right = Vector3.Cross(transform.forward, limit_dir).y > 0;

        //The relationship between the current orientation and the positive reference direction
        bool tra_is_right = Vector3.Cross(transform.forward, transform.parent.forward).y > 0;

        angle = Vector3.Angle(transform.forward, limit_dir);

        Debug.DrawRay(transform.position, transform.forward*10f,Color.green);
        Debug.DrawRay(transform.position, limit_dir*10f,Color.blue);
        //Debug.Log(transform.name + ":" + angle);
        if(targetPos!=Vector3.zero)
        {
            currentAngle = angle;
        }
        

        aixs = cur_is_right ? transform.up : -transform.up;

        //If the target vector is outside positive 180 degrees, use this condition
        if (Vector3.Dot(limit_dir, transform.parent.forward) <= 0)
        {
            //current heading edge vector
            Vector3 edge = Quaternion.AngleAxis(LimitAngle / 2, tra_is_right ? transform.parent.up : -transform.parent.up) * transform.parent.forward;

            //The angle between orientation and edge
            float edge_angle = Vector3.Angle(transform.forward, edge);

            //If the steering passes through the datum negative and the angle between the target and the boundary is less than the angle between the heading and the boundary, reverse the angle and axis
            if (((tar_is_right && cur_is_right) || (!tar_is_right && !cur_is_right)) && Vector3.Angle(limit_dir, edge) < edge_angle)
            {
                angle = 360 - angle;
                aixs = -aixs;
               
            }  
        }
        if (Mathf.Abs(transform.localRotation.x - 0) > 0.1f || (Mathf.Abs(transform.localRotation.z - 0) > 0.1f))
        {
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 0);
        }


        transform.Rotate(aixs, Mathf.Min(RotateSpeed * Time.deltaTime, angle));
        
    }

    Vector3 GetMouseDir_limit(float limit_angle,Vector3 mousePos)
    {    

        if (mousePos!=Vector3.zero)
        {
            Vector3 hitpos = new Vector3(mousePos.x, transform.position.y, mousePos.z);
            Vector3 dir = (hitpos - transform.position).normalized;
            //Debug.DrawLine(hitpos, transform.position, Color.blue, 0.5f);

            if (Vector3.Angle(transform.parent.forward,dir)>limit_angle/2)
            {
                bool in_my_right = Vector3.Cross(transform.parent.forward, dir).y > 0;

                Quaternion q = Quaternion.AngleAxis((in_my_right ? limit_angle:-limit_angle)/2, transform.parent.up);

                //Debug.DrawRay(transform.position, q * transform.parent.forward*5, Color.red, 0.5f);


                Debug.DrawRay(transform.position + Vector3.up, q * transform.parent.forward, Color.cyan);
                CanRotate2Target = false;
                return q * transform.parent.forward;
            }
            else
            {
                //Debug.DrawRay(transform.position, (hitpos - transform.position).normalized * 5, Color.yellow, 0.5f);

                Debug.DrawRay(transform.position + Vector3.up, (hitpos - transform.position).normalized, Color.white);
                CanRotate2Target = true;
                return (hitpos - transform.position).normalized;

                
            }         
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.parent.forward, Color.black);
            CanRotate2Target = false;
            return transform.parent.forward;
        }


    }

}
