using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTest : MonoBehaviour {

    [SerializeField]
    private Vector3 initVelocity;


    Rigidbody rd;
    private void Awake()
    {
        rd = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start ()
    {
        if (rd)
        {
            rd.velocity = initVelocity;
        }
	}

    private void Update()
    {
        if (rd && Input.GetMouseButtonDown(0))
        {
            rd.velocity = -rd.velocity;
        }
    }
}
