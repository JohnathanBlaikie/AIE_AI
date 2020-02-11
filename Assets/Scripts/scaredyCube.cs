using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaredyCube : MonoBehaviour
{
    public float dFP = 0;
    public float v = 0;
    public float spookRadius = 0;
    public Vector3 force;
    public Vector3 distance = new Vector3();
    public Vector3 velocity = new Vector3();
    public GameObject scaryThing = new GameObject();
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        //direction = (velocity) * Time.deltaTime;
        //v = ((scaryThing.transform - transform.position) * velocity).normalized;

        spookRadius = scaryThing.transform.position.x - transform.position.x;
        if (dFP >= Mathf.Abs(spookRadius))
        {
            velocity = ((scaryThing.transform.position - transform.position) * v).normalized;
            force = velocity - distance;
            velocity += force * Time.deltaTime;
            transform.position -= velocity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
