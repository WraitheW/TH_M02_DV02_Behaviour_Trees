using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public List<Connection> mConnections;

    public List<Connection> getConnections(Node fromNode)
    {
        List<Connection> connections = new List<Connection>();
        foreach (Connection con in mConnections)
        {
            if (con.getFromNode() == fromNode)
            {
                connections.Add(con);
            }
        }

        return connections;
    }

    public void Build(bool isCheap)
    {
        mConnections = new List<Connection>();

        Node[] nodes = GameObject.FindObjectsOfType<Node>();

        if (isCheap)
        {
            foreach (Node fromNode in nodes)
            {
                foreach (Node toNode in fromNode.ConnectsTo)
                {
                    float cost;

                    if (fromNode.gameObject.tag == "cheap" && toNode.gameObject.tag == "cheap")
                    {
                        cost = 1.0f;
                    }
                    else
                    {
                        cost = (toNode.transform.position - fromNode.transform.position).magnitude;
                    }
                    Connection c = new Connection(cost, fromNode, toNode);
                    mConnections.Add(c);
                }
            }
        }
        else
        {
            foreach (Node fromNode in nodes)
            {
                foreach (Node toNode in fromNode.ConnectsTo)
                {
                    float cost = (toNode.transform.position - fromNode.transform.position).magnitude;
                    Connection c = new Connection(cost, fromNode, toNode);
                    mConnections.Add(c);
                }
            }
        }
    }
}

public class Connection
{
    float cost;
    Node fromNode;
    Node toNode;

    public Connection(float cost, Node fromNode, Node toNode)
    {
        this.cost = cost;
        this.fromNode = fromNode;
        this.toNode = toNode;
    }

    public float getCost()
    {
        return cost;
    }

    public Node getFromNode()
    {
        return fromNode;
    }

    public Node getToNode()
    {
        return toNode;
    }
}
