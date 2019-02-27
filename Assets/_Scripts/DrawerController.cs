using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{

    public Material material;
    public Renderer rend;

    public float switchTime;

    bool isOn = true;

    public Camera cam;
    private Collider objCollider;
    private Plane[] planes;

    bool inView = true;


    // Use this for initialization
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();

        material = rend.material;
        rend.enabled = true;
        objCollider = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
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
        rend.enabled = true;
    }

    void TurnOff()
    {
        rend.enabled = false;
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
