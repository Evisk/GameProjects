using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {
    public bool random;
    public bool walking;
    public Node myNode;
    public List<Node> currentPath = new List<Node>();
    public Animator pawnAnimator;
    public Node getNodeFromWorldPos()
    {
        return GameManager.Instance.gridNodes[(int)transform.position.x, (int)transform.position.z];
    }

    public void Move()
    {
           

        float step = 3 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,new Vector3(currentPath[0].nodeX,
                                                                                transform.position.y
                                                                                , currentPath[0].nodeY), step);

        transform.LookAt(new Vector3(currentPath[0].nodeX,
                                     transform.position.y
                                   , currentPath[0].nodeY));

        
        if (transform.position.x == currentPath[0].nodeX && transform.position.z == currentPath[0].nodeY)
        {
            myNode = currentPath[0];
            currentPath.RemoveAt(0);
        }
        
           
    }
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (random && !walking)
        {
            currentPath = new List<Node>();
            currentPath = GameManager.Instance.FindPath(getNodeFromWorldPos(), GameManager.Instance.gridNodes[Random.Range(0, 30), Random.Range(0, 30)]);
            Debug.Log("trying new path generation");
        }
        if(currentPath.Count > 0)
        {
            walking = true;
        }else
        {
            walking = false;
        }
        if(walking && currentPath.Count > 0)
        {
            Move();
        }
	}
}
