using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {

    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;
    public Material material6;

    public Renderer rend1;
    public Renderer rend2;
    public Renderer rend3;
    public Renderer rend4;
    public Renderer rend5;
    public Renderer rend6;

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
        material3 = rend3.material;
        material4 = rend4.material;
        material5 = rend5.material;
        material6 = rend6.material;
        
        rend1.enabled = true;
        rend2.enabled = true;
        rend3.enabled = true;
        rend4.enabled = true;
        rend5.enabled = true;
        rend6.enabled = true;
        
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
        rend6.enabled = true;
    }

    void TurnOff()
    {
        rend1.enabled = false;
        rend2.enabled = false;
        rend3.enabled = false;
        rend4.enabled = false;
        rend5.enabled = false;
        rend6.enabled = false;
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
