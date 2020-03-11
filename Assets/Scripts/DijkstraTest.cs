using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraTest : MonoBehaviour
{
    private NodeScript currentNode;
    private Vector3 cVelocity;

    public NodeScript goal;
    public float speed;

    public List<NodeScript> goalPath = new List<NodeScript>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FindPath();
        }
        if(goalPath.Count > 0)
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
            if (closed.Exists(check => goal == check))
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
        if (closed.Exists(check => goal == check))
        {
            goalPath.Clear();
            NodeScript tGoal = goal;
            goalPath.Insert(0, goal);

            while (tGoal.former != null)
            {
                goalPath.Insert(0, tGoal.former);
                tGoal = tGoal.former;
            }
            goalPath.Remove(currentNode);
        }
    }
    
   // private void OnCollisionEnter(Collision collision)
   // {
   //     if (collision.gameObject.CompareTag("Node"))
   //     {
   //         NodeScript cNode = collision.gameObject.GetComponent<NodeScript>();
   //         currentNode = cNode;
   //
   //         if (goalPath.Count > 0)
   //         {
   //             if (cNode == goalPath[0])
   //             {
   //                 goalPath.Remove(cNode);
   //             }
   //         }
   //     }
   // }
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
            while(j >= 0 && nList[j].gS > temp.gS)
            {
                nList[j + 1] = nList[j];
                j = j - 1;
                nList[j + 1] = temp;
                if(j > nList.Count)
                {
                    j = 0;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(goal != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(
                goal.gameObject.transform.position.x,
                goal.gameObject.transform.position.y, 
                goal.gameObject.transform.position.z), 2f);

        }

        foreach(var n in goalPath)
        {
            Gizmos.color = Color.red;
            if(n.former != null)
            {
                Gizmos.DrawLine(n.gameObject.transform.position, n.former.gameObject.transform.position);

            }
        }
        if(currentNode != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(
                currentNode.gameObject.transform.position.x, 
                currentNode.gameObject.transform.position.y, 
                currentNode.gameObject.transform.position.z),2f);
        }
    }
}
