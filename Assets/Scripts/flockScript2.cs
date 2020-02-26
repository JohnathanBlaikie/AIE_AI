using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flockScript2 : MonoBehaviour
{
    public float dFP = 0;
    public float v = 0;
    public float aggroMin = 0;
    public float personalSpace = 0;
    public float isolationAnxiety;
    public float speed = 0;
    public Vector3 aggroDis;
    public Vector3 force;
    public Vector3 forceCorrection;
    public Vector3 flockSep = new Vector3();
    public Vector3 distance = new Vector3();
    public Vector3 velocity = new Vector3();
    public Vector3 velocityCorrection = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject goal;
    public GameObject target1;
    public GameObject target2;
    public GameObject threat;

    public Rigidbody[] flock = new Rigidbody[6];
    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (goal == null)
            {
                goal = target1;
            }
            else if (goal == target1)
            {
                goal = target2;
            }
            else if (goal == target2)
            {
                goal = null;
            }

        }

        foreach (Rigidbody rb in flock)
        {
            aggroVec = goal.transform.position - rb.transform.position;
            aggroDis.x = Mathf.Abs(goal.transform.position.x - rb.transform.position.x);
            aggroDis.y = Mathf.Abs(goal.transform.position.y - rb.transform.position.y);
            aggroDis.z = Mathf.Abs(goal.transform.position.z - rb.transform.position.z);
            for (int i = 0; i < flock.Length; i++)
            {
                //flockSep = new Vector3(
                //    Mathf.Abs(rb.transform.position.x - flock[i].transform.position.x),
                //    Mathf.Abs(rb.transform.position.y - flock[i].transform.position.y),
                //    Mathf.Abs(rb.transform.position.z - flock[i].transform.position.z));

                flockSep = PosComp(rb.transform, flock[i].transform);

                if (flockSep.x < personalSpace && flockSep.y < personalSpace && flockSep.z < personalSpace)
                {
                    //if(c.position.x < )
                    velocityCorrection = ((rb.transform.position - flock[i].transform.position) * v).normalized;
                    forceCorrection = velocityCorrection - flockSep;
                    velocityCorrection += forceCorrection * Time.deltaTime;
                    rb.AddForce((velocityCorrection * Time.deltaTime) * speed);
                    //rb.rotation = Quaternion.LookRotation(velocity);

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
                // aggroDis = goal.transform.position.x - transform.position.x;

                velocity = ((goal.transform.position - rb.transform.position) * v).normalized;
                force = velocity - distance;
                velocity += force * Time.deltaTime;
                rb.AddForce((velocity * Time.deltaTime) * speed);
                //rb.rotation = Quaternion.LookRotation(velocity);
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
}
