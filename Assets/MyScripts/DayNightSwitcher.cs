using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSwitcher : MonoBehaviour
{
    public Material daySkybox;
    public Material nightSkybox;

    public void SetDayMode()
    {
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();  // updates lighting if using GI
    }

    public void SetNightMode()
    {
        RenderSettings.skybox = nightSkybox;
        DynamicGI.UpdateEnvironment();  // updates lighting if using GI
    }
}
