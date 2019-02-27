using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour {

    public ClockTimer clockTimer;

    public DigitalClock digitalClock;

    public int initialHoursValue;
    public int initialMinutesValue;

    public int changeHoursValue;
    public int changeMinutesValue;

    public float switchTime;

    bool isOn = true;

    public Camera cam;

    public Collider objCollider1;

    private Plane[] planes;

    bool inView = true;

    // Use this for initialization
    void Start () {
		
        digitalClock = clockTimer.digitalClock;

    }
	
	// Update is called once per frame
	void Update () {

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
        if (digitalClock.hoursDCV != null) digitalClock.hoursDCV.ChangeToTargetTime(initialHoursValue);
        if (digitalClock.minutesDCV != null) digitalClock.minutesDCV.ChangeToTargetTime(initialMinutesValue);
    }

    void TurnOff()
    {
        if (digitalClock.hoursDCV != null) digitalClock.hoursDCV.ChangeToTargetTime(changeHoursValue);
        if (digitalClock.minutesDCV != null) digitalClock.minutesDCV.ChangeToTargetTime(changeMinutesValue);
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
