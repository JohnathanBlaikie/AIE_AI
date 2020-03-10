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
        
        aggroDis = PosComp(goal.transform, rb.transform);
        for (int i = 0; i < BeeSwarm.Length; i++)
        {
            beeEvasion = PosComp(rb.transform, BeeSwarm[i].transform);
            if (beeEvasion.x < personalSpace && beeEvasion.y < personalSpace && beeEvasion.z < personalSpace)
            {
                velocityCorrection = ((rb.transform.position - BeeSwarm[i].transform.position) * v).normalized;
                forceCorrection = velocityCorrection - beeEvasion;
                velocityCorrection += forceCorrection * Time.deltaTime;
                rb.AddForce((velocityCorrection * Time.deltaTime) * speed * 1.5f);

            }
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
    public Vector3 PosComp(Transform a, Transform b)
    {
        Vector3 temp;
        temp.x = Mathf.Abs(a.transform.position.x - b.transform.position.x);
        temp.y = Mathf.Abs(a.transform.position.y - b.transform.position.y);
        temp.z = Mathf.Abs(a.transform.position.z - b.transform.position.z);
        return temp;
    }
}
