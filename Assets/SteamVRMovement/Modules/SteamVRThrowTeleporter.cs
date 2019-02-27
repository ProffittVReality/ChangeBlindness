﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SteamVRThrowTeleporter : MonoBehaviour {

    [Header("-Teleport Button Controls-")]
    public SteamVRInput.Button TeleportButton = SteamVRInput.Button.Trigger;     //Teleport Button
    public SteamVRInput.Button CancelButton = SteamVRInput.Button.Grip;     //Teleport Button

    [Header("-Settings")]
    public bool fadeTeleport = true;
    public float teleportTime = 0;
    TeleportThrowerSteamVR leftPorter;
    TeleportThrowerSteamVR rightPorter;
    [Header("-Required Hookups-")]
    public TeleportThrowerSteamVR TeleportThrower;
    VRMoveSteamVR refSystem;
    //   // Use this for initialization

    void Start()
    {
        refSystem = GetComponent<VRMoveSteamVR>();
        if (!refSystem)
        {
            Debug.Log("VRAltMove is not on VRMove object disabling");
            this.enabled = false;
            return;
        }
    }

    //// Update is called once per frame
    void Update()
    {
        if (refSystem.canMove)
        {
            switch (refSystem.ControlsOn)
            {
                case VRMoveSteamVR.eControllerType.Left:
                    LeftThrower();
                    break;
                case VRMoveSteamVR.eControllerType.Right:
                    RightThrower();
                    break;
                case VRMoveSteamVR.eControllerType.Both:
                    LeftThrower();
                    RightThrower();
                    break;
                default:
                    break;
            }
        }
    }

    void LeftThrower()
    {
        VRMoveSteamVR.InputData InputHolderDown = refSystem.InputReturnDown(TeleportButton);
        VRMoveSteamVR.InputData InputHolderUp = refSystem.InputReturnUp(TeleportButton);
        if (InputHolderDown.pressed && InputHolderDown.isLeft)
        {
            if (leftPorter)
            {
                TeleportMe(leftPorter.transform);
                leftPorter.DeleteThrower();
                //Teleport To And Delete Teleporter;
            }
            else
            {
                leftPorter = Instantiate(TeleportThrower, refSystem.leftController.position, TeleportThrower.transform.rotation);
                leftPorter.myTeleporter = this;
                leftPorter.isSelected = true;
                leftPorter.transform.parent = refSystem.leftController;
                Physics.IgnoreCollision(leftPorter.GetComponent<Collider>(), refSystem.yourRig);
                //Spawn Teleporter //Parent!
            }
        }
        if (InputHolderUp.pressed && InputHolderUp.isLeft && leftPorter)
        {
            leftPorter.Drop();
            //Throw Teleporter System
        }
        InputHolderDown = refSystem.InputReturnDown(CancelButton);
        if (InputHolderDown.pressed && InputHolderDown.isLeft)
        {
            if(leftPorter)
            {
                Destroy(leftPorter.gameObject);
                leftPorter = null;
            }
        }
    }

    void RightThrower()
    {
        VRMoveSteamVR.InputData InputHolderDown = refSystem.InputReturnDown(TeleportButton);
        VRMoveSteamVR.InputData InputHolderUp = refSystem.InputReturnUp(TeleportButton);
        if (InputHolderDown.pressed && InputHolderDown.isRight)
        {
            if (rightPorter)
            {
                TeleportMe(rightPorter.transform);
                rightPorter.DeleteThrower();
                //Teleport To And Delete Teleporter;
            }
            else
            {
                rightPorter = Instantiate(TeleportThrower, refSystem.rightController.position, TeleportThrower.transform.rotation);
                rightPorter.myTeleporter = this;
                rightPorter.isSelected = true;
                rightPorter.transform.parent = refSystem.rightController;
                Physics.IgnoreCollision(rightPorter.GetComponent<Collider>(), refSystem.yourRig);
                //Spawn Teleporter //Parent!
            }
        }
        if (InputHolderUp.pressed && InputHolderUp.isRight && rightPorter)
        {
            rightPorter.Drop();
            //Throw Teleporter System
        }
        InputHolderDown = refSystem.InputReturnDown(CancelButton);
        if (InputHolderDown.pressed && InputHolderDown.isRight)
        {
            if (rightPorter)
            {
                Destroy(rightPorter.gameObject);
                rightPorter = null;
            }
        }
    }

    public void TeleportMe(Transform teleportPoint)
    {
        if(teleportPoint.transform == leftPorter)
        {
            leftPorter = null;
        }
        if (teleportPoint.transform == rightPorter)
        {
            rightPorter = null;
        }
        Vector3 holder = teleportPoint.position;
        holder.y += refSystem.GetHeight();
        //Instant if Zero
        if (fadeTeleport)
        {
            refSystem.myFade.StartFadeIn(refSystem.fadeTime);
            refSystem.yourRig.transform.position = holder;
            Invoke("BumpMe", Time.deltaTime);
            return;
        }
        if (teleportTime == 0)
        {
            refSystem.yourRig.transform.position = holder;
            Invoke("BumpMe", Time.deltaTime);
        }
        else
        {
            refSystem.yourRig.transform.DOMove(holder, teleportTime);
            Invoke("BumpMe", teleportTime + Time.deltaTime);
        }
    }
    void BumpMe()
    {
        refSystem.yourRig.Move(Vector3.down * .01f);
    }
}
