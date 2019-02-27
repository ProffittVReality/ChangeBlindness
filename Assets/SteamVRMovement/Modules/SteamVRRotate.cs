using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SteamVRRotate : MonoBehaviour
{
    public bool canRotate = true;
    public enum eRotationMode { PointAndShoot, QuickStick, TouchPadLR, None };
    [Header("-Rotate Modes-")]
    public eRotationMode RotationMode = eRotationMode.QuickStick;
    [Header("-Rotate Controls-")]
    public SteamVRInput.Button rotateButton;      //Rotate to Where Controller Is Located
    [Header("-Rotate Settings-")]
    public bool fadeRotate = true;
    [Range(0, 180)]
    public float rotateDegrees = 45;
    public float rotateTime = 0;            // ButtonRotation
    public float slowRotateSpeed = .7f;           //Speed for Stick Rotate

    

    VRMoveSteamVR refSystem;
    void Start()
    {
        


        refSystem = GetComponent<VRMoveSteamVR>();
    }

void Update()
    {
        if(canRotate)
        {
            if (RotationMode == eRotationMode.PointAndShoot)
            {
                PointShootPressed();
            }
            if (RotationMode == eRotationMode.QuickStick)
            {
                QuickStickRotate();
            }
            if (RotationMode == eRotationMode.TouchPadLR)
            {
                TouchPadLR();
            }
        }
      
    }

    void TouchPadLR()
    {
        if (SteamVRInput.GetDown(rotateButton, SteamVRInput.Controller.Right))
        {
            RotateByDegrees(-rotateDegrees);
        }
        if (SteamVRInput.GetDown(rotateButton, SteamVRInput.Controller.Left))
        {
            RotateByDegrees(rotateDegrees);
        }
    }

    //Slowly Rotates the Charactor Controller
    void SlowStickRotate()
    {
        float h = 0;
        switch (refSystem.ControlsOn)
        {
            case VRMoveSteamVR.eControllerType.Left:
                h = SteamVRInput.GetTouchPadAxis(SteamVRInput.Controller.Left).x;
                break;
            case VRMoveSteamVR.eControllerType.Right:
                h = SteamVRInput.GetTouchPadAxis(SteamVRInput.Controller.Right).x;
                break;
            case VRMoveSteamVR.eControllerType.Both:
                h = SteamVRInput.GetTouchPadAxis(SteamVRInput.Controller.Left).x;
                h += SteamVRInput.GetTouchPadAxis(SteamVRInput.Controller.Right).x;
                h = Mathf.Clamp(h, -1, 1);
                break;

        }
        if (Mathf.Abs(h) > .1f)
        {
            RotateByDegrees(h * slowRotateSpeed);
        }
    }
    //Quick 45 Degree Rotations for the Charactor Controller
    void QuickStickRotate()
    {
        switch (refSystem.ControlsOn)
        {
            case VRMoveSteamVR.eControllerType.Left:
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadLeft,SteamVRInput.Controller.Left))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadRight, SteamVRInput.Controller.Left))
                {
                    RotateByDegrees(rotateDegrees);
                }
                break;
            case VRMoveSteamVR.eControllerType.Right:
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadLeft,SteamVRInput.Controller.Right))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadRight, SteamVRInput.Controller.Right))
                {
                    RotateByDegrees(rotateDegrees);
                }
                break;
            case VRMoveSteamVR.eControllerType.Both:
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadLeft, SteamVRInput.Controller.Left))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadRight, SteamVRInput.Controller.Left))
                {
                    RotateByDegrees(rotateDegrees);
                }
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadLeft, SteamVRInput.Controller.Right))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (SteamVRInput.GetDown(SteamVRInput.Button.TouchPadRight, SteamVRInput.Controller.Right))
                {
                    RotateByDegrees(rotateDegrees);
                }
                break;

        }
    }
    //Point Shoot Pressed Event
    void PointShootPressed()
    {
        VRMoveSteamVR.InputData InputHolder = refSystem.InputReturnDown(rotateButton);
       if(InputHolder.isRight == true)
        {
            PointAndShootRotation(refSystem.rightController);
        }
        if (InputHolder.isLeft == true)
        {
            PointAndShootRotation(refSystem.leftController);
        }
    }

    /// <summary>
    /// Point and Shoot Rotation ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    ///  Changes the Rotation based on where the Controller is pointing and where you are looking only only in one direction. This allows someone to quickly look behind them.
    /// </summary>

    void PointAndShootRotation(Transform selectedController)
    {
            if (fadeRotate)
            {
                refSystem.myFade.StartFadeIn(refSystem.fadeTime);
            }
            //Get Position in front of Object
            Vector3 holder = new Ray(selectedController.position, selectedController.forward).GetPoint(1);
            //Get Rotational Direction
            Vector3 lookPos = holder - refSystem.headRig.transform.position;
            //Remove Y Component
            lookPos.y = 0;
            //Transform rotation
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            //Get Rotational Direction
            Vector3 rotPosition = rotation.eulerAngles - refSystem.transform.localRotation.eulerAngles;
            //Flatten Rotational Return
            rotPosition.x = 0;
            rotPosition.z = 0;
            //Apply Rotation
            refSystem.yourRig.transform.DORotate(rotPosition, rotateTime);
    }

    //Helper Function To rotate by set Degrees
    public void RotateByDegrees(float degrees)
    {
        RotateByDegrees2(degrees);
        //if (refSystem.myFade && fadeRotate)
        //{
        //    refSystem.myFade.StartFadeIn(refSystem.fadeTime);
        //}
        //Vector3 holder1;
        //holder1 = refSystem.yourRig.transform.rotation.eulerAngles;
        //holder1.y += degrees;
        //Vector3 rotPosition = holder1;
        //refSystem.yourRig.transform.DORotate(rotPosition, rotateTime);
    }

    void RotateByDegrees2(float degrees)
    {
        if (refSystem.myFade && fadeRotate)
        {
            refSystem.myFade.StartFadeIn(refSystem.fadeTime);
        }
        //Transform mc =//LogsUtil.getMainCamera();
        Vector3 cameraPos = transform.TransformPoint(refSystem.headRig.position);
        Vector3 holder1;
        holder1 = refSystem.yourRig.transform.rotation.eulerAngles;
        holder1.y += degrees;
        Vector3 rotPosition = holder1;
        if (rotateTime == 0)
        {
            refSystem.yourRig.transform.eulerAngles = holder1;
        }
        else
        {
            refSystem.yourRig.transform.DORotate(holder1, rotateTime);
        }

        
        Vector3 newCameraPos = transform.TransformPoint((refSystem.headRig.position));
        Vector3 cameraDifference = newCameraPos - cameraPos;
        refSystem.yourRig.transform.position = refSystem.yourRig.transform.position - cameraDifference;
        //Debug.Log("now cameraPos=" + cameraPos);
    
    }
}

