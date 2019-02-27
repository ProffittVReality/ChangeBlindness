using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SteamVRBlink : MonoBehaviour
{
    [Header("-Blink Controls-")]
    public SteamVRInput.Button BlinkButton;       //Blink Foward
    public enum eBlinkMode { Instant, HoldRelease };
    public bool canBlink = true;
    [Header("-Blink Settings-")]
    public bool fadeBlink;
    public eBlinkMode BlinkMode;
    public float blinkDistance = 10;          //Max Blink Distance
    public float blinkMoveTime = .4f;            // Blink and Teleport Speed, 0 Is Instant
    [Header("-Required HookUps-")]
    public Transform blinkPoint;          //Your Teleport Object




    VRMoveSteamVR refSystem;
    //   // Use this for initialization
    bool inBlink;
    Transform storedTransformBlink;

    // Use this for initialization
    void Start()
    {
        refSystem = GetComponent<VRMoveSteamVR>();
        if (!refSystem)
        {
            Debug.Log("VRTouchRotate is not on VRMove object disabling");
            this.enabled = false;
            return;
        }
    }
    void Update()
    {
        if (canBlink)
        {
            BlinkInput();
        }
    }

    public void BlinkInput()
    {
        if (BlinkMode == eBlinkMode.Instant)
        {
            FowardBlinkNormal();
        }
        if (BlinkMode == eBlinkMode.HoldRelease)
        {
            FowardBlinkHold();
        }
    }

    /// <summary>
    /// Foward Blink System  ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    ///     Blink Foward Set Direction.
    /// </summary>
    /// 
    public void FowardBlinkNormal()
    {
        VRMoveSteamVR.InputData InputHolderDown = refSystem.InputReturnDown(BlinkButton);
        if (InputHolderDown.pressed)
        {
            //Cast Ray Foward
            Ray ray = new Ray(InputHolderDown.selectedController.position, InputHolderDown.selectedController.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, blinkDistance))
            {
                //Blink to Point
                refSystem.yourRig.transform.DOMove(hit.point, blinkMoveTime);
                Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            }
            else
            {
                //Blink to Max Distance
                refSystem.yourRig.transform.DOMove(ray.GetPoint(blinkDistance), blinkMoveTime);
                Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            }
        }
    }

    void BumpMe()
    {
        refSystem.yourRig.Move(Vector3.down * .01f);
    }

    public void FowardBlinkHold()
    {
        VRMoveSteamVR.InputData InputHolderDown = refSystem.InputReturnDown(BlinkButton);
        VRMoveSteamVR.InputData InputHolderUp = refSystem.InputReturnUp(BlinkButton);
        if (InputHolderDown.pressed && !inBlink)
        {
            inBlink = true;
            storedTransformBlink = InputHolderDown.selectedController;
        }
        if (inBlink)
        {
            Ray ray = new Ray(storedTransformBlink.position, storedTransformBlink.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, blinkDistance))
            {
                blinkPoint.transform.DOMove(hit.point, .05f);
                blinkPoint.gameObject.SetActive(true);
                ////Blink to Point
            }
            else
            {
                blinkPoint.transform.DOMove(ray.GetPoint(blinkDistance), .05f);
                blinkPoint.gameObject.SetActive(true);
                ////Blink to Max Distance
            }
        }
        if (InputHolderUp.pressed)
        {
            inBlink = false;
            blinkPoint.gameObject.SetActive(false);
            //Cast Ray Foward
            Ray ray = new Ray(storedTransformBlink.position, storedTransformBlink.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, blinkDistance))
            {
                //Blink to Point
                refSystem.yourRig.transform.DOMove(hit.point, blinkMoveTime);
                Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            }
            else
            {
                //Blink to Max Distance
                refSystem.yourRig.transform.DOMove(ray.GetPoint(blinkDistance - .2f), blinkMoveTime);
                Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            }
        }
    }
}
