/*******************************************************
 * Copyright (C) 2017 3lbGames
 * 
 * SteamVRInput
 * 
 * SteamVRInput can not be copied and/or distributed without the express
 * permission of 3lbGames


 Updates:

 Added Support for non SteamVR Rigs

 For additional information or Unity development services please contact services@3lbgames.com
 
 For technical support contact support@3lbgames.com

 ViveInput is a simple-to-use replication of the OVRInput that works for Vive.
 It eases the amount of code needed to get your project going.

 1) Place Script on Object in Scene
 2) It will auto locate the left and right controller from SteamVR_ControllerManager;
 3) Script AWAY!

 Later versions will grab it from the existing scripts.

 Example calls:

         SteamVRInput.Get(Button.Grip, Controller.Right); //Get Grip on Right
         SteamVRInput.GetTouchDown(Button.TouchPad, Controller.Right); //GetTouchDown TouchPad Right;


         SteamVRInput.HapticPulse(Controller.Left, 100); //Play on Left Controller
         SteamVRInput.HapticPulse(Controller.Left, 100,true); //Play on both Controllers

         //ANY controller not supported on Axes and will return Vector2.zero;

          SteamVRInput.GetTriggerAxis(Controller.Left); //returns Vector2 from Trigger;
          SteamVRInput.GetTouchPadAxis(Controller.Right); //returns Vector2 from TouchPad;

 *******************************************************/
using UnityEngine;
using System.Collections;

public class SteamVRInput : MonoBehaviour
{

    public bool autoFindControllers = true;
    public float touchPadDeadZone = .3f;
    public static float touchPadDeadZoneS;
    public GameObject LeftController;
    public GameObject RightController;
    private static GameObject leftControllerS;
    private static GameObject rightControllerS;
    private static SteamVR_Controller.Device deviceL;
    private static SteamVR_Controller.Device deviceR;
    public bool RightControllerDetected;
    public bool LeftControllerDetected;

    public enum Button
    {
        None = 0,
        Trigger,
        Grip,
        TouchPad,
        ApplicationMenu,
        TouchPadUp,
        TouchPadDown,
        TouchPadLeft,
        TouchPadRight
    }
    public enum Controller
    {
        Left,
        Right,
        Any
    }

    enum InputRequest
    {
        GetDown,
        GetUp,
        Get,
        GetTouchDown,
        GetTouchUp,
        GetTouch
    }

    //#################################################
    // Start Function
    // It is best to assign the controllers, due to Vive taking a while to detect controllers.
    // You can, however, use Auto Find.
    //#################################################


    

    void Start()
    {
        

        if (autoFindControllers)
        {
            SteamVR_ControllerManager holder = GameObject.FindObjectOfType<SteamVR_ControllerManager>();
            LeftController = holder.left.GetComponent<SteamVR_TrackedObject>().gameObject;
            RightController = holder.right.GetComponent<SteamVR_TrackedObject>().gameObject;    
        }
        touchPadDeadZoneS = touchPadDeadZone;
        leftControllerS = LeftController;
        rightControllerS = RightController;
    }

    //#################################################
    // LateUpdate
    // This is used to Error Check the Controllers and ensure they are detected at all times.
    //#################################################
    public int leftIndex;
    public int rightIndex;
    void LateUpdate()
    {
        if (deviceR == null)
        {
            if (RightController.GetComponent<SteamVR_TrackedObject>())
            {
                rightIndex = (int)RightController.GetComponent<SteamVR_TrackedObject>().index;
                //Debug.Log("Right CONTROLLER IS " + rightIndex);
                if (rightIndex != -1 && rightIndex != leftIndex)
                {
                    deviceR = SteamVR_Controller.Input(rightIndex);
                    RightControllerDetected = true;
                }
                return;
            }
            if (rightControllerS.gameObject.activeSelf == true)
            {
                rightIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
                //Debug.Log("Right CONTROLLER IS " + rightIndex);
                if (rightIndex != -1 && rightIndex != leftIndex)
                {
                    deviceR = SteamVR_Controller.Input(rightIndex);
                    RightControllerDetected = true;
                }
            }
        }
        if (deviceL == null)
        {

            if (LeftController.GetComponent<SteamVR_TrackedObject>())
            {
                leftIndex = (int)LeftController.GetComponent<SteamVR_TrackedObject>().index;
                //Debug.Log("LEFT CONTROLLER IS " + leftIndex);
                if (leftIndex != -1 && rightIndex != leftIndex)
                {
                    deviceL = SteamVR_Controller.Input(leftIndex);
                    LeftControllerDetected = true;

                }
                return;
            }

            if (leftControllerS.gameObject.activeSelf == true)
            {
                leftIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
                //Debug.Log("LEFT CONTROLLER IS " + leftIndex);
                if (leftIndex != -1 && rightIndex != leftIndex)
                {
                    deviceL = SteamVR_Controller.Input(leftIndex);
                    LeftControllerDetected = true;
                }
            }

        }
    }

    //#################################################
    // ReturnSteamButtons
    // This SDK has a weird thing about using these buttons in the inspector and getting the units correctly. 
    // The simple job of this function is to return the units.
    //#################################################
    static Valve.VR.EVRButtonId ReturnSteamVRButtons(Button theButton)
    {
        switch (theButton)
        {
            case Button.Trigger:
                return Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
            case Button.Grip:
                return Valve.VR.EVRButtonId.k_EButton_Grip;
            case Button.TouchPad:
                return Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
            case Button.ApplicationMenu:
                return Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
            case Button.None:
                return Valve.VR.EVRButtonId.k_EButton_A;
            default:
                return Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
        }
    }


    //#################################################
    // isControllerActive
    // This function's job is to detect if the controller is active and has a device attached. 
    // You can use this to see if both controllers are connected, as well.
    //#################################################
    public static bool isControllerActive(Controller controller)
    {
        if (controller == Controller.Any)
        {
            //Error Check Done other Places for Sure
            bool holder = false;
            if (leftControllerS.gameObject.activeInHierarchy && deviceL != null)
            {
                holder = true;
            }
            if (rightControllerS.gameObject.activeInHierarchy && deviceR != null)
            {
                holder = true;
            }
            return holder;
        }
        if (controller == Controller.Left)
        {
            if (leftControllerS.gameObject.activeInHierarchy && deviceL != null)
            {
                return true;
            }
        }
        if (controller == Controller.Right)
        {
            if (rightControllerS.gameObject.activeInHierarchy && deviceR != null)
            {
                return true;
            }
        }
        return false;
    }

    //#################################################
    // HapticPulse
    // This is a simple Haptics Helper that allows you to buzz each controller. 
    // To play on both or any controller send a true to playboth.
    //#################################################
    public static void HapticPulse(Controller controller, int strength)
    {
        strength = Mathf.Clamp(strength, 0, 3999);
        if (controller == Controller.Any)
        {
            if (isControllerActive(Controller.Left))
            {
                SteamVR_Controller.Input((int)deviceL.index).TriggerHapticPulse((ushort)strength);
            }
            if (isControllerActive(Controller.Right))
            {
                SteamVR_Controller.Input((int)deviceR.index).TriggerHapticPulse((ushort)strength);
            }
            return;
        }

        if (controller == Controller.Left && isControllerActive(Controller.Left))
        {
            SteamVR_Controller.Input((int)deviceL.index).TriggerHapticPulse((ushort)strength);
        }
        if (controller == Controller.Right && isControllerActive(Controller.Right))
        {
            SteamVR_Controller.Input((int)deviceR.index).TriggerHapticPulse((ushort)strength);
        }
    }

    //#################################################
    // GetTriggerAxis
    // Get Trigger Axis will return the Trigger Axis of the selected controllers
    // Chosing Any will return both controllers, and you can choose to make it clamped or not.
    // UnClamped is useful if you want to detect Triggers being pressed as it will return a value greater than 1.
    //#################################################
    public static Vector2 GetTriggerAxis(Controller controller = Controller.Any, bool clamped = true)
    {
        if (!isControllerActive(controller)) { return Vector2.zero; }

        if (controller == Controller.Left) { return deviceL.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger); }

        if (controller == Controller.Right) { return deviceR.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger); }
        if (controller == Controller.Any)
        {
            Vector2 holder = Vector2.zero;
            if (isControllerActive(Controller.Left))
            {
                holder += deviceL.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
            }
            if (isControllerActive(Controller.Right))
            {
                holder += deviceR.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
            }

            if (clamped)
            {
                holder = Vector2.ClampMagnitude(holder, 1);
            }
            return holder;
        }
        return Vector2.zero;
    }
    //#################################################
    // GetTouchPadAxis
    // Get Touch Pad Axis will return the Touch Pad Axis of the selected controllers.
    // Chosing Any will return both controllers, and you can choose to make it clamped or not.
    // UnClamped is useful if you want to detect TouchPadAxis being pressed as it will return a value greater than 1.
    //#################################################
    public static Vector2 GetTouchPadAxis(Controller controller = Controller.Any, bool clamped = true)
    {
        if (!isControllerActive(controller)) { return Vector2.zero; }
        if (controller == Controller.Left) { return deviceL.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad); }

        if (controller == Controller.Right) { return deviceR.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad); }
        if (controller == Controller.Any)
        {
            Vector2 holder = Vector2.zero;
            if (isControllerActive(Controller.Left))
            {
                holder += deviceL.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            }
            if (isControllerActive(Controller.Right))
            {
                holder += deviceR.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            }
            return holder;
        }
        return Vector2.zero;
    }
    //###################################################################################################################################################
    // Public Button Functions
    // Each one of these functions takes the Input, for example:
    // ViveInput.Get(Button.Grip, Controller.Right); 
    // It then runs it through the solver to return a true or false.
    //###################################################################################################################################################

    //###################################################################################################################################################
    // Get Area
    //###################################################################################################################################################
    public static bool GetUp(Button button, Controller controller = Controller.Any)
    {
        switch (controller)
        {
            case Controller.Left:
                if (isControllerActive(Controller.Left))
                {
                    return InputSolverL(button, InputRequest.GetUp);
                }
                break;
            case Controller.Right:
                if (isControllerActive(Controller.Right))
                {
                    return InputSolverR(button, InputRequest.GetUp);
                }
                break;
            case Controller.Any:
                return InputSolverAny(button, InputRequest.GetUp);
        }
        return false;
    }

    public static bool Get(Button button, Controller controller = Controller.Any)
    {
        switch (controller)
        {
            case Controller.Left:
                if (isControllerActive(Controller.Left))
                {
                    return InputSolverL(button, InputRequest.Get);
                }
                break;
            case Controller.Right:
                if (isControllerActive(Controller.Right))
                {
                    return InputSolverR(button, InputRequest.Get);
                }
                break;
            case Controller.Any:
                return InputSolverAny(button, InputRequest.Get);
        }
        return false;
    }

    public static bool GetDown(Button button, Controller controller = Controller.Any)
    {
        switch (controller)
        {
            case Controller.Left:
                if (isControllerActive(Controller.Left))
                {
                    return InputSolverL(button, InputRequest.GetDown);
                }
                break;
            case Controller.Right:
                if (isControllerActive(Controller.Right))
                {
                    return InputSolverR(button, InputRequest.GetDown);
                }
                break;
            case Controller.Any:
                return InputSolverAny(button, InputRequest.GetDown);
        }
        return false;
    }
    //###################################################################################################################################################
    // Get Touch Area
    //###################################################################################################################################################
    public static bool GetTouchUp(Button button, Controller controller = Controller.Any)
    {
        switch (controller)
        {
            case Controller.Left:
                if (isControllerActive(Controller.Left))
                {
                    return InputSolverL(button, InputRequest.GetTouchUp);
                }
                break;
            case Controller.Right:
                if (isControllerActive(Controller.Right))
                {
                    return InputSolverR(button, InputRequest.GetTouchUp);
                }
                break;
            case Controller.Any:
                return InputSolverAny(button, InputRequest.GetTouchUp);
        }
        return false;
    }

    public static bool GetTouch(Button button, Controller controller = Controller.Any)
    {
        switch (controller)
        {
            case Controller.Left:
                if (isControllerActive(Controller.Left))
                {
                    return InputSolverL(button, InputRequest.GetTouch);
                }
                break;
            case Controller.Right:
                if (isControllerActive(Controller.Right))
                {
                    return InputSolverR(button, InputRequest.GetTouch);
                }
                break;
            case Controller.Any:
                return InputSolverAny(button, InputRequest.GetTouch);
        }
        return false;
    }

    public static bool GetTouchDown(Button button, Controller controller = Controller.Any)
    {
        switch (controller)
        {
            case Controller.Left:
                if (isControllerActive(Controller.Left))
                {
                    return InputSolverL(button, InputRequest.GetTouchDown);
                }
                break;
            case Controller.Right:
                if (isControllerActive(Controller.Right))
                {
                    return InputSolverR(button, InputRequest.GetTouchDown);
                }
                break;
            case Controller.Any:
                return InputSolverAny(button, InputRequest.GetTouchDown);
        }
        return false;
    }

    //###################################################################################################################################################
    // Left Input Solver
    //###################################################################################################################################################
    static bool InputSolverL(Button button, InputRequest request)
    {
        if (button == Button.TouchPadDown || button == Button.TouchPadLeft || button == Button.TouchPadRight || button == Button.TouchPadUp)
        {
            return TouchPadSolver(Controller.Left, button, request);
        }
        switch (request)
        {
            case InputRequest.GetDown:
                return deviceL.GetPressDown(ReturnSteamVRButtons(button));
            case InputRequest.GetUp:
                return deviceL.GetPressUp(ReturnSteamVRButtons(button));
            case InputRequest.Get:
                return deviceL.GetPress(ReturnSteamVRButtons(button));
            case InputRequest.GetTouchDown:
                return deviceL.GetTouchDown(ReturnSteamVRButtons(button));
            case InputRequest.GetTouchUp:
                return deviceL.GetTouchUp(ReturnSteamVRButtons(button));
            case InputRequest.GetTouch:
                return deviceL.GetTouch(ReturnSteamVRButtons(button));
            default:
                break;
        }
        return false;
    }

    //###################################################################################################################################################
    // Right Input Solver
    //###################################################################################################################################################
    static bool InputSolverR(Button button, InputRequest request)
    {
        if (button == Button.TouchPadDown || button == Button.TouchPadLeft || button == Button.TouchPadRight || button == Button.TouchPadUp)
        {
            return TouchPadSolver(Controller.Right, button, request);
        }
        switch (request)
        {
            case InputRequest.GetDown:
                return deviceR.GetPressDown(ReturnSteamVRButtons(button));
            case InputRequest.GetUp:
                return deviceR.GetPressUp(ReturnSteamVRButtons(button));
            case InputRequest.Get:
                return deviceR.GetPress(ReturnSteamVRButtons(button));
            case InputRequest.GetTouchDown:
                return deviceR.GetTouchDown(ReturnSteamVRButtons(button));
            case InputRequest.GetTouchUp:
                return deviceR.GetTouchUp(ReturnSteamVRButtons(button));
            case InputRequest.GetTouch:
                return deviceR.GetTouch(ReturnSteamVRButtons(button));
            default:
                break;
        }
        return false;
    }
    //###################################################################################################################################################
    // Any Input Solver
    //###################################################################################################################################################
    static bool InputSolverAny(Button button, InputRequest request)
    {
        if (button == Button.TouchPadDown || button == Button.TouchPadLeft || button == Button.TouchPadRight || button == Button.TouchPadUp)
        {
            return TouchPadSolver(Controller.Any, button, request);
        }
        bool holder = false;
        switch (request)
        {
            case InputRequest.GetDown:
                if (isControllerActive(Controller.Left))
                {
                    holder = deviceL.GetPressDown(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                if (isControllerActive(Controller.Right))
                {
                    holder = deviceR.GetPressDown(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                break;
            case InputRequest.GetUp:
                if (isControllerActive(Controller.Left))
                {
                    holder = deviceL.GetPressUp(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                if (isControllerActive(Controller.Right))
                {
                    holder = deviceR.GetPressUp(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                break;
            case InputRequest.Get:
                if (isControllerActive(Controller.Left))
                {
                    holder = deviceL.GetPress(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                if (isControllerActive(Controller.Right))
                {
                    holder = deviceR.GetPress(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                break;
            case InputRequest.GetTouchDown:
                if (isControllerActive(Controller.Left))
                {
                    holder = deviceL.GetTouchDown(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                if (isControllerActive(Controller.Right))
                {
                    holder = deviceR.GetTouchDown(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                break;
            case InputRequest.GetTouchUp:
                if (isControllerActive(Controller.Left))
                {
                    holder = deviceL.GetTouchUp(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                if (isControllerActive(Controller.Right))
                {
                    holder = deviceR.GetTouchUp(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                break;
            case InputRequest.GetTouch:
                if (isControllerActive(Controller.Left))
                {
                    holder = deviceL.GetTouch(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                if (isControllerActive(Controller.Right))
                {
                    holder = deviceR.GetTouch(ReturnSteamVRButtons(button));
                    if (holder) { return true; }
                }
                break;
            default:
                break;
        }
        return false;
    }

    static bool TouchPadSolver(Controller controller, Button button, InputRequest request)
    {
        bool didHappen = false;
        switch (request)
        {
            case InputRequest.GetDown:
                didHappen = GetDown(Button.TouchPad, controller);
                break;
            case InputRequest.GetUp:
                didHappen = GetUp(Button.TouchPad, controller);
                break;
            case InputRequest.Get:
                didHappen = Get(Button.TouchPad, controller);
                break;
            case InputRequest.GetTouchDown:
                didHappen = GetTouchDown(Button.TouchPad, controller);
                break;
            case InputRequest.GetTouchUp:
                Debug.Log("Get Touch Up does not work with Virtual Buttons!");
                didHappen = GetTouchUp(Button.TouchPad, controller);
                break;
            case InputRequest.GetTouch:
                didHappen = GetTouch(Button.TouchPad, controller);
                break;
            default:
                break;
        }

        if (didHappen)
        {
            Vector2 holder = GetTouchPadAxis(controller);
            switch (button)
            {
                case Button.TouchPadUp:
                    if (holder.y >= touchPadDeadZoneS)
                    {
                        return true;
                    }
                    break;
                case Button.TouchPadDown:
                    if (holder.y <= -touchPadDeadZoneS)
                    {
                        return true;
                    }
                    break;
                case Button.TouchPadRight:
                    if (holder.x >= touchPadDeadZoneS)
                    {
                        return true;
                    }
                    break;
                case Button.TouchPadLeft:
                    if (holder.x <= -touchPadDeadZoneS)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
        }
        return false;
    }


    //###################################################################################################################################################
    // Extra Useful Functions
    // Angle, for example, can be used to figure out a color wheel.
    //###################################################################################################################################################
    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    public static bool IsBetween(float testValue, float bound1, float bound2)
    {
        return (testValue >= Mathf.Min(bound1, bound2) && testValue <= Mathf.Max(bound1, bound2));
    }
}

