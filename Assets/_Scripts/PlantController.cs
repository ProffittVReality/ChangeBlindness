using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour {

    public Material material1;
    public Material material2;

    public Renderer rend1; // pink
    public Renderer rend2; // yellow

    public float switchTime;

    bool isOn = true;

    public Camera cam;

    public Collider objCollider1;
	public Collider objCollider2;

    private Plane[] planes;

    bool inView = true;


    // Use this for initialization
    void Start()
    {
        
        material1 = rend1.material;
        material2 = rend2.material;      

        rend1.enabled = true;
        rend2.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(planes, objCollider1.bounds))
        {
            inView = true;
            CancelInvoke();
        }
        else
        {
            if (inView)
            {
                inView = false;
                InvokeRepeating("FlipOn", 0f, switchTime);
            }
        }
    }

    void TurnOn()
    {
        rend1.enabled = true;
        rend2.enabled = false;
    }

    void TurnOff()
    {
        rend1.enabled = false;
        rend2.enabled = true;
    }

    void FlipOn()
    {
        if (isOn)
        {
            TurnOff();
            isOn = false;
        }
        else
        {
            TurnOn();
            isOn = true;
        }
    }

}