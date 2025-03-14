using UnityEngine;

[AddComponentMenu("XRLab/Generic VR/VR Player Blocker")]
public class VRPlayerBlocker : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("The collider that is attached to the same object as the script. if not assigned to directly the script will attempt to find the collider")]
    [SerializeField] private Collider m_attachedCollider;
    [Tooltip("The collider that is attached to the main camera. if not assigned to directly the script will attempt to find the collider itself")]
    [SerializeField] private Collider m_playerCollider;
    [Tooltip("the gameobject that indicates the direction the object will push the player if a collision is detected, if not assigned the transform from the object the script is on will be used")]
    [SerializeField] private Transform m_pushDirectionMarker;

    [Header("Additional Settings")]
    [Tooltip("the multiplier that is applied to the force vector that moves the player back")]
    [SerializeField] private float m_pushBackMultiplier = 1f;
    [Tooltip("if true this will move in the direction indicated by the marker. if false will push outward from the centre (best for smaller objects)")]
    [SerializeField] private bool pushInMarkerDirection = false; 
    
    /// <summary>
    /// assigns references automatically if they are not already assigned
    /// </summary>
    void Awake()
    {
        if(m_attachedCollider == null) //check if there is a valid collider assigned
            m_attachedCollider = GetComponent<Collider>(); //if there is not a collider, attempt to assign it

        if(m_playerCollider == null) //check if the player has a collider
            m_playerCollider = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Collider>(); //if there is not a collider, attempt to assign it

        if (m_pushDirectionMarker == null) //check if the push direction marker is assigned
            m_pushDirectionMarker = gameObject.transform; //if it is not assigned, use the attached object instead
    }

    /// <summary>
    /// Checks if player is intersecting with the target object and 
    /// pushes them in a given direction based on user settings 
    /// </summary>
    void FixedUpdate()
    {
        if (m_attachedCollider.bounds.Intersects(m_playerCollider.bounds)) //if the player collider is colliding with the attached collider
        {
            if (pushInMarkerDirection) //if the player has chosen to push in the direction of the marker
            {
                //push the player in the direction the marker faces
                m_playerCollider.gameObject.transform.root.position += m_pushDirectionMarker.up * m_pushBackMultiplier;
            }
            else //if the player does not want to use the push marker
            {
                //get a normalised vector that faces in the opposite direction to the player in relation to the attached object
                Vector3 newPos = (gameObject.transform.position - m_playerCollider.gameObject.transform.root.position).normalized;
                //set the Y position to the Y position of the player to prevent floating or falling
                newPos.y = m_playerCollider.gameObject.transform.root.position.y;

                //push the player with an additional force multiplier
                m_playerCollider.gameObject.transform.root.position -= newPos * m_pushBackMultiplier;
            }

        }
    }
}
