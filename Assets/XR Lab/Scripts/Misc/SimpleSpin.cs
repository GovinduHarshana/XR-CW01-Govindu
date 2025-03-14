using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab;

[AddComponentMenu("XRLab/General/Simple Spin")]
public class SimpleSpin : MonoBehaviour
{
    [Tooltip("The speed of the rotation applied to the object")]
    [SerializeField] private float m_spinSpeed;

    [Tooltip("The target axis to spin around")]
    [SerializeField] private XRLabLib.VectorDirections m_rotationAxis;

    //[System.Serializable]
    //enum VectorDirections
    //{ 
    //    forward, backward, up, down, left, right
    //}
   

    /// <summary>
    /// rotates attached object around the specified axis each frame
    /// </summary>
    void Update()
    {
        switch (m_rotationAxis)
        {
            case XRLabLib.VectorDirections.forward:
                gameObject.transform.RotateAround(transform.position, transform.forward, m_spinSpeed);
                break;
            case XRLabLib.VectorDirections.backward:
                gameObject.transform.RotateAround(transform.position, -transform.forward, m_spinSpeed);
                break;
            case XRLabLib.VectorDirections.up:
                gameObject.transform.RotateAround(transform.position, transform.up, m_spinSpeed);
                break;
            case XRLabLib.VectorDirections.down:
                gameObject.transform.RotateAround(transform.position, -transform.up, m_spinSpeed);
                break;
            case XRLabLib.VectorDirections.left:
                gameObject.transform.RotateAround(transform.position, -transform.right, m_spinSpeed);
                break;
            case XRLabLib.VectorDirections.right:
                gameObject.transform.RotateAround(transform.position, transform.right, m_spinSpeed);
                break;
            default:
                break;
        }
    }
}
