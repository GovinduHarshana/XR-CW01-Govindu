using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectController : MonoBehaviour
{    
    [SerializeField] private bool respawnIfUnderMap = true;
    [SerializeField] private float minYPos = -10f;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = gameObject.transform.position;
        startingRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < minYPos && respawnIfUnderMap)
        {
            Debug.Log("trying to return to " + startingPosition);
            ReturnToStartPosition();
        }   
    }

    public void ReturnToStartPosition()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        gameObject.transform.position = startingPosition;
        gameObject.transform.rotation = startingRotation;

        gameObject.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);

        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        Debug.Log("returning to start");
        
    }

    public void AdjustVelocity(Vector3 velocity, Vector3 angularVelocity)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

        gameObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
    }
}
