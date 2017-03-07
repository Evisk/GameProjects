using UnityEngine;
using System.Collections;

public class Event  {

   public GameObject eventObject;
   public string Name;

    public Event()
    {


    }

   public  Event(GameObject _eventObject,string _eventName) {

       this.eventObject = _eventObject;
       this.Name = _eventName;

    }




}
