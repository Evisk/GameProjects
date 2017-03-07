using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class starSystem : MonoBehaviour {
    [SerializeField]
    private List<GameObject> Neightbors = new List<GameObject>();
    [SerializeField]
    private List<GameObject> starLanes =  new List<GameObject>();

    public void addNeighbor(GameObject neighbor)
    {
        Neightbors.Add(neighbor);
    }

    public void removeNeighbor(GameObject neighbor)
    {
        Neightbors.Remove(neighbor);
    }

    public List<GameObject> getNeighbors()
    {
        return Neightbors;
    }

    public int getNeighborCount()
    {
        return Neightbors.Count;
    }

    public void addStarLane(GameObject lane)
    {
        starLanes.Add(lane);
    }

    public void removeStarLane(GameObject lane)
    {
        starLanes.Remove(lane);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
