using UnityEngine;
using System.Collections;

public class SteamVRInputTest : MonoBehaviour {

    [Header("Get Test Buttons Buttons")]
    public SteamVRInput.Controller Controller;

    [Header("Get TestButton")]
    public SteamVRInput.Button TestButton;

    [Header("Get Touch")]
    public SteamVRInput.Button TouchButton;





    [Header("Show Axis Buttons")]
    public SteamVRInput.Controller AxisController;
    public Vector2 trigger;
    public Vector2 axis;

    [Header("Get Cube Hookups")]
    public GameObject GetCube;
    public GameObject GetDownCube;
    public GameObject GetUpCube;
    [Header("Get Touch Cube Hookups")]
    public GameObject GetTouchCube;
    public GameObject GetTouchDownCube;
    public GameObject GetTouchUpCube;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //################################################
        // Get Examples
        //################################################
        if (SteamVRInput.GetUp(TestButton, Controller))
        {
            Debug.Log("Get Up Triggered");
            GetUpCube.GetComponent<Renderer>().material.color = RandomColor();
        }
        if (SteamVRInput.GetDown(TestButton, Controller))
        {
            Debug.Log("Get Down Triggered");
            GetDownCube.GetComponent<Renderer>().material.color = RandomColor();
        }
        if (SteamVRInput.Get(TestButton, Controller))
        {
            Debug.Log("Get Triggered");
            GetCube.GetComponent<Renderer>().material.color = RandomColor();
        }

        //#########################################################
        // Get Touch Examples - Note: this works best with TouchPad
        //#########################################################


        // Get Touch Up does not work with TouchPad Directions.
        //if (SteamVRInput.GetTouchUp(TouchButton, Controller))
        //{
        //    Debug.Log("Get Touch Up Triggered");
        //    GetTouchUpCube.GetComponent<Renderer>().material.color = RandomColor();
        //}

        if (SteamVRInput.GetTouchDown(TouchButton, Controller))
        {
            Debug.Log("Get Touch Down Triggered");
            GetTouchDownCube.GetComponent<Renderer>().material.color = RandomColor();
        }

        if (SteamVRInput.GetTouch(TouchButton, Controller))
        {
            Debug.Log("Get Touch Triggered");
            GetTouchCube.GetComponent<Renderer>().material.color = RandomColor();
        }

        //################################################
        // Haptic Strength
        //################################################
        axis = SteamVRInput.GetTouchPadAxis(AxisController);
        trigger = SteamVRInput.GetTriggerAxis(AxisController);
    }

    //################################################
    // Change Colors Quickly
    //################################################
    Color RandomColor()
    {
        Color holder;
        holder = new Color(Random.value, Random.value, Random.value);
        return holder; 
    }
}
