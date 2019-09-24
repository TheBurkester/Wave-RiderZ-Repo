using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceJoint : MonoBehaviour
{
    public Rigidbody ConnectedRigidBody;
    public bool DetermineDistance = true;
    public float Distance;
    public float Spring = 0.1f;
    public float Damper = 5f;

    protected Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (DetermineDistance)
        {
            Distance = Vector3.Distance(rb.position, ConnectedRigidBody.position);
        }
    }

    void FixedUpdate()
    {
        Vector3 Connection = rb.position - ConnectedRigidBody.position;
        float DistanceDis = Distance - Connection.magnitude;

        rb.position += DistanceDis * Connection.normalized;

        Vector3 VelocityTarget = Connection + (rb.velocity + Physics.gravity * Spring);
        Vector3 ProjectOnConnection = Vector3.Project(VelocityTarget, Connection);

        rb.velocity = (VelocityTarget - ProjectOnConnection) / (1 + Damper * Time.fixedDeltaTime);
    }
}
