using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawnScript : MonoBehaviour
{
    public GameObject pNode;

    public NodeScript[,] grid;
    public int x, y;

    // Start is called before the first frame update
    void Start()
    {
        grid = new NodeScript[x, y];
        
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                grid[i, j] = Instantiate(pNode, new Vector3(i, 0, j), Quaternion.identity).GetComponent<NodeScript>();
            }
        }

        for(int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if(i - 1 >= 0)
                {
                    grid[i, j].connections.Add(grid[i - 1, j]);
                }
                if (i + 1 < x)
                {
                    grid[i, j].connections.Add(grid[i + 1, j]);
                }
                if (j - 1 >= 0)
                {
                    grid[i, j].connections.Add(grid[i, j - 1]);
                }
                if (j + 1 < y)
                {
                    grid[i, j].connections.Add(grid[i, j + 1]);
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
