using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flockScipt : MonoBehaviour
{
    public float dFP = 0;
    public float v = 0;
    public float aggroDis = 0;
    public float aggroMin = 0;
    public float personalSpace = 0;
    public float speed = 0;
    public int flocks = 0;
    public Vector3 force;
    public Vector3 flockSep = new Vector3();
    public Vector3 distance = new Vector3();
    public Vector3 velocity = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject goal = new GameObject();
    public Transform[] flock = new Transform[6];
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform c in transform)
        {
            flock[flocks] = c;
            flocks++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform c in transform)
        {
            for(int i = 0; i < flock.Length; i++)
            {
               flockSep = new Vector3(
                   Mathf.Abs(c.transform.position.x - flock[i].transform.position.x),
                   Mathf.Abs(c.transform.position.y - flock[i].transform.position.y),
                   Mathf.Abs(c.transform.position.z - flock[i].transform.position.z));

                //flockSep = c.transform.position - flock[i].transform.position;
                if (flockSep.x < personalSpace && flockSep.y < personalSpace && flockSep.z < personalSpace)
                {
                    //if(c.position.x < )
                }
            }
            if ((Mathf.Abs(aggroVec.x) <= dFP || Mathf.Abs(aggroVec.z) <= dFP) && Mathf.Abs(aggroDis) > aggroMin)
            {
                //aggroDis = goal.transform.position.x - transform.position.x;
                aggroVec = goal.transform.position - c.transform.position;
                velocity = ((goal.transform.position - c.transform.position) * v).normalized;
                force = velocity - distance;
                velocity += force * Time.deltaTime;
                c.transform.position += (velocity * Time.deltaTime) * speed;
                c.transform.rotation = Quaternion.LookRotation(velocity);
            }
        }
    }
}
