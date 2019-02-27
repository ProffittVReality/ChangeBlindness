/*******************************************************
* Copyright (C) 2016 3lbGames
* 
* VRightMove
*
* VRightMove can not be copied and/or distributed without the express
* permission of 3lbGames

For additional Information or unity development services please contact services@3lbgames.com

For technical support contact https://discord.gg/W3CVAPu

DoTween is being used to help with easing and simulator sickness reduction.

*******************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Valve.VR;

public class VRMoveSteamVR : MonoBehaviour
{

    public enum eControllerType { Left,Right,Both };
    [Header("-Movement Control Settings-")]
    public eControllerType ControlsOn;
    SteamVRInput.Controller myController;  //Controller Choice
    public SteamVRInput.Button ForwardButton;        //Button for Default Movement
    public SteamVRInput.Button BackwardButton;      //Backwards Button


    [Header("-Movement Modes-")]
    public bool canMove = true;
    public enum eMovementMode { Flight, Grounded, None,NoneWithGravity, Keyboard};
    public eMovementMode MovementMode = eMovementMode.Flight;


    [Header("-General Settings-")]
    public float moveSpeed = 5;               // Your MovementSpeed shared across all Movement Systems
    public float PlayerGravity = 50;          //Player Gravity for the FPS Controller
    [Header("-Fade Settings-")]
    public float fadeTime = .3f;
    public bool resetTrackerOnLoad = true;
    //[Header("-Acceleration Settings-")]
    bool accelSpeed = true;            //Enable Speed Acceleration
    float decay = .9f;                 //Speed Decay 
    [Range(0, 2)]
    float acclAmount = .8f;             //Acceleration Curve 
    float acc = .1f;
    [Header("-Hookups-")]
    public CharacterController yourRig;      //Ensure the Charactor Controller is the correct size for your play space
    public bool AutoAssignTheRest = true;
    public Transform headRig;                //Slot for the Center Camera
    public Transform leftController;        //Use either a touch controller or the headRig;
    public Transform rightController;        //Use either a touch controller or the headRig;
    public VRFadeScript myFade;
    float curSpeed;
    [HideInInspector]
    public bool mainMovementOverRide;
    Transform selectedController;        //Use either a touch controller or the headRig;

   

    //Slowed Start to ensure you get all required controllers;
    IEnumerator Start()
    {
        

        //yourRig = Camera.main.GetComponent<CharacterController>();

        if (AutoAssignTheRest)
        {
            canMove = false;
            yield return new WaitForSeconds(Time.deltaTime);
            SteamVR_ControllerManager ObjectHolder = yourRig.GetComponent<SteamVR_ControllerManager>();
            leftController = ObjectHolder.left.transform;
            rightController = ObjectHolder.right.transform;
            headRig = GameObject.FindObjectOfType<SteamVR_Camera>().transform;
            if (headRig.GetComponent<VRFadeScript>())
            {
                myFade = headRig.GetComponent<VRFadeScript>();
            }
            else
            {
                myFade = headRig.gameObject.AddComponent<VRFadeScript>();
            }
        }
        canMove = true;
        if (resetTrackerOnLoad)
        {
            SteamVRReCenter();
        }
    }

    //Returns Player's Height for Teleportation
    public float GetHeight()
    {
        float holder;
        if(yourRig.height > yourRig.radius)
        {
            holder = yourRig.height;
        }
        else
        {
            holder = yourRig.radius/2;
        }
        return holder;
    }
  

    // Update is called once per frame
    void Update()
    {
        //Check Movement;
        //Note Debug Flight will OverRide Everything!
        if (canMove)
        {
            if (MovementMode == eMovementMode.Keyboard)
            {
                DebugFlight();
            }
            //Module will overRide Basic Movement
            if (mainMovementOverRide)
            {
                return;
            }
            if (MovementMode == eMovementMode.NoneWithGravity)
            {
                ApplyGravity();
            }
            if(MovementMode == eMovementMode.Flight || MovementMode == eMovementMode.Grounded)
            {
                MoveInputSystem();
                if(MovementMode == eMovementMode.Grounded)
                {
                    ApplyGravity();
                }
            }
        }
    }

    public void SteamVRReCenter()
    {
        Valve.VR.OpenVR.System.ResetSeatedZeroPose();
        Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
    }
    //Apply Gravity if not Grounded
    public void ApplyGravity()
    {
        if (!yourRig.isGrounded)
        {
            Vector3 holder = Vector3.zero;
            holder.y -= PlayerGravity * Time.deltaTime * moveSpeed;
            //Apply Movement
            yourRig.Move(holder * Time.deltaTime);
        }
       
    }
    //Gives gravity to modules
    public float GetGravity()
    {
        return -PlayerGravity * Time.deltaTime * moveSpeed;
    }

    //Detect Inputs on Which Hand
    void MoveInputSystem()
    {
        float v = 0;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                if (SteamVRInput.Get(ForwardButton, SteamVRInput.Controller.Left))
                {
                    v = GetAxisFromButton(ForwardButton, SteamVRInput.Controller.Left);
                }
                //Detect Left Backward
                if (SteamVRInput.Get(BackwardButton, SteamVRInput.Controller.Left))
                {
                    v = GetAxisFromButton(BackwardButton, SteamVRInput.Controller.Left, false); 
                }
                AdvancedMove(leftController, v);
                break;
            //RIGHT
            case eControllerType.Right:
                if (SteamVRInput.Get(ForwardButton, SteamVRInput.Controller.Right))
                {
                    v = GetAxisFromButton(ForwardButton, SteamVRInput.Controller.Right);
                }
                //Detect Left Backward
                if (SteamVRInput.Get(BackwardButton, SteamVRInput.Controller.Right))
                {
                    v = GetAxisFromButton(BackwardButton, SteamVRInput.Controller.Right, false);
                }
                AdvancedMove(rightController, v);
                break;
            ///BOTH;
            case eControllerType.Both:
                if (SteamVRInput.Get(ForwardButton, SteamVRInput.Controller.Left))
                {
                    v = GetAxisFromButton(ForwardButton, SteamVRInput.Controller.Left);
                }
                //Detect Left Backward
                if (SteamVRInput.Get(BackwardButton, SteamVRInput.Controller.Left))
                {
                    v = GetAxisFromButton(BackwardButton, SteamVRInput.Controller.Left, false);
                }
                if(v != 0)
                {
                    AdvancedMove(leftController, v);
                }
                v = 0;
                if (SteamVRInput.Get(ForwardButton, SteamVRInput.Controller.Right))
                {
                    v = GetAxisFromButton(ForwardButton, SteamVRInput.Controller.Right);
                }
                //Detect Left Backward
                if (SteamVRInput.Get(BackwardButton, SteamVRInput.Controller.Right))
                {
                    v = GetAxisFromButton(BackwardButton, SteamVRInput.Controller.Right, false);
                }
                if (v != 0)
                {
                    AdvancedMove(rightController, v);
                }
                break;
        }
    }
    //Advanced Movement System for flight and grounded systems
    public void AdvancedMove(Transform thisController,float speed)
    {
        acc = moveSpeed * acclAmount;
        float v = speed; //Place Holder for Foward Stick
        if (accelSpeed)
        {
            //Decay Speed
            if (Mathf.Abs(v) <= 0)
            {
                curSpeed *= decay;
            }
            else
            {
                //Apply Acceloration
                curSpeed += acc * v * Time.deltaTime;
                //curSpeed += acc * v2 * Time.deltaTime;
            }
            curSpeed = Mathf.Clamp(curSpeed, -moveSpeed, moveSpeed);
        }
        else
        {
            curSpeed = moveSpeed * v;
        }
        curSpeed = Mathf.Clamp(curSpeed, -curSpeed * v, curSpeed * v);
        Vector3 holder = thisController.forward;
        if(MovementMode == eMovementMode.Grounded)
        {
            holder.y = 0;
        }
        yourRig.Move(holder * curSpeed * Time.deltaTime);
    }


    /// <summary>
    ///Debug Flight
    ///      This is the Main Function for the Movement system. It is designed to be an arcade flight controller in order to navigate around in a space. 
    ///      Press the Move button and you are dragged foward where ever the touch controller is pointed.
    /// </summary>
    public void DebugFlight()
    {
        float v = 0;
        float h = 0;
        switch (MovementMode)
        {
            case eMovementMode.Keyboard:
                v = Input.GetAxis("Vertical");
                h = Input.GetAxis("Horizontal");
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    RotateByDegrees(-45);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    RotateByDegrees(45);
                }
                float speedAdd = 0;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speedAdd = moveSpeed;
                }
                yourRig.Move(headRig.TransformDirection(new Vector3(h, 0, v)) * (moveSpeed + speedAdd) * Time.deltaTime);
                break;
            default:
                break;
        }
    }
    //Helper function that returns the Axis when set to a button
    float GetAxisFromButton(SteamVRInput.Button theButton ,SteamVRInput.Controller theController,bool isPositive = true)
    {
        float holder = 0;
        switch (theButton)
        {
            case SteamVRInput.Button.Trigger:
                holder = SteamVRInput.GetTriggerAxis(theController).x;
                break;
            default:
                holder = 1; //This return is to for things without Axises
                break;
        }
        if(!isPositive)
        {
            holder *= -1;
        }
        return holder;
    }

    //Helper Function to do Rotation
    public void RotateByDegrees(float degrees)
    {
        if (myFade)
        {
            myFade.StartFadeIn(fadeTime);
        }
        Vector3 holder1;
        holder1 = yourRig.transform.rotation.eulerAngles;
        holder1.y += degrees;
        Vector3 rotPosition = holder1;
        yourRig.transform.DORotate(rotPosition,0);
    }

    //Input Data Structure for Modules
    public struct InputData
    {
        public bool pressed;
        public Transform selectedController;
        public bool isLeft;
        public bool isRight;
    }

    //Get InputData Function
    public InputData InputReturnGet(SteamVRInput.Button myButton)
    {
        InputData holder = new InputData();
        holder.pressed = false;
        holder.isLeft = false;
        holder.isRight = false;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                holder.selectedController = leftController;
                if (SteamVRInput.Get(myButton, SteamVRInput.Controller.Left))
                {
                    holder.pressed = true;
                    holder.isLeft = true;
                    return holder;
                }
                break;
            //RIGHT
            case eControllerType.Right:
                holder.selectedController = rightController;
                if (SteamVRInput.Get(myButton, SteamVRInput.Controller.Right))
                {
                    holder.pressed = true;
                    holder.isRight = true;
                    return holder;
                }
                break;
            ///BOTH;
            case eControllerType.Both:
                if (SteamVRInput.Get(myButton, SteamVRInput.Controller.Left))
                {
                    holder.selectedController = leftController;
                    holder.pressed = true;
                    holder.isLeft = true;

                }
                if (SteamVRInput.Get(myButton, SteamVRInput.Controller.Right))
                {
                    holder.selectedController = rightController;
                    holder.pressed = true;
                    holder.isRight = true;
                }
                return holder;
        }
        return holder;
    }
    //Down InputData Function
    public InputData InputReturnDown(SteamVRInput.Button myButton)
    {
        InputData holder = new InputData();
        holder.pressed = false;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                holder.selectedController = leftController;
                if (SteamVRInput.GetDown(myButton, SteamVRInput.Controller.Left))
                {
                    holder.pressed = true;
                    holder.isLeft = true;
                    return holder;
                }
                break;
            //RIGHT
            case eControllerType.Right:
                holder.selectedController = rightController;
                if (SteamVRInput.GetDown(myButton, SteamVRInput.Controller.Right))
                {
                    holder.pressed = true;
                    holder.isRight = true;
                    return holder;
                }
                break;
            ///BOTH;
            case eControllerType.Both:

                if (SteamVRInput.GetDown(myButton, SteamVRInput.Controller.Left))
                {
                    holder.selectedController = leftController;
                    holder.pressed = true;
                    holder.isLeft = true;
                }
                if (SteamVRInput.GetDown(myButton, SteamVRInput.Controller.Right))
                {
                    holder.selectedController = rightController;
                    holder.pressed = true;
                    holder.isRight = true;
                }
                return holder;
        }
        return holder;
    }
    //Up InputData Function
    public InputData InputReturnUp(SteamVRInput.Button myButton)
    {
        InputData holder = new InputData();
        holder.pressed = false;
        switch (ControlsOn)
        {
            //LEFT
            case eControllerType.Left:
                //Detect Left Foward
                holder.selectedController = leftController;
                if (SteamVRInput.GetUp(myButton, SteamVRInput.Controller.Left))
                {
                    holder.pressed = true;
                    holder.isLeft = true;
                    return holder;
                }
                break;
            //RIGHT
            case eControllerType.Right:
                holder.selectedController = rightController;
                if (SteamVRInput.GetUp(myButton, SteamVRInput.Controller.Right))
                {
                    holder.pressed = true;
                    holder.isRight = true;
                    return holder;
                }
                break;
            ///BOTH;
            case eControllerType.Both:

                if (SteamVRInput.GetUp(myButton, SteamVRInput.Controller.Left))
                {
                    holder.selectedController = leftController;
                    holder.pressed = true;
                    holder.isLeft = true;
                }
                if (SteamVRInput.GetUp(myButton, SteamVRInput.Controller.Right))
                {
                    holder.selectedController = rightController;
                    holder.pressed = true;
                    holder.isRight = true;
                }
                return holder;
        }
        return holder;
    }
}
