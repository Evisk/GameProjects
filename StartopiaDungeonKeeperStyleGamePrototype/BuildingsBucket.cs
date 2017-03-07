using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsBucket : MonoBehaviour {

    public GameObject RoomPanel;
    public struct BuildingBP
    {
        public int[,] layout;
        public GameObject buildingPrefab;

        public BuildingBP(int[,] _layout, GameObject _buildingPrefab)
        {
            layout = _layout;
            buildingPrefab = _buildingPrefab;
        }
    }
    public struct RoomBP
    {
        public GameObject wall, wallCorner, door;

        public RoomBP(GameObject _wall, GameObject _wallCorner, GameObject _door)
        {
            wall = _wall;
            wallCorner = _wallCorner;
            door = _door;
        }
    }

    static BuildingsBucket mInstance;
    public BuildingBP GeneralGoodShop;

    public List<BuildingBP> buildingBluePrints = new List<BuildingBP>();
    public List<RoomBP> roomPrefabs = new List<RoomBP>();

    public static BuildingsBucket Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject bb = new GameObject("BuildingsBucket");

                mInstance = bb.AddComponent<BuildingsBucket>();
            }
            return mInstance;
        }
    }

  


    public static int[,] RotateMatrixCounterClockwise(int[,] oldMatrix)
    {
        int[,] newMatrix = new int[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newMatrix;
    }

    public void IniBuildingBucket()
    {
        buildingBluePrints.Add(new BuildingBP(new int[5, 7] {{ 0 ,1 ,1 ,2 ,1 ,1,0},
                                                { 1, 1, 1, 0, 1 ,1,1},
                                                { 1, 3, 0, 0, 1 ,1,1},
                                                { 1, 1, 1, 1, 1 ,1,1},
                                                { 1, 1, 1, 1, 1 ,1,1}},
                                                Resources.Load<GameObject>("Prefabs/Shop")));


        roomPrefabs.Add(new RoomBP(Resources.Load<GameObject>("Prefabs/TestRoom/roomWall"), Resources.Load<GameObject>("Prefabs/TestRoom/roomCorner"), Resources.Load<GameObject>("Prefabs/TestRoom/roomDoor")));
        RoomPanel = Resources.Load<GameObject>("Prefabs/RoomPanel");


    }

    void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(transform.gameObject);

        

    }
}
