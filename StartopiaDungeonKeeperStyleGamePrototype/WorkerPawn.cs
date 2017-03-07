using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerPawn : Pawn {

    public enum WorkerState
    {
        idle,
        working,
        building,
        cleaning
    }
    public GameObject buildingToBuild = null;
    public Material mat;
    public WorkerState workerState = WorkerState.idle;

    public int waitTime = 1000;

    public Node FindClosestNode(List<GameObject> _list)
    {
        Node closestNode = new Node();
        int distance = -1;
        foreach (GameObject node in _list)
        {
            foreach(Node neighbour in GameManager.Instance.getNodeFromWorldPos(node).Neighbours)
            {
                if (neighbour.nodeState != Node.NodeState.walkable)
                    continue;

                if(distance == -1)
                {
                    closestNode = neighbour;
                    distance = GameManager.Instance.GetDistance(getNodeFromWorldPos(), neighbour);
                }

                if(GameManager.Instance.GetDistance(getNodeFromWorldPos(), neighbour) < distance)
                {
                    closestNode = neighbour;
                    distance = GameManager.Instance.GetDistance(getNodeFromWorldPos(), neighbour);
                }
            }
        }



        return closestNode;
    }

    void Build()
    { 

        if (waitTime <= 0 && buildingToBuild != null)
        {
            waitTime = 1000;
        }

        if(waitTime > 0)
        {
            waitTime--;
        }
        if(buildingToBuild != null)
        transform.LookAt(new Vector3(buildingToBuild.transform.position.x,transform.position.y, buildingToBuild.transform.position.z));

        pawnAnimator.SetBool("deployArm", true);
        
        if(pawnAnimator.GetBool("isBuilding")== false)
        {
            pawnAnimator.SetBool("isBuilding", true);
        }

        if (waitTime == 0)
        {
            if (buildingToBuild != null)
            {
                if (buildingToBuild.GetComponent<Renderer>())
                {
                    buildingToBuild.GetComponent<Renderer>().material = mat;
                }else
                {
                    foreach (Transform child in buildingToBuild.transform)
                    {
                        child.gameObject.GetComponent<Renderer>().material = mat;
                    }
                }
                
                GameManager.Instance.Buildings.Add(buildingToBuild);
                buildingToBuild = null;
            }
            if(pawnAnimator.GetBool("deployArm") == true)
                pawnAnimator.SetBool("deployArm", false);
            if (pawnAnimator.GetBool("isBuilding") == true)
                pawnAnimator.SetBool("isBuilding", false);
            if(pawnAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleWorker"))
            workerState = WorkerState.idle;


        }
    }

    public void AssignBuilding (GameObject _building)
    {
        buildingToBuild = _building;
    }

    
    void Awake()
    {
        pawnAnimator = GetComponent<Animator>();
    }
    

	void Update()
    {

        if (workerState == WorkerState.idle && !GameManager.Instance.IdleWorkers.Contains(gameObject))
        {
            GameManager.Instance.AddIdleWorker(gameObject);
        }

        if (buildingToBuild != null && workerState == WorkerState.idle)
        {
            GameManager.Instance.RemoveIdleWorker(gameObject);
            currentPath = new List<Node>();
            currentPath = GameManager.Instance.FindPath(getNodeFromWorldPos(), FindClosestNode(buildingToBuild.GetComponent<Building>().NodesOccupied));
            workerState = WorkerState.building;
        }

        if (currentPath.Count > 0)
        {   
            walking = true;
        }
        else
        {
            walking = false;
        }
        if (workerState == WorkerState.building)
        {
            if(currentPath.Count > 0)
                Move();
            if (currentPath.Count == 0)
                Build();

        }


    }

}
