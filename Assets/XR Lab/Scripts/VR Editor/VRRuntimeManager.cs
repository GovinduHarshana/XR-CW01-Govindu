using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRLab;

/// <summary>
/// Automatically detects whether a VR headset is connected
/// and spawns in required prefabs. 
/// 
/// This tool is primarily designed to allow seamless
/// switching between XR and mock XR workflows with 
/// minimal user intervention 
/// </summary>
[AddComponentMenu("XRLab/Generic VR/VR Runtime Manager")]
public class VRRuntimeManager : MonoBehaviour
{
    public enum Runtime { Default, XREnabled, MockEnabled}

    [Tooltip("The target position for the XR prefabs to spawn. If left unassigned, the attached object's transform will be used")]
    [SerializeField] private Transform m_xrTargetTransform;

    [Tooltip("The target position for the mock prefabs to spawn. If left unassigned, the attached object's transform will be used")]
    [SerializeField] private Transform m_mockTargetTransform;

    [Tooltip("Choose to override the runtime detection to spawn prefabs for a specific environment type")]
    [SerializeField] private Runtime m_runtimeOverride;

    [Tooltip("The prefabs to spawn for XR")]
    [SerializeField] private GameObject[] m_XRPrefabs;
    [Tooltip("The prefabs to spawn for mock XR")]
    [SerializeField] private GameObject[] m_mockPrefabs;

    void Awake()
    {
        if(m_mockTargetTransform == null) //if the transform is unassigned then assign the transform of the attached object
            m_mockTargetTransform = gameObject.transform;

        if (m_xrTargetTransform == null) //if the transform is unassigned then assign the transform of the attached object
            m_xrTargetTransform = gameObject.transform;

        //if the runtime override is default then run the detection code and spawn respective prefabs
        if (m_runtimeOverride == Runtime.Default)
        {
            //spawning in the relevent prefabs based on HMD detection Note prefabs are handled on the front end
            if (XRLabLib.GetHMDConnected())
            {
                Debug.Log("HMD detected, spawning " + m_XRPrefabs.Length + " XR prefabs");
                foreach (GameObject xrp in m_XRPrefabs)
                {
                    Instantiate(xrp, m_xrTargetTransform);
                }
            }
            else
            {
                Debug.Log("no HMD detected, spawning " + m_mockPrefabs.Length + " mock prefabs");
                foreach (GameObject mp in m_mockPrefabs)
                {
                    Instantiate(mp, m_mockTargetTransform);
                }
            }
        }
        else
        {
            //if override set to XR then spawn in XR prefabs
            if (m_runtimeOverride == Runtime.XREnabled)
            {
                Debug.Log("XR Override, spawning " + m_XRPrefabs.Length + " XR prefabs");
                foreach (GameObject xrp in m_XRPrefabs)
                {
                    Instantiate(xrp, m_xrTargetTransform);
                }
            }
            else if (m_runtimeOverride == Runtime.MockEnabled) //if override set to mock then spawn in mock prefabs
            {
                Debug.Log("Mock Override, spawning " + m_mockPrefabs.Length + " mock prefabs");
                foreach (GameObject mp in m_mockPrefabs)
                {
                    Instantiate(mp, m_mockTargetTransform);
                }
            }
        }

        //the teleportation provider that should be located on the prefab
#if UNITY_2023_1_OR_NEWER
        TeleportationProvider teleportationProvider = GameObject.FindFirstObjectByType<TeleportationProvider>();
#else
        TeleportationProvider teleportationProvider = GameObject.FindObjectOfType<TeleportationProvider>();
#endif

        //if no teleportation provider is found then return and print an error
        if (teleportationProvider == null)
        {
            Debug.LogError("ERROR: Teleportation provider not found, Some game aspects may not work correctly");
            return;
        }
#if UNITY_2023_1_OR_NEWER
        //getting arrays of all teleporter objects 
        TeleportationArea[] teleportationAreas = GameObject.FindObjectsByType<TeleportationArea>(FindObjectsSortMode.None);
        TeleportationAnchor[] teleportationAnchors = GameObject.FindObjectsByType<TeleportationAnchor>(FindObjectsSortMode.None);
#else
        //getting arrays of all teleporter objects 
        TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();
        TeleportationAnchor[] teleportationAnchors = GameObject.FindObjectsOfType<TeleportationAnchor>();
#endif
        //assigning references to teleportation areas
        foreach (TeleportationArea ta in teleportationAreas)
        {
            ta.teleportationProvider = teleportationProvider;
        }

        //assigning references to teleportation providers
        foreach (TeleportationAnchor tan in teleportationAnchors)
        {
            tan.teleportationProvider = teleportationProvider;
        }

        //additional references should be manually added here
    }
}
