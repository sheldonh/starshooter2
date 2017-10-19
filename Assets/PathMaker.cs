using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour {

    public GameObject asteroidField;
    public GameObject ship;
    public float minSecs;
    public float maxSecs;

    // TODO refactor copy from Ship.cs
    float topY = 4.5f;
    float bottomY = -4.5f;
    Rigidbody rb;
    float secs;
    Vector3 direction;
    float speed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        direction = Vector3.zero;
        secs = maxSecs;
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("asteroid"))
        {
            Debug.Log("PathMaker cleared out an asteroid");
            asteroidField.GetComponent<AsteroidField>().LaunchAsteroid(other.gameObject);
        }
    }

	void Update () {
        if (ship == null)
            return;

        ChooseDirection();
        float thrustForce = ship.GetComponent<Ship>().thrustForce;
        float y = rb.position.y;
        if (!(direction == Vector3.zero || (y > topY && direction == Vector3.up) || (y < bottomY && direction == Vector3.down)))
        {
            rb.AddForce(direction * thrustForce * speed);
        }
    }

    void ChooseDirection ()
    {
        secs -= Time.deltaTime;
        Debug.Log(secs);
        if (secs < 0)
        {
            secs = Random.Range(minSecs, maxSecs);
            float flip = Random.Range(0, 100);
            if (flip > 80)
                direction = Vector3.up;
            else if (flip > 60)
                direction = Vector3.down;
            else
                direction = Vector3.zero;
            speed = Random.Range(0.25f, 1f);
        }
    }
}
