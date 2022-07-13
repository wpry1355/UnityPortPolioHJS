using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isWall;
    public Node ParentsNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public float x, y, G, H;
    public float F { get { return G + H; } }
}

public class SC_AINavigation
{
    public Vector3 m_NodeArrayStart, m_NodeArrayEnd;
    public Vector3 m_StartPos, m_TargetPos;
    public List<Node> FinalNodeList;
    public int sizeX, sizeY;
    public Node[,] NodeArray;
    private int adRange = 100;
    Node StartNode = new Node();
    Node TargetNode = new Node();
    Node DstNode = new Node();
    Node CurrentNode;
    List<Node> OpenList, ClosedList;
    public void init(Vector3 StartPos, Vector3 TargetPos)
    {
        m_StartPos = StartPos;
        m_TargetPos = TargetPos;
        StartNode.x = StartPos.x;
        StartNode.y = StartPos.y;
        TargetNode.x = TargetPos.x;
        TargetNode.y = TargetPos.y;
        m_NodeArrayStart.x = m_StartPos.x;
        m_NodeArrayStart.y = m_StartPos.y;
    }

    public Node PathFinding()
    {

        sizeX = adRange;
        sizeY = adRange;
        NodeArray = new Node[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                Collider2D[] initNodeTag = Physics2D.OverlapBoxAll(new Vector3(m_NodeArrayStart.x - adRange / 2 + i, m_NodeArrayStart.y - adRange / 2 + j, 0), new Vector2(0.5f, 0.5f), 0);
                foreach (Collider2D collider in initNodeTag)
                {
                    if (collider.tag == "Wall")
                        isWall = true;
                    else if (collider.tag == "Enemy")
                        isWall = true;
                    else if (collider.tag == "EnemyArea")
                        isWall = true;
                }
                Node Tmp = new Node();
                Tmp.x = m_NodeArrayStart.x - adRange / 2 + i;
                Tmp.y = m_NodeArrayStart.y - adRange / 2 + j;
                Tmp.isWall = isWall;
                NodeArray[i, j] = Tmp;
            }
        }

        DstNode = FindNode(NodeArray, new Vector2(m_TargetPos.x, m_TargetPos.y));

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();
        int Count1 = 0;

        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurrentNode.F && OpenList[i].H < CurrentNode.H)
                    CurrentNode = OpenList[i];
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);      

            if (CurrentNode.x == DstNode.x && CurrentNode.y == DstNode.y)
            {
                Node TargetCurNode = DstNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    if (TargetCurNode.ParentsNode != null)
                    {
                        TargetCurNode = TargetCurNode.ParentsNode;
                    }
                    else
                        return null;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                return FinalNodeList[1];
            }

           
            OpenListAdd(CurrentNode.x, CurrentNode.y + 1);
            OpenListAdd(CurrentNode.x, CurrentNode.y - 1);
            OpenListAdd(CurrentNode.x + 1, CurrentNode.y);
            OpenListAdd(CurrentNode.x - 1, CurrentNode.y);
            Count1++;
        }
        return null;
    }


    void OpenListAdd(float checkX, float CheckY)
    {
        Vector2 AddCheckVector = new Vector2(checkX, CheckY);


         if (checkX >= m_NodeArrayStart.x - adRange / 2 && checkX <= m_NodeArrayEnd.x + adRange / 2 && CheckY >= m_NodeArrayStart.y - adRange / 2 && CheckY <= m_NodeArrayEnd.y + adRange / 2)
            if (FindNode(NodeArray, AddCheckVector) != null && FindNode(NodeArray, AddCheckVector).isWall == false)
                if (!ClosedList.Contains(FindNode(NodeArray, AddCheckVector)))
                {
                    Node NeighborNode = FindNode(NodeArray, AddCheckVector);
                    int MoveCost = (int)CurrentNode.G + 10;

                    if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
                    {
                        NeighborNode.G = MoveCost;
                        NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                        NeighborNode.ParentsNode = CurrentNode;

                        OpenList.Add(NeighborNode);
                    }
                }
    }
    private Node FindNode(Node[,] arrNode, Vector2 FindVector)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (arrNode[i, j].x == FindVector.x && arrNode[i, j].y == FindVector.y)
                {
                    return arrNode[i, j];
                }
            }
        }
        return null;
    }

}

