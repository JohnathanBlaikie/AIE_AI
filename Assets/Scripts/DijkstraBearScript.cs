using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraBearScript : MonoBehaviour
{
    //Dijkstra Stuff
    private NodeScript currentNode;
    private Vector3 cVelocity;

    public NodeScript goalNode;
    //Bear Script stuff
    public float dFP = 0, v, aggroMin, beeAlert, personalSpace, speed;
    public Vector3 aggroDis;
    public Vector3 force, forceCorrection, beeEvasion;
    public Vector3 distance = new Vector3();
    //(Possibly not necessary for Dijkstra implementation)
    //public Vector3 velocity = new Vector3();
    //public Vector3 velocityCorrection = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject goal, target1, target2;
    public Rigidbody rb;
    public Rigidbody[] BeeSwarm;

    public List<NodeScript> goalPath = new List<NodeScript>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        aggroVec = goal.transform.position - this.transform.position;
        aggroDis = PosComp(goal.transform, transform);

        for (int i = 0; i < BeeSwarm.Length; i++)
        {
            beeEvasion = PosComp(rb.transform, BeeSwarm[i].transform);

            if (beeEvasion.x < personalSpace && beeEvasion.y < personalSpace && beeEvasion.z < personalSpace)
            {
                goalNode = 
                //velocityCorrection = ((rb.transform.position - BeeSwarm[i].transform.position) * v).normalized;
                //forceCorrection = velocityCorrection - beeEvasion;
                //velocityCorrection += forceCorrection * Time.deltaTime;
                //rb.AddForce((velocityCorrection * Time.deltaTime) * speed * 1.5f);

            }
            else if(beeEvasion.x < beeAlert && beeEvasion.y < beeAlert && beeEvasion.z < beeAlert)
            {
                goalNode = null;
            }
            else goalNode = 
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            FindPath();
        }
        if (goalPath.Count > 0)
        {
            Vector3 v3 = ((goalPath[0].gameObject.transform.position - transform.position) * speed).normalized;
            Vector3 force = v3 - cVelocity;
            cVelocity += force * Time.deltaTime;
            transform.position += cVelocity * Time.deltaTime;
        }
    }

    void FindPath()
    {
        NodeSpawnScript nodeManager = GameObject.Find("GridSpawner").GetComponent<NodeSpawnScript>();
        foreach (var n in nodeManager.grid)
        {
            n.gS = 0;
            n.former = null;
        }

        List<NodeScript> open = new List<NodeScript>();
        List<NodeScript> closed = new List<NodeScript>();
        NodeScript current = currentNode;

        open.Add(current);

        while (open.Count > 0)
        {
            if (closed.Exists(check => goalNode == check))
            {
                break;
            }
            current = open[0];
            open.RemoveAt(0);
            closed.Add(current);

            foreach (var n in current.connections)
            {
                if (!closed.Exists(check => n == check))
                {
                    if (!open.Exists(check => n == check))
                    {
                        n.gS = current.gS + n.moveCost;
                        n.former = current;
                        open.Add(n);
                    }

                    else
                    {
                        if (n.gS > current.gS + n.moveCost)
                        {
                            n.gS = current.gS + n.moveCost;
                            n.former = current;
                        }
                    }
                }
            }
            SortNodes(ref open);
        }
        if (closed.Exists(check => goalNode == check))
        {
            goalPath.Clear();
            NodeScript tGoal = goalNode;
            goalPath.Insert(0, goalNode);

            while (tGoal.former != null)
            {
                goalPath.Insert(0, tGoal.former);
                tGoal = tGoal.former;
            }
            goalPath.Remove(currentNode);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            NodeScript cNode = collision.gameObject.GetComponent<NodeScript>();
            currentNode = cNode;

            if (goalPath.Count > 0)
            {
                if (cNode == goalPath[0])
                {
                    goalPath.Remove(cNode);
                }
            }
        }
    }
    void SortNodes(ref List<NodeScript> nList)
    {
        for (int i = 0; i < nList.Count; i++)
        {
            NodeScript temp = nList[i];

            int j = i - 1;
            while (j >= 0 && nList[j].gS > temp.gS)
            {
                nList[j + 1] = nList[j];
                j = j - 1;
                nList[j + 1] = temp;
                if (j > nList.Count)
                {
                    j = 0;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (goalNode != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(
                goalNode.gameObject.transform.position.x,
                goalNode.gameObject.transform.position.y,
                goalNode.gameObject.transform.position.z), 2f);

        }

        foreach (var n in goalPath)
        {
            Gizmos.color = Color.red;
            if (n.former != null)
            {
                Gizmos.DrawLine(n.gameObject.transform.position, n.former.gameObject.transform.position);

            }
        }
        if (currentNode != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(
                currentNode.gameObject.transform.position.x,
                currentNode.gameObject.transform.position.y,
                currentNode.gameObject.transform.position.z), 2f);
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

