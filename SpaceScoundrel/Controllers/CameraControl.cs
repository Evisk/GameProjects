using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	[SerializeField]
    public int speed = 25;
    public bool inMap = false;
    public bool noDrag = true;
    private int theScreenWidth;
    private int theScreenHeight;
    private float Boundary = 3f;
    private Vector3 cameraPositionLastFrame;
    private bool shouldParallaxHorizontal;
    private bool shouldParallaxVertical;

    public void CenterOnLocation(Vector3 location)
    {
        transform.position = location;
      
    }

    public void changeZoom(float zoomChange)
    {
        //Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize + zoomChange * 10, 10);
        //Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 10, GalaxyManager.Instance.cameraOrtBounds);
        GalaxyManager.Instance.resetBackGroundSize();
        GalaxyManager.Instance.resetOrtoGraphic();
    }

	void Start () {
        theScreenWidth = Screen.width;
       theScreenHeight = Screen.height;
       CenterOnLocation(new Vector3(GameObject.Find("PlayerMap").transform.position.x, GameObject.Find("PlayerMap").transform.position.y, transform.position.z));
        cameraPositionLastFrame = Camera.main.transform.position;
    }


    void Update() {

        shouldParallaxHorizontal = GalaxyManager.Instance.isObjectMoving(cameraPositionLastFrame, Camera.main.transform.position,GalaxyManager.objectDirection.Horizontal);
        shouldParallaxVertical = GalaxyManager.Instance.isObjectMoving(cameraPositionLastFrame, Camera.main.transform.position, GalaxyManager.objectDirection.Vertical);
        cameraPositionLastFrame = Camera.main.transform.position;
        changeZoom(Input.GetAxis("Mouse ScrollWheel"));

        if (inMap)
        {
           
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
               
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Horizontal, 0.1f, shouldParallaxHorizontal);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Horizontal, -0.1f, shouldParallaxHorizontal);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Vertical, -0.1f, shouldParallaxVertical);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Vertical, 0.1f, shouldParallaxVertical);
            }
            

            if (Input.mousePosition.x > theScreenWidth - Boundary)
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Horizontal, 0.1f, shouldParallaxHorizontal);
            }
            if (Input.mousePosition.x < 0 + Boundary)
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Horizontal, -0.1f, shouldParallaxHorizontal);
            }
            if (Input.mousePosition.y > theScreenHeight - Boundary)
            {

                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Vertical, -0.1f, shouldParallaxVertical);
            }
            if (Input.mousePosition.y < 0 + Boundary)
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
                GalaxyManager.Instance.parallexBackGround(GalaxyManager.parallexType.Vertical, 0.1f, shouldParallaxVertical);
            }
        } else if (!inMap && noDrag)
        {
            transform.position = new Vector3(GameObject.Find("Player").transform.position.x,GameObject.Find("Player").transform.position.y,transform.position.z);

        }else if (!inMap && !noDrag)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, transform.position.z), 0.3f);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, GalaxyManager.Instance.getMinBoundAndOrt("x"), GalaxyManager.Instance.getMaxBoundAndOrt("x")), 
                                        Mathf.Clamp(transform.position.y, GalaxyManager.Instance.getMinBoundAndOrt("y"), GalaxyManager.Instance.getMaxBoundAndOrt("y")), 
                                        transform.position.z);
            
    }
	
	}
