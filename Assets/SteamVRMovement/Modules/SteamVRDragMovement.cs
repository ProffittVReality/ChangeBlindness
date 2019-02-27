using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(VRMoveSteamVR))]
public class SteamVRDragMovement : MonoBehaviour
{

    [Header("-Drag Visual-")]
    public Transform DragVisual; // Used if you wish a visual where you started Dragging from
    [Header("-Settings-")]
    [Range(.1f, 50)]
    public float multipler = 1; //Increases the magnitude of your grab 
    public bool invertControls; //Inverts Direction
    Vector3 lastPos;       //Direction Holder
    bool isOn;             //Toggles Between Hands
    VRMoveSteamVR refSystem; //REf Holder
    Transform storedTransform; //Stores current Controller;
    void Start()
    {

        //Get Ref
        refSystem = GetComponent<VRMoveSteamVR>();
        refSystem.mainMovementOverRide = true;

        //Hide Drag Visual
        if (DragVisual)
        {
            DragVisual.gameObject.SetActive(false);
        }
    }

    //// Update is called once per frame
    void Update()
    {
        //Checks to siee if CanMove
        if (refSystem.canMove)
            {
                DraggingMove();
            }
        //Apply Gravity if Grounded;
        ApplyGravity();
    }

    public void ApplyGravity()
    {
        //Check to see if Grounded
        if(!refSystem.yourRig.isGrounded)
        {
            //Apply Gravity in Grounded Move Mode
            if (refSystem.MovementMode == VRMoveSteamVR.eMovementMode.Grounded)
            {
                Vector3 holder = Vector3.zero;
                holder.y -= refSystem.PlayerGravity * Time.deltaTime;
                refSystem.yourRig.Move(holder);

            }
        }
    }
    //Main Draging Function
    public void DraggingMove()
    {
        //Get Inputs
        VRMoveSteamVR.InputData InputHolderDown = refSystem.InputReturnDown(refSystem.ForwardButton);
        VRMoveSteamVR.InputData InputHolderUp = refSystem.InputReturnUp(refSystem.ForwardButton);

        if (InputHolderDown.pressed)
        {
            //Store Pressed
            storedTransform = InputHolderDown.selectedController;
            lastPos = storedTransform.position;
            isOn = true;
            if (DragVisual)
            {
                DragVisual.gameObject.SetActive(true);
                DragVisual.transform.position = storedTransform.position;
            }
        }

        if (isOn)
        {
            Vector3 holder = storedTransform.position - lastPos;
            //Invert Controls and Direction
            if (!invertControls)
            {
                holder = holder * ((-100 * multipler)) * Time.deltaTime;

            }
            else
            {
                holder = holder * ((100 * multipler)) * Time.deltaTime;
            }
            //If Grounded Give zero YAxis
            if(refSystem.MovementMode == VRMoveSteamVR.eMovementMode.Grounded)
            {
                holder.y = 0;
            }
            refSystem.yourRig.Move(holder);
            lastPos = storedTransform.position;
            if (InputHolderUp.pressed)
            {
                if (InputHolderUp.selectedController == storedTransform)
                {
                    isOn = false;
                    if (DragVisual)
                    {
                        DragVisual.gameObject.SetActive(false);
                    }

                }
            }
        }
    }
}
