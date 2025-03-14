using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[AddComponentMenu("XRLab/Generic VR/Grapple Hook")]
[RequireComponent (typeof(XRRayInteractor))]
public class XRIGrappleHook : MonoBehaviour
{
    [Tooltip("the XRRig that the script is attached to. If left unnasigned then script will use the root of the gameobject it is attached to")]
    [SerializeField] private GameObject XRRig;
    [Tooltip("The control that the script should listen for")]
    [SerializeField] private InputActionReference control;
    
    [SerializeField] private XRRayInteractor ray;
    [SerializeField] private float grappleSpeed = 0.5f;

    private bool isGrappled = false; //used to disable additional input while the player is grappling to a new location
    private bool inputPressed = false; //used to activate aditional checks for input and location data
    private Vector3 targetPos; //the position that the player should be going to next
    private Coroutine grappleToLocationRoutine; //the routine used to run the grapple system


    // Start is called before the first frame update
    void Awake()
    {
        if (XRRig.Equals(null)) //if left unassigned, try to get the reference by assuming script is attached to the XRRig
        {
            XRRig = gameObject.transform.root.GetComponent<GameObject>(); //getting the root position of the XRRig
        }

        ray = gameObject.GetComponent<XRRayInteractor>(); //getting the ray component

        grappleToLocationRoutine = StartCoroutine(GrappleToLocation()); //starting the coroutine
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    /// <summary>
    /// once this is active, will run concurrently with the rest of the 
    /// script and if there is a location to grapple to, will move the player in
    /// that direction. note this will stop running when it is disabled in 
    /// the hierarchy 
    /// </summary>
    /// <returns></returns>
    private IEnumerator GrappleToLocation()
    {
        while (gameObject.activeInHierarchy)
        {
            if (inputPressed)
            {
                isGrappled = true;

                while ((gameObject.transform.position - targetPos).magnitude > 0.5f)
                {
                    Vector3 newPos = (gameObject.transform.position - targetPos).normalized;
                    XRRig.transform.root.position -= newPos * grappleSpeed;
                    yield return new WaitForSeconds(0.01f);
                }
            }

            isGrappled = false;

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// when the object is enabled, will bind the controls
    /// tp the grapple function
    /// </summary>
    private void OnEnable()
    {
        control.action.started += Grapple;
        control.action.canceled += Grapple;
    }

    /// <summary>
    /// when the object is disabled, will remove 
    /// callbacks to the grapple function
    /// </summary>
    private void OnDisable()
    {
        control.action.started -= Grapple;
        control.action.canceled -= Grapple;
    }    
    
    /// <summary>
    /// this will be called by the input and will run any code
    /// and functions required for the grapple hook system
    /// </summary>
    /// <param name="context"></param>
    private void Grapple(InputAction.CallbackContext context)
    {
        FireRay();

        inputPressed = context.control.IsPressed();
    }

    /// <summary>
    /// uses the XRRay interactor to fire a raycast and update the 
    /// target position for the player to grapple to
    /// </summary>
    private void FireRay()
    {
        RaycastHit rayHit; //the container for the raycast information

        ray.TryGetCurrent3DRaycastHit(out rayHit); //attempting to get the RayHit from the raycase

        //rayHit.collider.gameObject.name = "ACTIVATED"; //debugging line

        if(!isGrappled) //stopping target change when actively grappling
            targetPos = rayHit.point; //setting the target position to the world position the raycast hit
    }
}
