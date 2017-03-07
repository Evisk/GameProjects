using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GalaxyManager : MonoBehaviour {



 

	private static GalaxyManager _instance;
    private GameObject background;
    private int numberofSolarSystems = 20;
    private List<GameObject> solarSystemList = new List<GameObject>();
    private List<Event> currentEvents = new List<Event>();
    private GalaxySize galaxySize = GalaxySize.Tiny;
    private float totalBoundX;
    private float totalBoundY;
    private float minBoundX;
    private float maxBoundX;
    private float minBoundY;
    private float maxBoundY;
    private int ortoY;
    private int ortoX;

    public int cameraOrtBounds;




    private MeshRenderer backGroundMeshRender;

    public enum GalaxySize
    {
        Tiny,
        Small,
        Normal,
        Large,
        Huge
    }



    public enum parallexType
    {
        Horizontal,
        Vertical
    }

    public enum objectDirection{

        Both,
        Horizontal,
        Vertical

    }


    public static GalaxyManager Instance {

		get
		{ if (_instance == null)
			{
				GameObject gm = new GameObject("GalaxyManager");
				gm.AddComponent<GalaxyManager>();
			}
			return _instance;
		}
		
		
		
		
	}

    public bool isObjectMoving(Vector3 previousFramePos, Vector3 currentFramePos,objectDirection direction)
    {
        switch (direction)
        {
            case objectDirection.Both:
                if (previousFramePos == currentFramePos)
                    return false;
                return true;

            case objectDirection.Horizontal:
                if(previousFramePos.x == currentFramePos.x)
                    return false;
                return true;
            case objectDirection.Vertical:
                if (previousFramePos.y == currentFramePos.y)
                    return false;
                return true;
            default:
                return false;
        }  
    }

   public int getNumberOfSolarSystems()
    {
        return numberofSolarSystems;
    }

    public GalaxySize getGalaxySize()
    {
        return galaxySize;
    }
    public void increaseGalaxySize()
    {
        if ((int)galaxySize < 4)
        {
            galaxySize += 1;
        }
        else
        {
            galaxySize = (GalaxySize)0;
        }
        setGalaxySelectorText();
    }

    public void decreaseGalaxySize()
    {
        if ((int)galaxySize > 0)
        {
            galaxySize -= 1;
        }
        else
        {
            galaxySize = (GalaxySize)4;
        }
        setGalaxySelectorText();
    }

    public void setGalaxySelectorText()
    {
        ResoBucket.Instance.getGalaxyTextObject().text = getGalaxySize().ToString();
    }

    public void setGalaxySize(GalaxySize i)
    {
        galaxySize = i;
    }

    public List<GameObject> getSolarSystemList()
    {
        return solarSystemList;
    }

    public int getMinBoundAndOrt(string coord)
    {
        switch (coord)
        {
            case "x":
                return (int)(minBoundX + (ortoX/2));
            case "y":
                return (int)(minBoundY + (ortoY/2));
            default:
                return 0;
        }
    }

    public int getMaxBoundAndOrt(string coord)
    {
        switch (coord)
        {
            case "x":
                return (int)(maxBoundX - (ortoX/2));
            case "y":
                return (int)(maxBoundY - (ortoY/2));
            default:
                return 0;
        }
    }

    public void resetOrtoGraphic()
    {
        ortoY = (int)(Camera.main.orthographicSize * 2.0f);
        ortoX = (int)(ortoY * Screen.width / Screen.height);
    }


    public void createBackGround()
    {
        
        background = Instantiate(ResoBucket.Instance.getBackGroundPrefab());
        resetBackGroundSize();
        background.transform.SetParent(Camera.main.transform);
        backGroundMeshRender = background.GetComponent<MeshRenderer>();
    }

    public void resetBackGroundSize()
    {
        float y = Camera.main.orthographicSize * 2.0f;
        float x = y * Screen.width / Screen.height;
        background.transform.localScale = new Vector3(x, y, 1);
        background.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
    }

    public void parallexBackGround(parallexType type, float i , bool shouldParallax)
    {
        if (shouldParallax)
        {
            //float percentParallaxX = backGroundMeshRender.bounds.size.x*0.1f;
            //float percentParallaxY = backGroundMeshRender.bounds.size.y * 0.1f;
            Vector2 currentOffest = backGroundMeshRender.sharedMaterial.GetTextureOffset("_MainTex");
            switch (type)
            {
                case parallexType.Vertical:
                    backGroundMeshRender.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(currentOffest.x, currentOffest.y + i * Time.deltaTime));
                    break;
                case parallexType.Horizontal:
                    backGroundMeshRender.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(currentOffest.x + i * Time.deltaTime, currentOffest.y));
                    break;
            }
        }

    }

    //Randomy Generate a number of planets in a selected area
    public void GenerateSolarSystems(GalaxySize galaxySize)
    {

        float offset = 7.5f;
        switch (galaxySize)
        {
            case GalaxySize.Tiny:
                numberofSolarSystems = 20;
                totalBoundX = ortoX;
                totalBoundY = ortoY;
                break;
            case GalaxySize.Small:
                numberofSolarSystems = 50;
                totalBoundX = ortoX + (ortoX*0.5f);
                totalBoundY = ortoY + (ortoY * 0.5f);
                break;
            case GalaxySize.Normal:
                numberofSolarSystems = 100;
                totalBoundX = numberofSolarSystems * ResoBucket.Instance.getStarPrefab().GetComponent<SpriteRenderer>().bounds.extents.x + ortoX;
                totalBoundY = (numberofSolarSystems * ResoBucket.Instance.getStarPrefab().GetComponent<SpriteRenderer>().bounds.extents.y) * 0.6f + ortoY;
                break;
            case GalaxySize.Large:
                numberofSolarSystems = 200;
                totalBoundX = numberofSolarSystems * ResoBucket.Instance.getStarPrefab().GetComponent<SpriteRenderer>().bounds.extents.x;
                totalBoundY = (numberofSolarSystems * ResoBucket.Instance.getStarPrefab().GetComponent<SpriteRenderer>().bounds.extents.y) * 0.6f;
                break;
            case GalaxySize.Huge:
                numberofSolarSystems = 300;
                totalBoundX = numberofSolarSystems * ResoBucket.Instance.getStarPrefab().GetComponent<SpriteRenderer>().bounds.extents.x;
                totalBoundY = (numberofSolarSystems * ResoBucket.Instance.getStarPrefab().GetComponent<SpriteRenderer>().bounds.extents.y)*0.6f;
                break;
        }
        Debug.Log("Bound x " + totalBoundX + "Bound y " + totalBoundY + "Aspect Ratio " + totalBoundX/ totalBoundY);

        minBoundX = totalBoundX / -2;
        maxBoundX = totalBoundX / 2;
        minBoundY = totalBoundY / -2;
        maxBoundY = totalBoundY / 2;
        //offset so planets are alwys in the bounds
        
        int numberOfCoreSystems = (int)(numberofSolarSystems * 0.3f);
        int numberOfRimSystems = (int)(numberofSolarSystems * 0.5f);
        int numberOfFringeSystems = (int)(numberofSolarSystems * 0.2f);

        float coreWorldMaxBoundX = maxBoundX * 0.4f;
        float coreWorldMaxBoundY = maxBoundY * 0.4f;
        float coreWorldMinBoundX = minBoundX * 0.4f;
        float coreWorldMinBoundY = minBoundY * 0.4f;

       
        for (int i = 0; i < numberOfCoreSystems; i++)
        {
            bool minDistance = true;

            while (minDistance)
            {
                Vector3 pos = new Vector3(Random.Range(coreWorldMinBoundX, coreWorldMaxBoundX), Random.Range(coreWorldMinBoundY, coreWorldMaxBoundY), -2);
                bool distanceOK = true;

                for (int s = 0; s < solarSystemList.Count; s++)
                {
                    if (Vector3.Distance(solarSystemList[s].transform.position, pos) < offset)
                    {
                        distanceOK = false;
                    }
                }
                if (distanceOK)
                {
                    if (Mathf.Pow(pos.x, 2) / Mathf.Pow(coreWorldMaxBoundX, 2) +
                    Mathf.Pow(pos.y, 2) / Mathf.Pow(coreWorldMaxBoundY, 2) <= 1)
                    {

                        solarSystemList.Add((GameObject)Instantiate(ResoBucket.Instance.getStarPrefab(), pos, Quaternion.identity));
                        solarSystemList[i].transform.SetParent(GameObject.Find("Galaxy").GetComponent<Transform>());
                        solarSystemList[i].transform.name = "Core_System_" + i;
                        minDistance = false;
                    }
                }
            }
        }

        float rimWorldMaxBoundX = maxBoundX * 0.7f;
        float rimWorldMaxBoundY = maxBoundY * 0.7f;
        float rimWorldMinBoundX = minBoundX * 0.7f;
        float rimWorldMinBoundY = minBoundY * 0.7f;

        int hackyTomato = solarSystemList.Count -1;
        for (int f = 0; f < numberOfRimSystems; f++)
        {

            bool minDistance = true;

            while (minDistance)
            {
                Vector3 pos = new Vector3(Random.Range(rimWorldMinBoundX, rimWorldMaxBoundX), Random.Range(rimWorldMinBoundY, rimWorldMaxBoundY), -2);
                bool distanceOK = true;

                for (int s = 0; s < solarSystemList.Count; s++)
                {
                    if (Vector3.Distance(solarSystemList[s].transform.position, pos) < offset)
                    {
                        distanceOK = false;
                    }
                }
                if (distanceOK)
                {
                    if (Mathf.Pow(pos.x, 2) / Mathf.Pow(rimWorldMaxBoundX, 2) +
                    Mathf.Pow(pos.y, 2) / Mathf.Pow(rimWorldMaxBoundY, 2) <= 1)
                    {
                        if (Mathf.Pow(pos.x, 2) / Mathf.Pow(coreWorldMaxBoundX, 2) +
                    Mathf.Pow(pos.y, 2) / Mathf.Pow(coreWorldMaxBoundY, 2) > 1)
                        {

                            solarSystemList.Add((GameObject)Instantiate(ResoBucket.Instance.getStarPrefab(), pos, Quaternion.identity));
                            solarSystemList[f + hackyTomato].transform.SetParent(GameObject.Find("Galaxy").GetComponent<Transform>());
                            solarSystemList[f + hackyTomato].transform.name = "Rim_System_" + f;
                            minDistance = false;
                        }
                    }
                }
            }
        }

        float fringeWorldMaxBoundX = maxBoundX;
        float fringeWorldMaxBoundY = maxBoundY;
        float fringeWorldMinBoundX = minBoundX;
        float fringeWorldMinBoundY = minBoundY;

        hackyTomato = solarSystemList.Count -1;
        for (int f = 0; f < numberOfFringeSystems; f++)
        {

            bool minDistance = true;

            while (minDistance)
            {
                Vector3 pos = new Vector3(Random.Range(fringeWorldMinBoundX, fringeWorldMaxBoundX), Random.Range(fringeWorldMinBoundY, fringeWorldMaxBoundY), -2);
                bool distanceOK = true;

                for (int s = 0; s < solarSystemList.Count; s++) {
                    if (Vector3.Distance(solarSystemList[s].transform.position,pos) < offset)
                    {
                        distanceOK = false;
                    }
                        }
                if (distanceOK)
                {
                    if (Mathf.Pow(pos.x, 2) / Mathf.Pow(rimWorldMaxBoundX, 2) +
                    Mathf.Pow(pos.y, 2) / Mathf.Pow(rimWorldMaxBoundY, 2) > 1)
                    {
                        if (Mathf.Pow(pos.x, 2) / Mathf.Pow(coreWorldMaxBoundX, 2) +
                    Mathf.Pow(pos.y, 2) / Mathf.Pow(coreWorldMaxBoundY, 2) > 1)
                        {
                            if (Mathf.Pow(pos.x, 2) / Mathf.Pow(fringeWorldMaxBoundX, 2) +
                   Mathf.Pow(pos.y, 2) / Mathf.Pow(fringeWorldMaxBoundY, 2) <= 1)
                            {

                                solarSystemList.Add((GameObject)Instantiate(ResoBucket.Instance.getStarPrefab(), pos, Quaternion.identity));
                                solarSystemList[f + hackyTomato].transform.SetParent(GameObject.Find("Galaxy").GetComponent<Transform>());
                                solarSystemList[f + hackyTomato].transform.name = "Fringe_System_" + f;
                                minDistance = false;
                            }
                        }
                    }
                }
            }
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    //Both planets add each other as neighbors
    void addNeighbor(GameObject planet, GameObject neighbor)
    {
        //Bool which determines if we add the neighbor or not
        bool haveNeighbor = false;
        //If the current planet doesn't have any neighbors then we automatically add the neighbor
        if (planet.GetComponent<starSystem>().getNeighborCount() == 0)
        {
            planet.GetComponent<starSystem>().addNeighbor(neighbor);
        }
        else
        {
            //Else we check if the planet's neighbor list contains this neighbor
            for (int i = 0; i < planet.GetComponent<starSystem>().getNeighborCount(); i++)
            {
                //If it does we change the bool;
                if (planet.GetComponent<starSystem>().getNeighbors()[i] == neighbor)
                {
                    haveNeighbor = true;
                }


            }
            //If bool is false add the neighbor do nothing if true
            if (haveNeighbor == false)
            {
                planet.GetComponent<starSystem>().addNeighbor(neighbor);
            }
        }

        //Reset the bool
        haveNeighbor = false;
        //Then we do the same for the neighbor
        if (neighbor.GetComponent<starSystem>().getNeighborCount() == 0)
        {
            neighbor.GetComponent<starSystem>().addNeighbor(planet);
        }
        else
        {

            for (int j = 0; j < neighbor.GetComponent<starSystem>().getNeighborCount(); j++)
            {
                if (neighbor.GetComponent<starSystem>().getNeighbors()[j] == planet)
                {
                    haveNeighbor = true;
                }
            }

            if (haveNeighbor == false)
            {
                neighbor.GetComponent<starSystem>().addNeighbor(planet);
            }

        }
    }
    //Debug/Visual Player Aid function
    //Used when your mouse enters a planet gameobject to create lines between the planet and it's neighbors
    public void DrawLanes()
    {
        for (int i = 0; i < numberofSolarSystems; i++)
        {
            for (int j = 0; j < solarSystemList[i].GetComponent<starSystem>().getNeighborCount(); j++)
            {
                GameObject Lane = new GameObject();
                Lane.name = "Lane";
                Lane.transform.SetParent(GameObject.Find("Lanes").transform);
                Lane.AddComponent<LineRenderer>();
                Lane.GetComponent<LineRenderer>().SetPosition(0, new Vector3(solarSystemList[i].transform.position.x, solarSystemList[i].transform.position.y, -1));
                Lane.GetComponent<LineRenderer>().SetPosition(1, new Vector3(solarSystemList[i].GetComponent<starSystem>().getNeighbors()[j].transform.position.x, solarSystemList[i].GetComponent<starSystem>().getNeighbors()[j].transform.position.y, -1));
                Lane.GetComponent<LineRenderer>().material = new Material(Shader.Find("Particles/Alpha Blended"));

                Lane.GetComponent<LineRenderer>().SetColors(Color.cyan, Color.cyan);
                Lane.GetComponent<LineRenderer>().SetWidth(0.2f, 0.2f);

            }

        }

    }
    //Find neightbors of each planet based on a minimum distance
    //Based on Prim's Algorithm
    public void FindNeighbors()
    {
        //Cycle through all the planet
        for (int a = 0; a < numberofSolarSystems; a++)
        {
            //Set/Reset minimal distance
            float minDistance = 0;
            //Currently selected planet
            GameObject currentSolarSystem = solarSystemList[a];
            //Currently selected neighbor
            GameObject currentNeighbor = null;
            //Cycle through the remaining planets
            //int randNum = Random.RandomRange(0,neighbourHolder.Count-1);
            //addNeighbor(currentPlanet, neighbourHolder[randNum]);
            //neighbourHolder.RemoveAt(randNum);




            for (int b = 0 + a; b < numberofSolarSystems; b++)
            {
                if (solarSystemList[a].GetComponent<starSystem>().getNeighborCount() != 0)
                {
                    //Check if neighbor list already contains this planet
                    if (solarSystemList[b] == solarSystemList[a].GetComponent<starSystem>().getNeighbors()[0])
                    {
                      //  If it does skip to next one
                        continue;
                    }
                }
               // Check if the currently selected planet is the same as
                if (currentSolarSystem == solarSystemList[b])
                {
                    continue;
                }
                //Calculate distance between planet A and planet B
                float distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(solarSystemList[a].transform.position.x - solarSystemList[b].transform.position.x, 2) + Mathf.Pow(solarSystemList[a].transform.position.y - solarSystemList[b].transform.position.y, 2)));
               // If the distance is still at reset value then the current distance is set as minimum distance
               // And the current planet is set as the neighbor to add;
                if (minDistance == 0)
                {
                    minDistance = distance;
                    currentNeighbor = solarSystemList[b].gameObject;
                   // Else if the distance is smaller then the current minimum distance
                    //The distance becomes the new minimum distance
                   // And the planet becomes the neighbor to add
                }
                else if (distance < minDistance)
                {
                    minDistance = distance;
                    currentNeighbor = solarSystemList[b].gameObject;
                }
            }
           // If the neighbor to add is not null
            //Use addNeighbor function to add
            if (currentNeighbor != null)
                addNeighbor(currentSolarSystem, currentNeighbor);

        }

        foreach (GameObject planet in solarSystemList)
        {

            float distanceone = 0;
            GameObject planetone = planet;
            if (planet.GetComponent<starSystem>().getNeighborCount() < 2)
            {
                foreach (GameObject newneightbours in solarSystemList)
                {
                    if (newneightbours == planet)
                        continue;
                    if (planet.GetComponent<starSystem>().getNeighbors().Contains(newneightbours))
                        continue;

                    float distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(planet.transform.position.x - newneightbours.transform.position.x, 2) + Mathf.Pow(planet.transform.position.y - newneightbours.transform.position.y, 2)));

                    if (distanceone == 0)
                    {
                        distanceone = distance;
                        planetone = newneightbours;
                    }

                    if (distanceone > distance)
                    {
                        distanceone = distance;
                        planetone = newneightbours;
                    }


                }

            }

            planet.GetComponent<starSystem>().addNeighbor(planetone);
            planetone.GetComponent<starSystem>().addNeighbor(planet);
        }
    }
    //Planets remove each other as neighbors
    public void removeNeighbor(GameObject planet, GameObject neighbor)
    {


        foreach (GameObject item in planet.GetComponent<starSystem>().getNeighbors())
        {
            if (item == neighbor)
            {
                planet.GetComponent<starSystem>().removeNeighbor(item);
                break;
            }
        }
        foreach (GameObject item in neighbor.GetComponent<starSystem>().getNeighbors())
        {
            if (item == planet)
            {
                neighbor.GetComponent<starSystem>().removeNeighbor(item);
                break;
            }
        }
    }



    //As travel in the game is intended to be between planets and their neighbors
    //And the FindNeighbor() function created combinations where a path between a planet and it's neighbor
    //was either really close or through another planet
    //This function was made to fix that problem
    public void AdjustNeightbor()
    {
        //This is the planet we are currently on
        //Can be either the startingPlanet or the betweenNeighbor
        GameObject currentSolarSystem;
        //The neighbor we are trying to reach
        GameObject targetNeighbor;
        //The planet object between the currentPlanet and targetNeighbor
        GameObject betweenNeighbor;
        //This is the planet from which we being
        GameObject startingSolarSystem;
        //List of the neighbors we will be removing
        List<GameObject> neightborsToRemove = new List<GameObject>();
        //List of the neighbors we will be adding
        List<GameObject> neightborsToAdd = new List<GameObject>();




        for (int x = 0; x < numberofSolarSystems; x++)
        {
            //Clean up the lists from previous iteration
            neightborsToAdd.Clear();
            neightborsToRemove.Clear();
            //Select which planet we are going to adjust neighbors
            startingSolarSystem = solarSystemList[x];
            for (int y = 0; y < startingSolarSystem.GetComponent<starSystem>().getNeighborCount(); y++)
            {

                currentSolarSystem = startingSolarSystem;
                targetNeighbor = currentSolarSystem.GetComponent<starSystem>().getNeighbors()[y];
                bool isCurrentNeighbor = true;


                while (isCurrentNeighbor)
                {
                    //Set current planet to ignore raycasting
                    currentSolarSystem.layer = 2;
                    //Get direction vector
                    Vector3 direction = targetNeighbor.transform.position - currentSolarSystem.transform.position;
                    //Raycast in the direction
                    RaycastHit2D hit = Physics2D.CircleCast(currentSolarSystem.transform.position, 4f, direction);
                    //If we don't hit anything return
                    //If this is called then something went wrong
                    if (hit == false)
                    {
                        return;

                    }
                    //If for some reason we hit ourselves end the loop
                    if (hit.collider.gameObject == startingSolarSystem)
                    {
                        isCurrentNeighbor = false;


                    }
                    //If we hit the target neighbor end the loop
                    //And make it so the current planet can be raycasted
                    if (hit.collider.gameObject == targetNeighbor)
                    {
                        currentSolarSystem.layer = 0;


                        isCurrentNeighbor = false;


                    }
                    //If hit another planet that is not the target neighbor
                    if (hit.collider.gameObject != targetNeighbor && hit.collider.gameObject != false & hit.collider.gameObject != startingSolarSystem)
                    {
                        //That planet now becomes the betweenNeighbor
                        betweenNeighbor = hit.collider.gameObject;
                        //If we are still at the starting planet
                        //Add the between neighbor to the the add neighbor list
                        //Add the target neighbor to the remove neighbor list

                        if (currentSolarSystem == startingSolarSystem)
                        {
                            neightborsToAdd.Add(betweenNeighbor);
                            neightborsToRemove.Add(targetNeighbor);
                        }
                        //If we are not at the starting planet
                        //The current planet and the between neighbor add each other as neighbors
                        if (currentSolarSystem != startingSolarSystem)
                        {
                            addNeighbor(currentSolarSystem, betweenNeighbor);


                        }
                        //Allow the current planet to be raycasted 
                        currentSolarSystem.layer = 0;
                        //Set the current planet to the be between neighbor
                        currentSolarSystem = betweenNeighbor;


                    }
                }
            }
            //Add and remove all neighbor from the lists of the starting planet
            foreach (GameObject item in neightborsToAdd)
            {
                addNeighbor(startingSolarSystem, item);
            }
            foreach (GameObject item in neightborsToRemove)
            {
                startingSolarSystem.GetComponent<starSystem>().removeNeighbor(item);

            }
        }

        //foreach (GameObject planet in GalaxyManager.Instance.solarSystemList)
        //{

        //    float distanceone = 0;
        //    GameObject planetone = planet;
        //    if (planet.GetComponent<Planet>().Neightbors.Count < 2)
        //    {
        //        foreach (GameObject newneightbours in GalaxyManager.Instance.solarSystemList)
        //        {
        //            if (newneightbours == planet)
        //                continue;
        //            if (planet.GetComponent<Planet>().Neightbors.Contains(newneightbours))
        //                continue;

        //            float distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(planet.transform.position.x - newneightbours.transform.position.x, 2) + Mathf.Pow(planet.transform.position.y - newneightbours.transform.position.y, 2)));

        //            if (distanceone == 0)
        //            {
        //                distanceone = distance;
        //                planetone = newneightbours;
        //            }

        //            if (distanceone > distance)
        //            {
        //                distanceone = distance;
        //                planetone = newneightbours;
        //            }


        //        }

        //    }

        //    planet.GetComponent<Planet>().Neightbors.Add(planetone);
        //    planetone.GetComponent<Planet>().Neightbors.Add(planet);
        //}



    }





    
  
	void Awake(){


		_instance = this;
        ortoY = (int)(Camera.main.orthographicSize * 2.0f);
        ortoX = (int)(ortoY * Screen.width / Screen.height);

        cameraOrtBounds = (int)(totalBoundX / totalBoundY);


        DontDestroyOnLoad (gameObject);

 
	}


}
