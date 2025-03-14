using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRLab;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(LineRenderer))]
public class XRIHandheldToolgun : MonoBehaviour
{
    [Header("Shooting Parameters")]
    [Tooltip("The position the projectile should spawn at. Also affects fire rotation. If unassigned, transform if attached object is used")]
    [SerializeField] Transform m_shootPos;
    [Tooltip("How fast should the gun fire")]
    [SerializeField] float m_fireRate = 0.1f;
    [Tooltip("The force applied to rigidbodies when shot by the raycast")]
    [SerializeField] float m_raycastForce = 100;
    [Tooltip("Misc")]
    [SerializeField] AudioSource m_audioSource;
    
    [Header("VR Editor References")]
    [SerializeField] XRIObjectComponentManager m_objectManager;

    XRGrabInteractable _InteractableBase; //used to reference the interactor that activates the gun
    float _TriggerHeldTime = 999f; //used to count time between shots
    bool _TriggerDown; //used to check if trigger is down in the update
    LineRenderer _tracerLine; //the line renderer used to show where the ray is firing
    
    GameObject _raycastCollider; //used for a crude workaround to allow onTriggerEnter to function with the raycast

    void Start()
    {
        _raycastCollider = new GameObject();
        SphereCollider col = _raycastCollider.AddComponent<SphereCollider>();
        col.radius = 0.1f;
        col.isTrigger = true;

        _raycastCollider.AddComponent<Rigidbody>().isKinematic = true;

        _tracerLine = GetComponent<LineRenderer>();

        //if shootPos is null then assign attached gameObject transform
        if (m_shootPos == null)
            m_shootPos = gameObject.transform;

        if(m_audioSource == null)
            m_audioSource = GetComponent<AudioSource>();

        if(m_objectManager == null)
            m_objectManager = GameObject.FindAnyObjectByType<XRIObjectComponentManager>();
        
        //Get a reference to the grab interactable 
        _InteractableBase = GetComponent<XRGrabInteractable>();

        //set up listeners
        _InteractableBase.selectExited.AddListener(DroppedGun);
        _InteractableBase.activated.AddListener(TriggerPulled);
        _InteractableBase.deactivated.AddListener(TriggerReleased);
    }

    void FixedUpdate()
    {
        if (_TriggerDown)
        {
            _TriggerHeldTime += Time.deltaTime;

            if (_TriggerHeldTime >= m_fireRate)
            {
                // if an audio source is assigned, play a sound
                if (m_audioSource != null)
                    m_audioSource.Play();

                _tracerLine.enabled = true;

                ShootRay(); // shoot a ray

                _TriggerHeldTime = 0; //reset to zero for auto fire


            }
            else
            {
                _tracerLine.enabled = false;
                _raycastCollider.transform.position = transform.position; //call the collider back to the hand so it is only sent out when firing
            }
        }
        else
        {
            _TriggerHeldTime = 0; //reset to zero for auto fire
        }
    }

    /// <summary>
    /// shoot raycast and apply force to rigidbody
    /// components of objects the raycase hits
    /// 
    /// Largely based off code available at 
    /// <href>https://learn.unity.com/tutorial/let-s-try-shooting-with-raycasts</href>
    /// 
    /// If you want to make a shooter game, this is the main 
    /// function you need to edit
    /// </summary>
    void ShootRay()
    {
        // Declare a raycast hit to store information about what our raycast has hit
        RaycastHit hit;

        // Set the start position for our visual effect for our laser to the position of gunEnd
        _tracerLine.SetPosition(0, m_shootPos.position);

        // Check if our raycast has hit anything
        if (Physics.Raycast(m_shootPos.position, m_shootPos.forward, out hit, 2000))
        {
            Debug.Log("raycast running");
            // Check if the object we hit has a rigidbody attached
            if (hit.rigidbody != null)
            {
                //get reference to the gamobject we just shot
                Debug.Log(hit.rigidbody.gameObject.name + " was shot at"); //pass the reference to the wherever

                m_objectManager.SetSelectedGameObject(hit.rigidbody.gameObject);
                // Add force to the rigidbody we hit, in the direction from which it was hit
                hit.rigidbody.AddForce(-hit.normal * m_raycastForce);
            }
        }

        _tracerLine.SetPosition(1, m_shootPos.forward * 2000);
    }

    void TriggerReleased(DeactivateEventArgs args)
    {
        _TriggerDown = false;
        _TriggerHeldTime = 0f;
    }

    void TriggerPulled(ActivateEventArgs args)
    {
        _TriggerDown = true;
    }

    void DroppedGun(SelectExitEventArgs args)
    {
        _TriggerDown = false;
        _TriggerHeldTime = 0f;
    }
}
