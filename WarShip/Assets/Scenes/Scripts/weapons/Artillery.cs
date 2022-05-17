using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Emitter
{
    [SerializeField]
    private Transform model;

    float spreadRadius;

    Vector3 TargetDropPos = Vector3.zero;

    /// <summary>
    /// Maximum elevation angle
    /// </summary>
    public float MaxElevation=10f;
    /// <summary>
    /// Maximum pitch angle
    /// </summary>
    public float MaxDepression=-5f;

    float AngleOfPitch;

    float G = 10f;
    [SerializeField]
    float d_offset=0;

    public Transform Model
    {
        get
        {
            return model ? model : transform.Find("Model").transform;
        }
    }


    [SerializeField]
    private AnimationCurve LerpCurve = AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);

    TurretsContorl t;

    public Mesh mesh;
    Vector3 drawPos = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        t = GetComponentInParent<TurretsContorl>();
    }

    protected override void Update()
    {
        AnimUpdate();
        float velocity = target.GetComponent<Rigidbody>().velocity.magnitude;

        t.targetPos = drawPos;
        //GetFireAngle(target.position);
        RotateUpdate();
    }

    public void GetTargetDrop()
    {
        Vector3 targetPos = transform.GetComponentInParent<TurretsContorl>().targetPos;
        TargetDropPos = new Vector3(targetPos.x, 0, targetPos.z);
    }


    void AnimUpdate()
    {   
        float t = Time.time - PrevFireTime;
        model.localPosition = Vector3.forward * LerpCurve.Evaluate(t);
    }

    Vector3 Vector_Y2Zero(Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }

    void SimulationDropPos_visualization(Vector3 direction)
    {
        float time = 0;

        Ray ray = new Ray();
        RaycastHit hit;
        Vector3 move = Vector3.zero;

        Vector3 curPos = muzzle.position;

        while (!Physics.Raycast(ray, out hit, move.magnitude, 1 << LayerMask.NameToLayer("Sea")) && curPos.y > 0)
        {
            Vector3 gravDir = Vector3.down * G * time;

            move = (direction * speed + gravDir) * Time.fixedDeltaTime;

            ray = new Ray(curPos, move);
            Debug.DrawRay(curPos, move, Color.red);
            curPos += move;
            time += Time.fixedDeltaTime;
        }
        if (hit.transform)
        {
            curPos = hit.point;
        }

        Debug.DrawRay(curPos, Vector3.up * 10, Color.black);
    }

    Vector2 SimulationProjectile(float X, float Y, float V, float G)
    {
        if (G == 0)
        {
            float THETA = Mathf.Atan(Y / X);
            float T = (Y / Mathf.Sin(THETA)) / V;
            return (new Vector2(THETA, T));
        }
        else
        {
            float DELTA = Mathf.Pow(V, 4) - G * (G * X * X - 2 * Y * V * V);
            if (DELTA < 0)
            {
                return Vector2.zero;
            }
            float rad1 = Mathf.Atan(((V * V) + Mathf.Sqrt(DELTA)) / (G * X));
            float rad2 = Mathf.Atan(((V * V) - Mathf.Sqrt(DELTA)) / (G * X));

            float rad = Mathf.Min(rad1, rad2);
            float T = X / (V * Mathf.Cos(rad));
            return new Vector2(rad, T);
        }
    }

    Vector3 Sim_DropPos(float speed, Vector3 curPos, Vector3 tarTowards, float time)
    {
        Vector3 sim_pos = Vector_Y2Zero(curPos) + Vector_Y2Zero(tarTowards) * (speed * time);
        return sim_pos;
    }


    //public float GetFireAngle(Vector3 tarPos)
    //{

    //    if (tarPos == Vector3.zero) { return 0f; }


    //    Vector3 Dir = (Vector_Y2Zero(tarPos) - Vector_Y2Zero(transform.parent.position)).normalized;

    //    float d_aix2muzzle = (Vector_Y2Zero(transform.parent.position) - Vector_Y2Zero(muzzle.position)).magnitude;
    //    float d_tar2aix = (Vector_Y2Zero(transform.parent.position) - Vector_Y2Zero(tarPos)).magnitude;

    //    float d = d_tar2aix - d_aix2muzzle + d_offset;
    //    float h = muzzle.transform.position.y;

    //    float X = d;
    //    float Y = h;
    //    float V = speed;
        //Debug.Log(" X:" + X + " Y:" + Y + " V:" +V+ " G:" + G);
        //float angle = SimulationProjectile(X,Y,V,G).x;



        //float a = Mathf.Sqrt((speed * speed * speed * speed - G * (G * d * d - 2 * h * speed * speed)));
        //float tanAngle_1 = ((speed * speed + a) / (G * d));
        //float tanAngle_2 = ((speed * speed - a) / (G * d));
        //float a1 = Mathf.Atan(tanAngle_1) * Mathf.Rad2Deg;
        //float a2 = Mathf.Atan(tanAngle_2) * Mathf.Rad2Deg;



        //float angle = Mathf.Min(a1, a2);

        //Debug.Log("old"+a1 + ":" + a2);

    //    AngleOfPitch = angle*Mathf.Rad2Deg;

    //    return angle;
    //}

    void RotateUpdate()
    {
        //Limit pitch angle
        float angle = Mathf.Clamp(AngleOfPitch,MaxDepression,MaxElevation) ;
        
        //Debug.Log(angle);

        transform.localRotation = Quaternion.Euler(new Vector3(-angle, 0, 0));
        //transform.RotateAround(transform.position, transform.right, angle);

    }

    protected override void Shoot()
    {
        Instantiate(projectile).GetComponent<Projectile>().Init(muzzle.position, transform.forward, speed, lifeTime, damage, this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        model = transform.Find("Model").transform;
    }
#endif
}
