using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshOnClick : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent nMA;
    // Start is called before the first frame update
    void Start()
    {
        nMA = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(r,out RaycastHit hit))
            {
                nMA.SetDestination(hit.point);
            }
        }
    }
}
