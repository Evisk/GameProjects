using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarPath : MonoBehaviour
{
	private static StarPath _instance;
	
	public static StarPath Instance {
		
		get
		{ if (_instance == null)
			{
				GameObject sp = new GameObject("StarPath");
				sp.AddComponent<StarPath>();
			}
			return _instance;
		}
		
		
		
		
	}
	//Return distance between two gameObjects
	public float GetDistance(GameObject one, GameObject two){
		return Mathf.Abs (Mathf.Sqrt (Mathf.Pow (two.transform.position.x - one.transform.position.x,2) + Mathf.Pow (two.transform.position.y - one.transform.position.y,2)));

	}
	//Returns a List of GameObjects which Players or NPCs can use to travers from the start Planet to the end Planet
	//Based on A Star Path Algorithm
	public List<GameObject> FindPath(GameObject start, GameObject end){
	
		List<GameObject> currentPath = new List<GameObject> ();
		currentPath.Clear ();
		currentPath.Add (start);

		GameObject currentPlanet = start;

		Debug.Log (start.name);
		Debug.Log (end.name);
		//List of planet that have been checked
		List<GameObject> checkedPlanets = new List<GameObject> ();
		bool noPath = true;
		//Add the starting planet to the beginign of the path
		checkedPlanets.Add (start);

		while (noPath) {
			float shortestDistance = 0;
			GameObject closestNeighbor = null;
			//Check which Neighbor has the closest distance to the end Planet
			foreach (GameObject item in currentPlanet.GetComponent<Planet>().Neightbors) {
				//If one of our Neighbors is the end Planet add to path and return path
				if (item == end) {
					currentPath.Add (item);
                    return currentPath;
				}
				//If we have already checked this Planet before move to next planet
				if(checkedPlanets.Contains(item)){
					continue;
				}
				float tempDistance = GetDistance (item, end);
				//If shortestDistance is at it's default value
				//Make tempDistance the new shortDistance
				//And add the Neighbor as the closestNeighbor to the end Planet
				if (shortestDistance == 0) {
					shortestDistance = tempDistance;
					closestNeighbor = item;
				}
				//Check distances if the Neighbor is closer to the end Planet it becomes the closesNeighbor
				if (tempDistance < shortestDistance) {
					closestNeighbor = item;

				}


			}	
			//If we've checked all the neighbors
			//Then closesNeighbor will return null
			//If it does move back one Planet and check again
			//Else add the closestNeighbor to the checked list
			//And set it as the currentPlanet
				if(closestNeighbor == null){
				currentPath.Remove(currentPlanet);
				currentPath.TrimExcess();
				currentPlanet = currentPath[currentPath.Count-1];
				}else if(closestNeighbor != null){
				checkedPlanets.Add(closestNeighbor);
				currentPath.Add(closestNeighbor);
				currentPlanet = closestNeighbor;
				}
		
		}
        //Return Path
		return currentPath;
	}
	void Awake(){
		_instance = this;
		DontDestroyOnLoad (gameObject);
}
}


