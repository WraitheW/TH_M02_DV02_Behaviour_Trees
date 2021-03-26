using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dijkstra
{
    class NodeRecord : IComparable<NodeRecord>
    {
        public Node node;
        public Connection connection;
        public float currentCost;

        public int CompareTo(NodeRecord other)
        {
            if (other == null)
            {
                return 1;
            }

            return (int)(currentCost - other.currentCost);
        }

    }

    class PathFindingList
    {
        List<NodeRecord> nodeRecords = new List<NodeRecord>();

        public void add(NodeRecord n)
        {
            nodeRecords.Add(n);
        }

        public void remove(NodeRecord n)
        {
            nodeRecords.Remove(n);
        }

        public NodeRecord smallElement()
        {
            nodeRecords.Sort();
            return nodeRecords[0];
        }

        public int length()
        {
            return nodeRecords.Count;
        }

        public bool contains(Node node)
        {
            foreach (NodeRecord n in nodeRecords)
            {
                if (n.node == node)
                {
                    return true;
                }
            }
            return false;
        }

        public NodeRecord find(Node node)
        {
            foreach (NodeRecord n in nodeRecords)
            {
                if (n.node == node)
                {
                    return n;
                }
            }
            return null;
        }
    }

    public static List<Connection> pathFind(Graph graph, Node start, Node goal)
    {
        NodeRecord startR = new NodeRecord();
        startR.node = start;
        startR.connection = null;
        startR.currentCost = 0;

        PathFindingList open = new PathFindingList();
        open.add(startR);
        PathFindingList closed = new PathFindingList();

        NodeRecord current = new NodeRecord();
        while (open.length() > 0)
        {
            current = open.smallElement();

            if (current.node == goal)
            {
                break;
            }

            List<Connection> connections = graph.getConnections(current.node);

            foreach (Connection connection in connections)
            {
                Node endN = connection.getToNode();
                float endNodeCost = current.currentCost + connection.getCost();

                NodeRecord endNodeRecord = new NodeRecord();

                if (closed.contains(endN))
                {
                    continue;
                }    

                else if (open.contains(endN))
                {
                    endNodeRecord = open.find(endN);
                    if (endNodeRecord != null && endNodeRecord.currentCost < endNodeCost)
                    {
                        continue;
                    }
                }

                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endN;
                }

                endNodeRecord.currentCost = endNodeCost;
                endNodeRecord.connection = connection;

                if (!open.contains(endN))
                {
                    open.add(endNodeRecord);
                }
            }

            open.remove(current);
            closed.add(current);
        }

        if (current.node != goal)
        {
            return null;
        }

        else
        {
            List<Connection> path = new List<Connection>();

            while (current.node != start)
            {
                path.Add(current.connection);
                Node fromN = current.connection.getFromNode();
                current = closed.find(fromN);
            }

            path.Reverse();
            return path;
        }
    }
}
