﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearScript : MonoBehaviour
{
    public float dFP = 0;
    public float v = 0;
    public Vector3 aggroDis;
    public float aggroMin = 0;
    public float personalSpace = 0;
    public float speed = 0;
    public Vector3 force;
    public Vector3 forceCorrection;
    public Vector3 distance = new Vector3();
    public Vector3 velocity = new Vector3();
    public Vector3 velocityCorrection = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject goal;
    public GameObject target1;
    public GameObject target2;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aggroVec = goal.transform.position - rb.transform.position;
        aggroDis.x = Mathf.Abs(goal.transform.position.x - rb.transform.position.x);
        aggroDis.x = Mathf.Abs(goal.transform.position.y - rb.transform.position.y);
        aggroDis.z = Mathf.Abs(goal.transform.position.z - rb.transform.position.z);
        
        if ((Mathf.Abs(aggroVec.x) <= dFP || Mathf.Abs(aggroVec.y) <= dFP || Mathf.Abs(aggroVec.z) <= dFP) &&
            (Mathf.Abs(aggroDis.x) > aggroMin || Mathf.Abs(aggroDis.y) > aggroMin || Mathf.Abs(aggroDis.z) > aggroMin))
        {
            // aggroDis = goal.transform.position.x - transform.position.x;

            velocity = ((goal.transform.position - rb.transform.position) * v).normalized;
            force = velocity - distance;
            velocity += force * Time.deltaTime;
            rb.AddForce((velocity * Time.deltaTime) * speed);
            rb.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
