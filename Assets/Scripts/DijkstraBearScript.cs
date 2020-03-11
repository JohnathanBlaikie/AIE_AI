using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraBearScript : MonoBehaviour
{
    public enum BearBrain { GATHER, FLEE, HOLD};
    public BearBrain bB;
    public int gC = 0, fC = 0, hC = 0;
    //Dijkstra Stuff
    private NodeScript currentNode;
    public NodeScript goalNode;
    public GameObject gridSpawner;
    public NodeSpawnScript nSS;
    private Vector3 cVelocity;
    //Bear Script stuff
    public float dFP = 0, v, aggroMin, beeAlert, personalSpace, speed;
    public float waitTime, waitFloat, gatherTime, gatherFloat, hideTime, hideFloat;
    public Vector3 aggroDis;
    public Vector3 force, forceCorrection, beeEvasion;
    public Vector3 distance = new Vector3();
    //(Possibly not necessary for Dijkstra implementation)
    //public Vector3 velocity = new Vector3();
    //public Vector3 velocityCorrection = new Vector3();
    public Vector3 aggroVec = new Vector3();
    public GameObject goalGameObject, target1, target2;
    //public Rigidbody rb;
    public Rigidbody[] BeeSwarm;

    public List<NodeScript> goalPath = new List<NodeScript>();
    // Start is called before the first frame update
    void Start()
    {
        //ClosestPoint(gridSpawner, target1.transform, (nSS.x * nSS.y));

    }

    private void Awake()
    {
    }
    // Update is called once per frame
    void Update()
    {
        aggroVec = goalGameObject.transform.position - this.transform.position;
        aggroDis = PosComp(goalGameObject.transform, this.transform);

        switch (bB)
        {
            case BearBrain.GATHER:

                goalGameObject = target1;
                if (gC == 0)
                {
                    goalNode = ClosestPoint(gridSpawner, target1.transform, nSS.x * nSS.y);
                    FindPath();
                }
                for (int i = 0; i < BeeSwarm.Length; i++)
                {
                    beeEvasion = PosComp(transform, BeeSwarm[i].transform);
                    if (beeEvasion.x < beeAlert &&
                        beeEvasion.y < beeAlert &&
                        beeEvasion.z < beeAlert)
                    {
                        hC = 0;
                        waitTime = waitFloat + Time.time;
                        bB = BearBrain.HOLD;
                    }
                }
                if (gC > 400)
                {
                    gC = 0;
                }
                gC++;
                // goalNode = ClosestPoint(gridSpawner, goal.transform, (nSS.x * nSS.y));
                break;
            case BearBrain.HOLD:
                //goalGameObject = null;
                if(hC == 0)
                {
                    goalNode = ClosestPoint(gridSpawner, transform, nSS.x * nSS.y);
                    FindPath();
                }
                if (beeEvasion.x < personalSpace &&
                   beeEvasion.y < personalSpace &&
                   beeEvasion.z < personalSpace)
                {
                    fC = 0;
                    hideTime = hideFloat + Time.time;
                    bB = BearBrain.FLEE;
                    //goalNode = ClosestPoint(gridSpawner, target2.transform, nSS.x * nSS.y);
                }
                else if (waitTime <= Time.time && beeAlert > personalSpace)
                {
                    gC = 0;
                    bB = BearBrain.GATHER;
                }
                hC++;
                break;
            case BearBrain.FLEE:
                goalGameObject = target2;
                if (fC == 0)
                {
                    goalNode = ClosestPoint(gridSpawner, target2.transform, nSS.x * nSS.y);
                    FindPath();
                }
                if (hideTime <= Time.time)
                {
                    gC = 0;
                    bB = BearBrain.GATHER;
                }
                if(fC > 200)
                {
                    fC = 0;
                }
                fC++;
                break;
            //default:
        }

       

        if (Input.GetKeyDown(KeyCode.P))
        {
            FindPath();
        }
        if (goalPath.Count > 0)
        {
            Vector3 v3 = ((goalPath[0].gameObject.transform.position - transform.position)).normalized;
            Vector3 force = v3 - cVelocity;
            cVelocity += force * Time.deltaTime;
            
            transform.position += cVelocity * speed * Time.deltaTime;
            // transform.position = goalPath[0].gameObject.transform.position;

            // check if at destination here
            //insert ontrigger code here

            //if (currentNode == goalPath[0])
            //{
            //    goalPath.Remove(currentNode);
            //}
           
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Node"))
        {
            NodeScript cNode = other.gameObject.GetComponent<NodeScript>();
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
                goalNode.gameObject.transform.position.z), 0.2f);

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
                currentNode.gameObject.transform.position.z), 0.2f);
        }
    }
    public Vector3 PosComp(Transform a, Transform b)
    {
        Vector3 temp;
        temp.x = Mathf.Abs(a.transform.position.x - b.transform.position.x);
        temp.y = Mathf.Abs(a.transform.position.y - b.transform.position.y);
        temp.z = Mathf.Abs(a.transform.position.z - b.transform.position.z);
        //Vector3.Distance(a.transform.position, b.transform.position);

        return temp;
    }
    
    public NodeScript ClosestPoint(GameObject spawner, Transform tempGoal, int totalNodes)
    {

        NodeScript[] nodeLoc = new NodeScript[totalNodes];
        int counter = 0;
        float dis = 0;
        NodeScript closestNode = new NodeScript();
        foreach(NodeScript tf in spawner.GetComponentsInChildren<NodeScript>())
        {
            //Vector3.Distance(nodeLoc[counter].transform, )
            nodeLoc[counter] = tf;
            counter++;
        }
        for(int i = 0; i < totalNodes; i++)
        {
            float tempDis;
            if(i == 0)
            dis = Vector3.Distance(nodeLoc[i].transform.position, tempGoal.position);
            else
            {
                tempDis = Vector3.Distance(nodeLoc[i].transform.position, tempGoal.position);
                if(tempDis < dis)
                {
                    closestNode = nodeLoc[i];
                    dis = tempDis;
                }
            }
        }
    
        return closestNode;
    }
}

