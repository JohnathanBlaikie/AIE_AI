using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flockScript2 : MonoBehaviour
{
    public enum Beehavior { PATROL, CHASE, GATHER };
    public Beehavior bHE;
    public float dFP, v, aggroMin, personalSpace, isolationAnxiety, speed, callOffPursuit;
    public float patrolTimer, patrolFloat, gatherTimer, gatherFloat;
    public Vector3 aggroDis;
    public Vector3 force, forceCorrection;
    public Vector3 flockSep = new Vector3();
    public Vector3 distance = new Vector3();
    public Vector3 velocity = new Vector3();
    public Vector3 velocityCorrection = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject goal, target1, target2, threat;

    public Time timer;
    public Rigidbody[] flock = new Rigidbody[6];
    // Start is called before the first frame update
    void Start()
    {
        patrolTimer += patrolFloat;
        //foreach (Rigidbody rb in transform)
        //{
        //    flock[flocks] = rb;
        //    flocks++;
        //}
        //goal = target1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time);

        if (Input.GetKeyDown(KeyCode.T))
        {
            
            if(bHE == Beehavior.PATROL)
            {
                bHE = Beehavior.CHASE;
            }
            else if (bHE == Beehavior.CHASE)
            {
                bHE = Beehavior.GATHER;
            }
            else if (bHE == Beehavior.GATHER)
            {
                bHE = Beehavior.PATROL;
            }


        }

        switch (bHE)
        {
            case Beehavior.PATROL:
                goal = target1;
                if (patrolTimer <= Time.time)
                {
                    gatherTimer = gatherFloat + Time.time;
                    bHE = Beehavior.GATHER;
                }
                else if(patrolTimer >= Time.time
            && callOffPursuit >= Vector3.Distance(target1.transform.position, threat.transform.position))
                {
                    bHE = Beehavior.CHASE;
                }
                break;
            case Beehavior.CHASE:
                goal = threat;
                if(callOffPursuit <= Vector3.Distance(target1.transform.position, threat.transform.position))
                    {
                    patrolTimer = patrolFloat + Time.time;
                    bHE = Beehavior.PATROL;
                }
                break;
            case Beehavior.GATHER:
                goal = target2;
                if(gatherTimer <= Time.time)
                {
                    patrolTimer = patrolFloat + Time.time;
                    bHE = Beehavior.PATROL;
                }
                break;
            default:
                break;
        }

        foreach (Rigidbody rb in flock)
        {
            aggroDis = PosComp(goal.transform, rb.transform);
            aggroVec = goal.transform.position - rb.transform.position;
            for (int i = 0; i < flock.Length; i++)
            {

                flockSep = PosComp(rb.transform, flock[i].transform);

                if (flockSep.x < personalSpace && flockSep.y < personalSpace && flockSep.z < personalSpace)
                {
                    velocityCorrection = ((rb.transform.position - flock[i].transform.position) * v).normalized;
                    forceCorrection = velocityCorrection - flockSep;
                    velocityCorrection += forceCorrection * Time.deltaTime;
                    rb.AddForce((velocityCorrection * Time.deltaTime) * speed);
                }
                #region Max distance
                //else if (flockSep.x > isolationAnxiety && flockSep.y > isolationAnxiety && flockSep.z > isolationAnxiety)
                //{
                //    if (i < flock.Length - 1)
                //    {
                //        Vector3 tempFlockSep = PosComp(rb.transform, flock[i + 1].transform);
                //        if (tempFlockSep.x > isolationAnxiety || tempFlockSep.y > isolationAnxiety || tempFlockSep.z > isolationAnxiety)
                //        {
                //            velocityCorrection = ((rb.transform.position - flock[i].transform.position) * v).normalized;
                //            forceCorrection = velocityCorrection - flockSep;
                //            velocityCorrection -= forceCorrection * Time.deltaTime;
                //            rb.AddForce((velocityCorrection * Time.deltaTime) * speed);
                //            //flock[i].AddForce((velocityCorrection * Time.deltaTime) * speed);
                //        }
                //    }
                //    else
                //    {
                //        Vector3 tempFlockSep = PosComp(rb.transform, flock[i - 1].transform);
                //        if (tempFlockSep.x > isolationAnxiety || tempFlockSep.y > isolationAnxiety || tempFlockSep.z > isolationAnxiety)
                //        {
                //            velocityCorrection = ((rb.transform.position - flock[i].transform.position) * v).normalized;
                //            forceCorrection = velocityCorrection - flockSep;
                //            velocityCorrection -= forceCorrection * Time.deltaTime;
                //            rb.AddForce((velocityCorrection * Time.deltaTime) * -speed);
                //            //flock[i].AddForce((velocityCorrection * Time.deltaTime) * speed);
                //        }
                //    }
                //}
                #endregion
            }
            if ((Mathf.Abs(aggroVec.x) <= dFP || Mathf.Abs(aggroVec.y) <= dFP || Mathf.Abs(aggroVec.z) <= dFP) &&
                (Mathf.Abs(aggroDis.x) > aggroMin || Mathf.Abs(aggroDis.y) > aggroMin || Mathf.Abs(aggroDis.z) > aggroMin))
            {
                velocity = ((goal.transform.position - rb.transform.position) * v).normalized;
                force = velocity - distance;
                velocity += force * Time.deltaTime;
                rb.AddForce((velocity * Time.deltaTime) * speed);
            }
        }
    }
    public Vector3 PosComp(Transform a, Transform b)
    {
        Vector3 temp;
        temp.x = Mathf.Abs(a.transform.position.x - b.transform.position.x);
        temp.y = Mathf.Abs(a.transform.position.y - b.transform.position.y);
        temp.z = Mathf.Abs(a.transform.position.z - b.transform.position.z);
        return temp;
    }
    //void Gather(Vector3 location)
    //{
    //    Patrol(location);
    //}
    //void Patrol(Vector3 location)
    //{
    //    Vector3 vec3 = ((location - gameObject.transform.position) * speed).normalized;
    //    Vector3 force = vec3 - velocity;
    //    velocity += force * Time.deltaTime;
    //    gameObject.transform.position += velocity * speed * Time.deltaTime;
    //}
    //
    //void Chase(Vector3 target)
    //{
    //    Vector3 vec3 = ((target - gameObject.transform.position) * speed).normalized;
    //    Vector3 force = vec3 - velocity;
    //    velocity += force * Time.deltaTime;
    //    gameObject.transform.position += velocity * speed * Time.deltaTime;
    //}

}
