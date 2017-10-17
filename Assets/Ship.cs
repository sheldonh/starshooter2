using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float thrustForce = 1f;
    public Game game;
    public GameObject explosionPrefab;
    public AudioClip collisionSound;

    Rigidbody rb;
    float topY = 4.5f;
    float bottomY = -4.5f;
    Vector3 lastTorque = Vector3.zero;
    Quaternion levelRotation;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        levelRotation = rb.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((Input.GetKey("up") || Input.GetKey("q")) && !(Input.GetKey("down") || Input.GetKey("a")) && rb.position.y < topY)
        {
            rb.AddForce(Vector3.up * thrustForce);
            rb.AddTorque(Vector3.right * thrustForce);
            lastTorque = Vector3.right;
        }
        else if ((Input.GetKey("down") || Input.GetKey("a")) && !(Input.GetKey("up") || Input.GetKey("q")) && rb.position.y > bottomY)
        {
            rb.AddForce(Vector3.down * thrustForce);
            rb.AddTorque(Vector3.left * thrustForce);
            lastTorque = Vector3.left;
        } else
        {
            if (Quaternion.Angle(transform.rotation, levelRotation) < 90.0f)
            {
                // //With Slerp:
                transform.rotation = Quaternion.Slerp(transform.rotation, levelRotation, 0.1f);

                // //Without Slerp:
                //Quaternion rot = Quaternion.Inverse(transform.rotation);
                //rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * thrustForce);
            }
            else
            {
                if (lastTorque == Vector3.left)
                    rb.AddTorque(Vector3.left * thrustForce);
                else if (lastTorque == Vector3.right)
                    rb.AddTorque(Vector3.right * thrustForce);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("asteroid"))
        {
            GetComponent<AudioSource>().PlayOneShot(collisionSound, 1f);
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.GetComponent<Transform>().SetPositionAndRotation(rb.position, rb.rotation);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            ps.Play();
            Destroy(ps, 5.0f);
            transform.position = new Vector3(-20, 0, 0);
            game.GetComponent<Game>().SetStateGameOver();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("shipSpace"))
        {
            Debug.Log("Ship exited shipSpace");
            rb.AddForce(-rb.velocity);
        }
    }

    public void Reset()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(-7 , 0, 0);
    }
}
