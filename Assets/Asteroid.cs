using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public AudioClip collisionSound;

	// Use this for initialization
	void Start () {

    }
	
    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        float colliderMass = collider.GetComponent<Rigidbody>().mass;
        float mass = GetComponent<Rigidbody>().mass;
        if (collider.gameObject.CompareTag("asteroid") && mass > colliderMass)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.pitch = 0f + (2 * mass) + colliderMass; // 0.6..1.5 but assumes mass 0.2-0.5
            audioSource.PlayOneShot(collisionSound);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
