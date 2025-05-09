using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BreakablePot : MonoBehaviour
{
    public ParticleSystem breakParticles;
    public AudioClip breakSound;
    private Rigidbody rb;
    private Animator anim;
    private XRGrabInteractable grabInteractable;
    private bool isBroken = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void BreakPot()
    {
        if (isBroken) return;

        // Notify game manager
        PotBreakingGame.Instance.PotBroken();

        isBroken = true;

        // Disable interaction
        if (grabInteractable != null)
            grabInteractable.enabled = false;

        // Play effects
        if (breakParticles != null)
        {
            ParticleSystem particles = Instantiate(breakParticles, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }

        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

        // Disable the pot
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Break immediately on any stick contact
        if (collision.gameObject.CompareTag("Stick"))
        {
            BreakPot();
        }
    }
}
