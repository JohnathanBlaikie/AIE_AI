using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearScript : MonoBehaviour
{
    public float dFP = 0, v, aggroMin, personalSpace, speed;
    public Vector3 aggroDis;
    public Vector3 force, forceCorrection, beeEvasion;
    public Vector3 distance = new Vector3();
    public Vector3 velocity = new Vector3();
    public Vector3 velocityCorrection = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject goal, target1, target2;
    public Rigidbody rb;
    public Rigidbody[] BeeSwarm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aggroVec = goal.transform.position - rb.transform.position;
        //aggroDis.x = Mathf.Abs(goal.transform.position.x - rb.transform.position.x);
        //aggroDis.y = Mathf.Abs(goal.transform.position.y - rb.transform.position.y);
        //aggroDis.z = Mathf.Abs(goal.transform.position.z - rb.transform.position.z);
        aggroDis = PosComp(goal.transform, rb.transform);
        for (int i = 0; i < BeeSwarm.Length; i++)
        {
            //beeEvasion = new Vector3(
            //    Mathf.Abs(rb.transform.position.x - BeeSwarm[i].transform.position.x),
            //    Mathf.Abs(rb.transform.position.y - BeeSwarm[i].transform.position.y),
            //    Mathf.Abs(rb.transform.position.z - BeeSwarm[i].transform.position.z));
            beeEvasion = PosComp(rb.transform, BeeSwarm[i].transform);

            if (beeEvasion.x < personalSpace && beeEvasion.y < personalSpace && beeEvasion.z < personalSpace)
            {
                //if(c.position.x < )
                velocityCorrection = ((rb.transform.position - BeeSwarm[i].transform.position) * v).normalized;
                forceCorrection = velocityCorrection - beeEvasion;
                velocityCorrection += forceCorrection * Time.deltaTime;
                rb.AddForce((velocityCorrection * Time.deltaTime) * speed * 1.5f);
                //rb.rotation = Quaternion.LookRotation(velocity);

            }
        }
        if ((Mathf.Abs(aggroVec.x) <= dFP || Mathf.Abs(aggroVec.y) <= dFP || Mathf.Abs(aggroVec.z) <= dFP) &&
            (Mathf.Abs(aggroDis.x) > aggroMin || Mathf.Abs(aggroDis.y) > aggroMin || Mathf.Abs(aggroDis.z) > aggroMin))
        {
            // aggroDis = goal.transform.position.x - transform.position.x;

            velocity = ((goal.transform.position - rb.transform.position) * v).normalized;
            force = velocity - distance;
            velocity += force * Time.deltaTime;
            rb.AddForce((velocity * Time.deltaTime) * speed);
           // rb.rotation = Quaternion.LookRotation(velocity);
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
