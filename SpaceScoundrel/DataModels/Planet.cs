using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Planet : MonoBehaviour {
	public float PlanetRotateSpeed = -1f;
	public List<GameObject> Neightbors;
	public List<GameObject> Moons;
	public int stockTake = 100;
    public int planetNumber;

  public  enum planetType
    {
        Volcanic,
        Oceanic,
        Desert,
        Agrarian,
        Ecumenopolis
    }

   public  enum planetPopulationMajority
    {
        Mixed,
        FelineTypes,
        EnergyTypes,
        KroganTypes,
        GoblinTypes,
        Humans,
        Insectoids
    }


    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }


    //public void AddEvent()
    //{
    //    GalaxyManager.Instance.currentEvents.Add(new Event(this.gameObject, "War"));
    //}


  public   planetType type;
  public  planetPopulationMajority popMajority;
 int eventChance;


	void Awake () {


        type = GetRandomEnum<planetType>();
        planetNumber = (int)type;
        popMajority = GetRandomEnum<planetPopulationMajority>();
        GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Planets/planet" + planetNumber, typeof(Sprite)) as Sprite;
        eventChance = (int)Random.Range(1, 100);
        if(eventChance > 75)
        {
            //AddEvent();
        }
       

    }


   

    public void OnMouseDown()
    {


    }

			          

       public  void OnMouseEnter(){
      
    }


	public void OnMouseExit(){
      
    }

}
