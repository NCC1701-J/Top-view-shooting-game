using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TController : MonoBehaviour
{
    [Range(30, 330)]

    public float LimitAngle = 60f;
    public int RotateSpeed = 180;

    public Camera cam;



    void Update()
    {
        RotateUpdate();
    }

    void RotateUpdate()
    {
        //Vector3 limit_dir = GetMouseDir_limit(LimitAngle);

        //Quaternion rotate = Quaternion.RotateTowards(transform.rotation,
        //Quaternion.LookRotation(limit_dir, transform.up),
        //RotateSpeed * Time.deltaTime);
        //transform.rotation = rotate;

        Vector3 aixs;
        float angle;
        Vector3 limit_dir = GetMouseDir_limit(LimitAngle);

        //The relationship between the target vector and the positive direction of the Z axis
        bool tar_is_right = Vector3.Cross(limit_dir, transform.parent.forward).y > 0;

        //The relationship between the current orientation of the turret and the target vector
        bool cur_is_right = Vector3.Cross(transform.forward, limit_dir).y > 0;

        //The relationship between the current orientation and the positive reference direction
        bool tra_is_right = Vector3.Cross(transform.forward, transform.parent.forward).y > 0;


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
                angle = 360 - Vector3.Angle(transform.forward, -limit_dir);
                aixs = Vector3.Cross(transform.forward, -limit_dir);
            }

            else
            {
                angle = Vector3.Angle(transform.forward, limit_dir);
                aixs = Vector3.Cross(transform.forward, limit_dir);
            }
        }

        else
        {
            angle = Vector3.Angle(transform.forward, limit_dir);
            aixs = Vector3.Cross(transform.forward, limit_dir);
        }

        transform.Rotate(aixs, Mathf.Min(RotateSpeed * Time.deltaTime, angle));
    }

    //Get the orientation within the limited angle
    Vector3 GetMouseDir_limit(float limit_angle)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitpos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 dir = (hitpos - transform.position).normalized;
            Debug.DrawLine(hitpos, transform.position, Color.blue, 0.5f);

            //If the pointing direction is more than half of the limit angle, returns the product of the limit angle and the Z axis of the ship
            if (Vector3.Angle(transform.parent.forward, dir) > limit_angle / 2)
            {
                bool in_my_right = Vector3.Cross(transform.parent.forward, dir).y > 0;
                Quaternion q = Quaternion.AngleAxis((in_my_right ? limit_angle : -limit_angle) / 2, transform.parent.up);
                Debug.DrawRay(transform.position, q * transform.parent.forward * 5, Color.red, 0.5f);
                return q * transform.parent.forward;
            }
            else
            {
                Debug.DrawRay(transform.position, (hitpos - transform.position).normalized * 5, Color.green, 0.5f);
                return (hitpos - transform.position).normalized;
            }
        }
        else
        {
            return transform.parent.forward;
        }
    }
}