using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    // moving from tutorial room to main room
    public Vector3 mainLocation;
    public GameObject cameraRig;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // started from unity (teleport button's onClick)
    public void Teleport()
    {
        Vector3 mainPosition = mainLocation;
        cameraRig.transform.position = mainPosition;
    }
}


