using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform model;

    public Transform Model
    {
        get
        {
            return model ? model : transform.Find("Model").transform;
        }
    }

    //Set the animation curve of the recoil mechanism
    [SerializeField]
    private AnimationCurve LerpCurve = AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);

    //Associated fire and recoil animations
    void AnimUpdate()
    {
        float t = Time.time - PrevFireTime;
        model.localPosition = Vector3.forward * LerpCurve.Evaluate(t);
    }

    //Fire Frequency

    public float FireFrequency = 0.4f;

    //Time of last fire

    private float PrevFireTime = float.MinValue;



    //If time of last fire add Fire Frequency less than the current game time, the weapon are prepare to shoot.

    private bool ReadyToShoot
    {
        get
        {
            return PrevFireTime + FireFrequency < Time.time ? true : false;
        }
    }

    void Update()
    {
        KeyUpdate();
    }

    void Fire()
    {
        Debug.Log("Fire");
    }

    void KeyUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (ReadyToShoot)
            {
                Fire();
                PrevFireTime = Time.time;
            }
        }
    }
}
