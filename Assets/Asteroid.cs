using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public AudioClip collisionSound;

	// Use this for initialization
	void Start () {

    }
	
    public void PlayCollisionSound(float colliderMass = 0f, float mass = 0f)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (colliderMass == 0f)
            colliderMass = 0.2f;
        if (mass == 0f)
            mass = GetComponent<Rigidbody>().mass;
        audioSource.pitch = 0f + (2 * mass) + colliderMass; // 0.6..1.5 but assumes mass 0.2-0.5
        audioSource.PlayOneShot(collisionSound);
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        float colliderMass = collider.GetComponent<Rigidbody>().mass;
        float mass = GetComponent<Rigidbody>().mass;
        if (collider.gameObject.CompareTag("asteroid") && mass > colliderMass)
        {
            PlayCollisionSound(colliderMass, mass);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
