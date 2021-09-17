using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableShieldCube : MonoBehaviour, IRepulsive
{
    private Rigidbody rb;

    public void Repulse(Vector3 direction, float speed)
    {
        rb.AddForce(direction.normalized);// * (speed / 2));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
