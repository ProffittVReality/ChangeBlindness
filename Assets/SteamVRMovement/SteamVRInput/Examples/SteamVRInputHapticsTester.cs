using UnityEngine;
using System.Collections;

public class SteamVRInputHapticsTester : MonoBehaviour {


    [Header("Haptics Test Buttons")]
    public SteamVRInput.Controller Controller;
    public SteamVRInput.Button HapticButton;
    public SteamVRInput.Controller ControllerToVibrate;
    public bool useGetDown;
    [Header("Haptics Power MAX = 3999")]
    [Range(0, 3999)]
    public int HapticPower;

    void Update()
    {
        if(useGetDown)
        {
            if (SteamVRInput.GetDown(HapticButton, Controller))
            {
                SteamVRInput.HapticPulse(ControllerToVibrate, HapticPower);
            }
        }
        else
        {
            if (SteamVRInput.Get(HapticButton, Controller))
            {
                SteamVRInput.HapticPulse(ControllerToVibrate, HapticPower);
            }
        }
      
    }
}
