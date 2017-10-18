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
    Bounds launchBounds;
    float launchMaxX;
    Stack<GameObject> spareAsteroids = new Stack<GameObject>();
    Stack<GameObject> asteroids = new Stack<GameObject>();

    // Use this for initialization
    void Start ()
    {
        launchBounds = GetLaunchBounds();
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
        foreach (GameObject asteroid in asteroids)
        {
            asteroid.GetComponent<Pausible>().Pause();
        }
        enabled = false;
    }

    public void Resume ()
    {
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
        t.SetPositionAndRotation(RandomPositionInBounds(launchBounds), Quaternion.identity);
        rb.mass = Random.Range(minSize, maxSize);
        t.localScale = Vector3.one * rb.mass;
        rb.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float speed = Random.Range(5f, 7f);
        if (atPlayer && ship != null)
        {
            rb.velocity = (ship.GetComponent<Rigidbody>().position - rb.position).normalized * speed;
        } else
        {
            rb.velocity = new Vector3(-1f, Random.Range(-0.1f, 0.1f), 0f) * speed;
        }
        
        asteroid.SetActive(true);
    }

    Bounds GetLaunchBounds()
    {
        Bounds b = GetComponent<Collider>().bounds;
        Bounds launchBounds = new Bounds(
            new Vector3(b.center.x + (b.size.x / 6), b.center.y, b.center.z),
            new Vector3(b.size.x * 2 / 3, b.size.y, 0));
        Debug.Log("Collider bounds: " + b);
        Debug.Log("Launch bounds:   " + launchBounds);
        return launchBounds;
    }

    Vector3 RandomPositionInBounds(Bounds b)
    {
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            Random.Range(b.min.z, b.max.z));
    }
}
