using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausible : MonoBehaviour {

    public MonoBehaviour controlScript;
    Vector3 pausedVelocity;
    Vector3 pausedAngularVelocity;
    enum State { Paused, Resumed };
    State state = State.Resumed;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
    }

    public void Pause()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (state == State.Resumed)
        {
            if (controlScript != null)
                controlScript.enabled = false;
            pausedVelocity = rb.velocity;
            pausedAngularVelocity = rb.angularVelocity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            state = State.Paused;
        }
    }

    public void Resume()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (state == State.Paused)
        {
            if (controlScript != null)
                controlScript.enabled = true;
            rb.WakeUp();
            rb.velocity = pausedVelocity;
            rb.angularVelocity = pausedAngularVelocity;
            state = State.Resumed;
        }
    }
}
