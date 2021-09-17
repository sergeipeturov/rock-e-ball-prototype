using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnshieldCube : Defeatable, IRepulsive
{
    private Rigidbody rb;

    public void Repulse(Vector3 direction, float speed)
    {
        rb.AddForce(direction.normalized);// * (speed / 2));
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        Health = 50.0f;
        DamageMultiplier = 0.5f;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
