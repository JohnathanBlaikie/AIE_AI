﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public int moveCost;
    public int gS,hS,fS;
    public Color color;
    MeshRenderer render;

    public List<NodeScript> connections = new List<NodeScript>();

    public NodeScript former;
    // Start is called before the first frame update
    void Start()
    {
        render = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        render.material.SetColor("_Color", color);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere
        (new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 0.2f);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach(var node in connections)
        {
            Gizmos.DrawLine(transform.position, node.gameObject.transform.position);
        }
    }
}
