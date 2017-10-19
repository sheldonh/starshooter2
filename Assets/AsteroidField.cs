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
    public float minSpeed;
    public float maxSpeed;
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
            StoreAsteroid(asteroid);
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
            StoreAsteroid(asteroid);
        }
        for (int i = 0; i < initialAsteroids; i++)
        {
            GameObject asteroid = spareAsteroids.Pop();
            asteroids.Push(asteroid);
            LaunchAsteroid(asteroid);
        }
        Pause();
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag("asteroid"))
        {
            if (!game.GetComponent<Game>().ScoreUp(1))
            {
                LaunchAsteroid(other.gameObject);
                relaunches += 1;
                if (relaunches % relaunchesPerSpare == 0)
                {
                    if (spareAsteroids.Count > 0)
                    {
                        GameObject asteroid = spareAsteroids.Pop();
                        asteroids.Push(asteroid);
                        LaunchAsteroid(asteroid, true);
                    }
                }
            }
        }
    }

    void StoreAsteroid (GameObject asteroid)
    {
        asteroid.SetActive(false);
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        Transform t = asteroid.GetComponent<Transform>();
        rb.velocity = Vector3.zero;
        t.SetPositionAndRotation(new Vector3(50f, 0f, 0f), Quaternion.identity);
    }

    void LaunchAsteroid (GameObject asteroid, bool atPlayer = false)
    {
        asteroid.SetActive(false);
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        Transform t = asteroid.GetComponent<Transform>();
        t.SetPositionAndRotation(RandomPositionInBounds(launchBounds), Quaternion.identity);
        rb.mass = Random.Range(minSize, maxSize);
        t.localScale = Vector3.one * rb.mass;
        rb.angularVelocity = RandomSpin();
        float speed = Random.Range(minSpeed, maxSpeed);
        if (atPlayer && ship != null)
        {
            rb.velocity = (ship.GetComponent<Rigidbody>().position - rb.position).normalized * speed;
        } else
        {
            rb.velocity = new Vector3(-1f, Random.Range(-0.1f, 0.1f), 0f).normalized * speed;
        }
        
        asteroid.SetActive(true);
    }

    Vector3 RandomSpin ()
    {
        return new Vector3(
            RandomSign() * Mathf.Log(Random.Range(1f, 10f), 3f),
            RandomSign() * Mathf.Log(Random.Range(1f, 10f), 3f),
            RandomSign() * Mathf.Log(Random.Range(1f, 10f), 3f));
    }

    int RandomSign()
    {
        return Random.Range(0, 2) * 2 - 1;
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
