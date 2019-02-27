using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowController : MonoBehaviour
{

    public Material material1;

    public Renderer rend1;

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

        rend1.enabled = true;
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
    }

    void TurnOff()
    {
        rend1.enabled = false;
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
