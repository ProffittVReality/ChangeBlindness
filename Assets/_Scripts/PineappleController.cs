using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineappleController : MonoBehaviour {

    public Material material1;
    public Material material2;

	// pineapples = stems up
    public Renderer rend1;
    public Renderer rend2;
	public Renderer rend3;
	public Renderer rend4;
	public Renderer rend5;

	// pineapples = stems down
	public Renderer rend6;
	public Renderer rend7;
	public Renderer rend8;
	public Renderer rend9;
	public Renderer rend10;

    public float switchTime;

    bool isOn = true;

    public Camera cam;

    public Collider objCollider1;

    private Plane[] planes;

    bool inView = true;


    // Use this for initialization
    void Start()
    {
        
        material1 = rend1.material;
        material2 = rend2.material;

        rend1.enabled = true;
        rend2.enabled = true;
		rend3.enabled = true;
		rend4.enabled = true;
		rend5.enabled = true;


		rend6.enabled = false;
		rend7.enabled = false;
		rend8.enabled = false;
		rend9.enabled = false;
		rend10.enabled = false;
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
		rend2.enabled = true;
		rend3.enabled = true;
		rend4.enabled = true;
		rend5.enabled = true;


		rend6.enabled = false;
		rend7.enabled = false;
		rend8.enabled = false;
		rend9.enabled = false;
		rend10.enabled = false;
    }

    void TurnOff()
    {
		rend1.enabled = false;
		rend2.enabled = false;
		rend3.enabled = false;
		rend4.enabled = false;
		rend5.enabled = false;


		rend6.enabled = true;
		rend7.enabled = true;
		rend8.enabled = true;
		rend9.enabled = true;
		rend10.enabled = true;
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
