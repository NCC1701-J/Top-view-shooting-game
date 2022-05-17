using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour
{
    [SerializeField]
    bool hasAnim = true;

    [SerializeField]
    private Transform model;
    public Transform Model
    {
        get
        {
            return model ? model : transform;
        }
    }
    [SerializeField]
    private float FireFrequency = 0.4f;
    [SerializeField]
    private float PrevFireTime = float.MinValue;

    [SerializeField]
    private AnimationCurve LerpCurve =AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);

    public Transform muzzle;

    bool controllerIsPlayer = false;

    [SerializeField]
    TurretsContorl turrets;


    public float  speed;

    public float lifeTime;

    public int damage;

    public bool ReadyToShoot
    {
        get
        {
            if (controllerIsPlayer)
            {
                return PrevFireTime + FireFrequency < Time.time;
            }
            else
            {
                return (PrevFireTime + FireFrequency < Time.time) && turrets.inShootArea;
            }
        }

    }

    void Start()
    {
        Transform root = GetRoot(transform);
        
        if (root.tag== "Player")
        {      
            controllerIsPlayer = true;   
        }
        if(!turrets)
        {
            turrets = transform.GetComponentInParent<TurretsContorl>();
        }
        if(!muzzle)
        {
            muzzle = transform.Find("Muzzle").transform;
        }   

    }

    Transform GetRoot(Transform t)
    {
        if(t.parent==null)
        {
            return t;
        }
        else
        {
            return GetRoot(t.parent);
        }
    }

    void Update()
    {
        if (hasAnim)
            AnimUpdate();
    }


    void Shoot()
    {
       
        
    }

    public void Fire()
    {

        if (ReadyToShoot)
        {
            Shoot();
            PrevFireTime = Time.time;
        }

    }

    void AnimUpdate()
    {
        float t = Time.time - PrevFireTime;
        model.localPosition = Vector3.forward * LerpCurve.Evaluate(t);
    }
    

  

#if UNITY_EDITOR

    private void Reset()
    {
        model = transform.GetChild(0).transform;
        muzzle = transform.Find("Muzzle").transform;
        turrets = transform.GetComponentInParent<TurretsContorl>();
    }

#endif

}
