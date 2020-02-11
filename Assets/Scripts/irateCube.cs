﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class irateCube : MonoBehaviour
{
    public float dFP = 0;
    public float v = 0;
    public float aggroDis = 0;
    public float aggroMin = 0;
    public float speed = 0;
    public Vector3 force;
    public Vector3 distance = new Vector3();
    public Vector3 velocity = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject annoyingThing = new GameObject();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aggroVec = annoyingThing.transform.position - transform.position;
        aggroDis = annoyingThing.transform.position.x - transform.position.x;
        if (Mathf.Abs(aggroVec.x) <= dFP || Mathf.Abs(aggroVec.z) <= dFP &&  Mathf.Abs(aggroDis) > aggroMin)
        {
            velocity = ((annoyingThing.transform.position - transform.position) * v).normalized;
            force = velocity - distance;
            velocity += force * Time.deltaTime;
            transform.position += (velocity * Time.deltaTime) * speed;
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}