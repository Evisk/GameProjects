using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public GameObject BuildingBP;
    public GameObject BuildingBPHolo;
    public int BuildNumber;
    public bool buildingBuilding = false;
    bool noBuild = false;
    public BuildingsBucket.BuildingBP currentSelectedBuilding;
    public int buildingSizeX, buildingSizeY;
    public bool buildingRoom = false;
    public bool selectingPivotOne = true;
    public bool selectingPivotTwo = false;
    public bool placingDoor = false;
    GameObject previousObject = null;
    GameObject wallPreviousHolder = null;
    GameObject currentDoorObject = null;

    static BuildingManager mInstance;

    public BuildingsBucket.BuildingBP buildingBP;


    public GameObject roomPivotOne;
    public GameObject roomPivotTwo;
    public GameObject roomPivotTwoPrevious;
    public GameObject currentlyGridNodeSelected;

    public Vector2 pivotOne;
    public Vector2 pivotTwo;

    public GameObject roomWall, roomDoor, roomCorner;
    GameObject roomHolder;
    public List<GameObject> roomWalls = new List<GameObject>();


    public int buildNodeSizeXPlus, buildNodeSizeXMinus;
    public int buildNodeSizeYPlus, buildNodeSizeYMinus;

    public static BuildingManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject bm = new GameObject("BuildingManager");

                mInstance = bm.AddComponent<BuildingManager>();
            }
            return mInstance;
        }
    }

    public void BuildBuilding(BuildingsBucket.BuildingBP _building)
    {
        currentSelectedBuilding = _building;
        BuildingBP = (GameObject)Instantiate(currentSelectedBuilding.buildingPrefab);

        if (buildingBuilding != true)
            buildingBuilding = true;
    }

    public void BuildRoom(int i)
    {
        roomDoor = BuildingsBucket.Instance.roomPrefabs[i].door;
        roomWall = BuildingsBucket.Instance.roomPrefabs[i].wall;
        roomCorner = BuildingsBucket.Instance.roomPrefabs[i].wallCorner;
        roomHolder = new GameObject();
        roomHolder.name = "Room";
        roomHolder.AddComponent<Building>();
        buildingRoom = true;
    }

    public void FinishBuildingRoom()
    {
        GameManager.Instance.AddBuildingToQueue(roomHolder);
    }

    public void CalculateNodes()
    {
        buildingSizeY = currentSelectedBuilding.layout.GetLength(0);
        buildingSizeX = currentSelectedBuilding.layout.GetLength(1);
        if (buildingSizeY % 2 == 0)
        {
            buildNodeSizeYMinus = ((buildingSizeY) / 2) * -1;
            buildNodeSizeYPlus = (buildingSizeY / 2);
        }
        else
        {
            buildNodeSizeYMinus = ((buildingSizeY - 1) / 2) * -1;
            buildNodeSizeYPlus = (buildingSizeY - 1) / 2;
        }

        if (buildingSizeX % 2 == 0)
        {
            buildNodeSizeXMinus = ((buildingSizeX) / 2) * -1;
            buildNodeSizeXPlus = (buildingSizeX / 2);
        }
        else
        {
            buildNodeSizeXMinus = ((buildingSizeX - 1) / 2) * -1;
            buildNodeSizeXPlus = (buildingSizeX - 1) / 2;
        }
    }
    public void BuildingBuild()
    {
        CalculateNodes();

        foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
        {
            node.GetComponent<Renderer>().material.color = Color.white;
        }
        GameManager.Instance.toBeBuiltNodes.Clear();
        RaycastHit buildingRayHit;
        Ray buildingRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(buildingRay, out buildingRayHit, 100.0f))
        {
            BuildingBP.transform.position = buildingRayHit.transform.position;
            int a = 0;
            for (int x = buildNodeSizeXMinus; x <= buildNodeSizeXPlus; x++)
            {
                if ((int)BuildingBP.transform.position.x + x > 29 || (int)BuildingBP.transform.position.x + x < 0)
                {
                    a++;
                    break;
                }
                for (int y = buildNodeSizeYMinus; y <= buildNodeSizeYPlus; y++)
                {
                    if ((int)BuildingBP.transform.position.z + y > 29 || (int)BuildingBP.transform.position.z + y < 0)
                    {
                        a++;
                        break;
                    }
                    if (GameManager.Instance.gridNodes[(int)BuildingBP.transform.position.x + x, (int)BuildingBP.transform.position.z + y].nodeState == Node.NodeState.occupied)
                    {
                        GameManager.Instance.gridNodes[(int)BuildingBP.transform.position.x + x, (int)BuildingBP.transform.position.z + y].gridNodeObject.GetComponent<Renderer>().material.color = Color.red;
                        a++;
                    }
                    else
                    {
                        GameManager.Instance.gridNodes[(int)BuildingBP.transform.position.x + x, (int)BuildingBP.transform.position.z + y].gridNodeObject.GetComponent<Renderer>().material.color = Color.cyan;
                    }
                }
            }
            if (a > 0)
            {

                noBuild = true;
                BuildingBP.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);
            }
            else
            {
                noBuild = false;
                BuildingBP.GetComponent<Renderer>().material.color = new Color(0, 1, 1, 0.5f);
            }


            for (int x = buildNodeSizeXMinus; x <= buildNodeSizeXPlus; x++)
            {

                if ((int)BuildingBP.transform.position.x + x < GameManager.Instance.gridSize.x && (int)BuildingBP.transform.position.x + x >= 0)
                {
                    for (int y = buildNodeSizeYMinus; y <= buildNodeSizeYPlus; y++)
                    {
                        if ((int)BuildingBP.transform.position.z + y < GameManager.Instance.gridSize.y && (int)BuildingBP.transform.position.z + y >= 0)
                        {
                            GameManager.Instance.toBeBuiltNodes.Add(GameManager.Instance.gridNodes[(int)BuildingBP.transform.position.x + x, (int)BuildingBP.transform.position.z + y].gridNodeObject);
                        }
                    }
                }
            }
        }
    }



    void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(transform.gameObject);
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (buildingRoom == true)
            {
                buildingRoom = false;
                roomPivotTwo = null;
                pivotOne = new Vector2();
                pivotTwo = new Vector2();
                roomPivotOne = null;
                roomPivotTwoPrevious = null;

                foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
                {
                    node.GetComponent<Renderer>().material.color = Color.white;
                }
                GameManager.Instance.toBeBuiltNodes.Clear();

                foreach (GameObject wall in roomWalls)
                {
                    Destroy(wall);
                }
                roomWalls.Clear();
                Destroy(roomHolder);
                selectingPivotOne = true;
            }

            if (buildingBuilding)
            {

                buildingBuilding = false;
                foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
                {
                    node.GetComponent<Renderer>().material.color = Color.white;
                }
                GameManager.Instance.toBeBuiltNodes.Clear();
                Destroy(BuildingBP);

            }
        }

        if (buildingRoom)
        {
            RaycastHit roomRayHit;
            Ray roomRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            currentlyGridNodeSelected = null;


            if (Physics.Raycast(roomRay, out roomRayHit, 100.0f))
            {
                if (roomPivotOne != null)
                    roomPivotOne.GetComponent<Renderer>().material.color = Color.white;
                if (roomPivotTwo != null)
                    roomPivotTwo.GetComponent<Renderer>().material.color = Color.white;


                if (roomPivotTwo != null)
                    roomPivotTwo.GetComponent<Renderer>().material.color = Color.cyan;
                if (selectingPivotOne)
                {
                    roomPivotOne = roomRayHit.collider.gameObject;
                    if (roomPivotOne != null)
                        roomPivotOne.GetComponent<Renderer>().material.color = Color.cyan;
                }
                else if (selectingPivotTwo)
                {
                    roomPivotTwo = roomRayHit.collider.gameObject;
                    if (roomPivotTwoPrevious == null)
                        roomPivotTwoPrevious = roomRayHit.collider.gameObject;

                }
                else if (placingDoor)
                {
                    if (roomHolder.GetComponent<Building>().NodesOccupied.Contains(roomRayHit.collider.gameObject))
                    {
                        if (previousObject == null)
                        {
                            previousObject = roomRayHit.collider.gameObject;
                        }
                        else if (previousObject != roomRayHit.collider.gameObject)
                        {
                            previousObject.GetComponent<GridNodeObject>().occupiedObject.SetActive(true);
                            previousObject = roomRayHit.collider.gameObject;
                        }


                        currentDoorObject.transform.position = roomRayHit.collider.gameObject.transform.position;
                        currentDoorObject.transform.rotation = roomRayHit.collider.gameObject.GetComponent<GridNodeObject>().occupiedObject.transform.rotation;
                        roomRayHit.collider.gameObject.GetComponent<GridNodeObject>().occupiedObject.SetActive(false);


                    }
                }

                if (roomPivotTwo != null && selectingPivotTwo)
                {
                    //Efficiency
                    //Mother fucker
                    if (roomPivotTwo != roomPivotTwoPrevious)
                    {

                        //Clear up node colouring
                        //General maintenance
                        foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
                        {
                            node.GetComponent<Renderer>().material.color = Color.white;
                        }
                        //Reset nodelist to prep for new ones
                        GameManager.Instance.toBeBuiltNodes.Clear();
                        
                        roomPivotTwoPrevious = roomPivotTwo;

                        //Clean up previous walls
                        //To prep for new ones to be made
                        foreach (GameObject wall in roomWalls)
                        {
                            Destroy(wall);
                        }
                        roomWalls.Clear();

                        //Figure out which pivot is where
                        int differenceX = (int)(roomPivotOne.transform.position.x - roomPivotTwo.transform.position.x);
                        int differenceY = (int)(roomPivotOne.transform.position.z - roomPivotTwo.transform.position.z);
                        //And create custom pivots
                        //For flooding
                        if (differenceX > 0)
                        {
                            pivotOne.x = roomPivotTwo.transform.position.x;
                            pivotTwo.x = roomPivotOne.transform.position.x;
                        }
                        else if (differenceX < 0)
                        {
                            pivotTwo.x = roomPivotTwo.transform.position.x;
                            pivotOne.x = roomPivotOne.transform.position.x;
                        }

                        if (differenceY > 0)
                        {
                            pivotOne.y = roomPivotTwo.transform.position.z;
                            pivotTwo.y = roomPivotOne.transform.position.z;
                        }
                        else if (differenceY < 0)
                        {
                            pivotTwo.y = roomPivotTwo.transform.position.z;
                            pivotOne.y = roomPivotOne.transform.position.z;
                        }
                        

                        //Depending where a node is
                        //Spawn a wall or a corner in the proper rotation
                        //If not an edge node nothing is spawned
                        GameObject wallHolder = null;
                        for (int x = (int)pivotOne.x; x <= pivotTwo.x; x++)
                        {
                            for (int y = (int)pivotOne.y; y <= pivotTwo.y; y++)
                            { 
                                if (x == pivotOne.x && y == pivotOne.y)
                                {
                                    wallHolder = (GameObject)Instantiate(roomCorner);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 180);
                                    roomWalls.Add(wallHolder);
                                }
                                if (x == pivotOne.x && y == pivotTwo.y)
                                {
                                    wallHolder = (GameObject)Instantiate(roomCorner);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 270);
                                    roomWalls.Add(wallHolder);
                                }
                                if (x == pivotTwo.x && y == pivotOne.y)
                                {
                                    wallHolder = (GameObject)Instantiate(roomCorner);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 90);
                                    roomWalls.Add(wallHolder);
                                }
                                if (x == pivotTwo.x && y == pivotTwo.y)
                                {
                                    wallHolder = (GameObject)Instantiate(roomCorner);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 0);
                                    roomWalls.Add(wallHolder);
                                }
                                if (x == pivotOne.x && y != pivotTwo.y && y != pivotOne.y)
                                {
                                    wallHolder = (GameObject)Instantiate(roomWall);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 270);
                                    roomWalls.Add(wallHolder);
                                }

                                if (x == pivotTwo.x && y != pivotTwo.y && y != pivotOne.y)
                                {
                                    wallHolder = (GameObject)Instantiate(roomWall);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 90);
                                    roomWalls.Add(wallHolder);
                                }

                                if (y == pivotOne.y && x != pivotTwo.x && x != pivotOne.x)
                                {
                                    wallHolder = (GameObject)Instantiate(roomWall);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 180);
                                    roomWalls.Add(wallHolder);
                                }

                                if (y == pivotTwo.y && x != pivotTwo.x && x != pivotOne.x)
                                {
                                    wallHolder = (GameObject)Instantiate(roomWall);
                                    wallHolder.transform.position = new Vector3(x, 0, y);
                                    wallHolder.transform.rotation = Quaternion.Euler(270, 0, 0);
                                    roomWalls.Add(wallHolder);
                                }

                                GameManager.Instance.toBeBuiltNodes.Add(GameManager.Instance.gridNodes[x, y].gridNodeObject);
                                roomHolder.transform.position = new Vector3((pivotOne.x + pivotTwo.x) / 2, 0, (pivotOne.y + pivotTwo.y) / 2);
                                GameManager.Instance.gridNodes[x, y].gridNodeObject.GetComponent<GridNodeObject>().occupiedObject = wallHolder;
                                
                            }
                        }

                    }
                }
                //Button Click functionality
                if (Input.GetMouseButtonDown(0) && buildingRoom == true)
                {
                    //We start off with selecting pivotOne
                    //This moves us to selecting pivotTWo
                    if (selectingPivotOne == true)
                    {
                        selectingPivotOne = false;
                        selectingPivotTwo = true;
                    }

                    //If we click and we are on selecting pivotTwo
                    //We do some pre-setup
                    else if (selectingPivotTwo == true)
                    {

                        
                    //Empty flooded nodes of residual objects
                        for (int x = (int)pivotOne.x + 1; x < pivotTwo.x; x++)
                        {
                            for (int y = (int)pivotOne.y + 1; y < pivotTwo.y; y++)
                            {
                                GameManager.Instance.gridNodes[x, y].gridNodeObject.GetComponent<GridNodeObject>().occupiedObject = null;
                            }
                        }
                        //Set node status
                        //NEEDS WORK
                        //Wall nodes need to be set as occupied
                        //Door holding nodes need to be set as doors
                        //inside needs to be setup as "Special"
                        //Special need to be walkable but also unique to each room
                        //So specific inside items can be placed only in the correct rooms
                        foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
                        {
                            GameManager.Instance.getNodeFromWorldPos(node).nodeState = Node.NodeState.occupied;
                            roomHolder.GetComponent<Building>().NodesOccupied.Add(node);
                        }
                        //Mostly just tyding code
                        //Setting their parent doesn't do much
                        //For now
                        foreach (GameObject wall in roomWalls)
                        {
                            wall.transform.SetParent(roomHolder.transform);
                        }
                        //General Clean up
                        //Room size is set can prepare it for building
                        roomPivotTwo = null;
                        pivotOne = new Vector2();
                        pivotTwo = new Vector2();
                        roomPivotOne = null;
                        selectingPivotTwo = false;
                        placingDoor = true;
                        Instantiate(BuildingsBucket.Instance.RoomPanel, GameObject.Find("Canvas").transform,false);
                        currentDoorObject = Instantiate(roomDoor);
                        previousObject = null;



                    }
                    else if (placingDoor == true)
                    {
                        selectingPivotOne = true;
                        placingDoor = false;
                        buildingRoom = false;
                    }
                    roomWalls.Clear();

                }
                //Colour the nodes
                //After building is done 
                //Worker will change inside node colour and textures to room specific
                foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
                {
                    node.GetComponent<Renderer>().material.color = Color.cyan;
                }
            }

        }
        if (buildingBuilding)
        {
            BuildingBuild();

            if (Input.GetMouseButtonDown(0) && buildingBuilding == true)
            {
                if (noBuild)
                    return;

                foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
                {
                    if (GameManager.Instance.getNodeFromWorldPos(node).nodeState == Node.NodeState.occupied)
                    {
                        return;
                    }
                }
 

                for (int x = buildNodeSizeXMinus + (int)BuildingBP.transform.position.x; x <= (buildNodeSizeXPlus + (int)BuildingBP.transform.position.x); x++)
                {  
                    for (int y = buildNodeSizeYMinus + (int)BuildingBP.transform.position.z; y <= (buildNodeSizeYPlus + (int)BuildingBP.transform.position.z); y++)
                    {
                        int h = x - (buildNodeSizeXMinus + (int)BuildingBP.transform.position.x);
                        int j = y - (buildNodeSizeYMinus + (int)BuildingBP.transform.position.z);
                        Debug.Log(j);
                        switch (currentSelectedBuilding.layout[j, h])
                        {
                            case 0:
                                GameManager.Instance.gridNodes[x, y].nodeState = Node.NodeState.walkable;
                                break;
                            case 1:
                                GameManager.Instance.gridNodes[x, y].nodeState = Node.NodeState.occupied;
                                break;
                            case 2:
                                GameManager.Instance.gridNodes[x, y].nodeState = Node.NodeState.door;
                                break;
                            case 3:
                                GameManager.Instance.gridNodes[x, y].nodeState = Node.NodeState.interiorPoint;
                                break;
                        }
                    }
                }


                GameObject building = (GameObject)Instantiate(BuildingBP);
                foreach (GameObject node in GameManager.Instance.toBeBuiltNodes)
                {
                    building.GetComponent<Building>().NodesOccupied.Add(node);
                    
                }
                GameManager.Instance.toBeBuiltNodes.Clear();
                building.transform.position = BuildingBP.transform.position;
                building.transform.eulerAngles = new Vector3(
                                                                building.transform.eulerAngles.x,
                                                                BuildingBP.transform.eulerAngles.y,
                                                                 BuildingBP.transform.eulerAngles.z
                                                                            );
                GameManager.Instance.AddBuildingToQueue(building);
                Destroy(BuildingBP);
                buildingBuilding = false;

           
            }
       


            if (Input.GetKeyDown(KeyCode.R))
            {
                BuildingBP.transform.eulerAngles = new Vector3(
                                                                BuildingBP.transform.eulerAngles.x,
                                                                BuildingBP.transform.eulerAngles.y + 90,
                                                                 BuildingBP.transform.eulerAngles.z
                                                                            );
                currentSelectedBuilding.layout = BuildingsBucket.RotateMatrixCounterClockwise(currentSelectedBuilding.layout);
                CalculateNodes();

            }
        }
    }
}