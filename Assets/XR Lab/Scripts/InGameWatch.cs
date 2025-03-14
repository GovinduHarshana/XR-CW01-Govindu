using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Provides basic time and fps data to a display
/// </summary>
[AddComponentMenu("XRLab/Generic VR/In Game Watch")]
public class InGameWatch : MonoBehaviour
{
    [Tooltip("the Text component to output the date and time")]
    [SerializeField] private Text timeReadout;
    [Tooltip("the Text componet to output FPS data")]
    [SerializeField] private Text fpsReadout;

    [Tooltip("the Text componet to output FPS data")]
    [SerializeField] private Text m_diag;


    // Update is called once per frame
    void Update()
    {
        if(timeReadout != null)
            timeReadout.text = System.DateTime.Now.ToString();

        if(fpsReadout != null)
            fpsReadout.text = "" + (1f / Time.unscaledDeltaTime) + " fps";
        
        if (m_diag == null)
            return;

        m_diag.text = "" + SystemInfo.processorType + " \n" + SystemInfo.processorCount + "\n" + SystemInfo.graphicsDeviceType + "\n" + SystemInfo.deviceName + " - " + SystemInfo.deviceModel;
    }
}
