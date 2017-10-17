using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour {

    public GameObject asteroidPrefab;
    public int totalAsteroids;
    public int initialAsteroids;
    public int relaunchesPerSpare;
    public float minSize;
    public float maxSize;
    public Game game;
    public GameObject ship;

    int relaunches;
    Stack<GameObject> spareAsteroids = new Stack<GameObject>();
    Stack<GameObject> asteroids = new Stack<GameObject>();

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < totalAsteroids; i++)
        {
            GameObject asteroid = Instantiate<GameObject>(asteroidPrefab);
            asteroid.transform.SetParent(GetComponent<Transform>(), true);
            asteroid.tag = "asteroid";
            spareAsteroids.Push(asteroid);
            storeAsteroid(asteroid);
        }
    }

    // Update is called once per frame
    void Update ()
    {
	}

    public void Pause ()
    {
        Debug.Log("AsteroidField pausing asteroids");
        foreach (GameObject asteroid in asteroids)
        {
            asteroid.GetComponent<Pausible>().Pause();
        }
        enabled = false;
    }

    public void Resume ()
    {
        Debug.Log("AsteroidField resuming asteroids");
        foreach (GameObject asteroid in asteroids)
        {
            asteroid.GetComponent<Pausible>().Resume();
        }
        enabled = true;
    }

    public void Reset()
    {
        relaunches = 0;

        while (asteroids.Count > 0)
        {
            GameObject asteroid = asteroids.Pop();
            spareAsteroids.Push(asteroid);
            storeAsteroid(asteroid);
        }
        for (int i = 0; i < initialAsteroids; i++)
        {
            GameObject asteroid = spareAsteroids.Pop();
            asteroids.Push(asteroid);
            launchAsteroid(asteroid);
        }
        Pause();
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag("asteroid"))
        {
            if (!game.GetComponent<Game>().ScoreUp(1))
            {
                launchAsteroid(other.gameObject);
                relaunches += 1;
                if (relaunches % relaunchesPerSpare == 0)
                {
                    if (spareAsteroids.Count > 0)
                    {
                        GameObject asteroid = spareAsteroids.Pop();
                        asteroids.Push(asteroid);
                        launchAsteroid(asteroid, true);
                    }
                }
            }
        }
    }

    void storeAsteroid (GameObject asteroid)
    {
        asteroid.SetActive(false);
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        Transform t = asteroid.GetComponent<Transform>();
        rb.velocity = Vector3.zero;
        t.SetPositionAndRotation(new Vector3(50f, 0f, 0f), Quaternion.identity);
    }

    void launchAsteroid (GameObject asteroid, bool atPlayer = false)
    {
        asteroid.SetActive(false);
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        Transform t = asteroid.GetComponent<Transform>();
        t.SetPositionAndRotation(new Vector3(Random.Range(11.5f, 30f), Random.Range(-4.5f, 4.5f), 0f), Quaternion.identity);
        rb.mass = Random.Range(minSize, maxSize);
        t.localScale = Vector3.one * rb.mass;
        rb.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float speed = Random.Range(5f, 7f);
        if (atPlayer)
        {
            rb.velocity = (ship.GetComponent<Rigidbody>().position - rb.position).normalized * speed;
        } else
        {
            rb.velocity = new Vector3(-1f, Random.Range(-0.1f, 0.1f), 0f) * speed;
        }
        
        asteroid.SetActive(true);
    }
}
