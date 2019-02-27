using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RugColorController : MonoBehaviour {

    public Material material;
    public Renderer rend;
    public Color color1;
    public Color color2;

    public float switchTime;

    bool isColor1 = true;

    public Camera cam;
    public Collider objCollider;
    private Plane[] planes;

    bool inView = true;

    // Use this for initialization
    void Start()
    {      
        material = rend.material;    
        ChangeColor(color1);
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
                // object has just left view
                inView = false;
                InvokeRepeating("FlipColor", 0f, switchTime);
            }
        }
    }

    void ChangeColor(Color newColor)
    {
        material.color = newColor;
    }

    void FlipColor()
    {
        if (isColor1)
        {
            ChangeColor(color2);
            isColor1 = false;
        }
        else
        {
            ChangeColor(color1);
            isColor1 = true;
        }
    }

}
