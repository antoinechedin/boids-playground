using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;

    public float maxSpeed, minSpeed;
    public float maxForce;

    public Transform target;
    public Box box;

    private void Start()
    {
        velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * (maxSpeed + minSpeed) / 2f ;
    }

    private void Update()
    {
        Vector3 acceleration = Vector3.zero;

        acceleration += box.alignementCoef * Alignement();
        acceleration += box.cohesionCoef * Cohesion();
        acceleration += box.separationCoef * Separation();

        velocity += acceleration;
        float speed = Mathf.Clamp(velocity.magnitude, minSpeed, maxSpeed);
        transform.position += velocity.normalized * speed;

        transform.LookAt(transform.position + velocity);

        // Teleport boid
        if (transform.position.x > box.size / 2f)
            transform.position += new Vector3(-box.size, 0, 0);
        if (transform.position.x < -box.size / 2f)
            transform.position += new Vector3(box.size, 0, 0);
        if (transform.position.y > box.size / 2f)
            transform.position += new Vector3(0, -box.size, 0);
        if (transform.position.y < -box.size / 2f)
            transform.position += new Vector3(0, box.size, 0);
        if (transform.position.z > box.size / 2f)
            transform.position += new Vector3(0, 0, -box.size);
        if (transform.position.z < -box.size / 2f)
            transform.position += new Vector3(0, 0, box.size);
    }

    public Vector3 Alignement()
    {
        Vector3 avgVelocity = Vector3.zero;
        int numNeighbor = 0;
        foreach (Boid boid in box.boids)
        {
            if (boid != this)
            {
                float t = (boid.transform.position - transform.position).magnitude;
                if (t < box.separationDistance)
                {
                    avgVelocity += boid.velocity;
                    numNeighbor++;
                }
            }
        }
        if (numNeighbor > 0)
        {
            avgVelocity /= numNeighbor;

            return Steer(transform.position + avgVelocity);
        }
        return Vector3.zero;
    }

    public Vector3 Cohesion()
    {
        Vector3 centerOfNeighbors = Vector3.zero;
        int numNeighbor = 0;
        foreach (Boid boid in box.boids)
        {
            if (boid != this)
            {
                float t = (boid.transform.position - transform.position).magnitude;
                if (t < box.neighborDistance)
                {
                    centerOfNeighbors += boid.transform.position;
                    numNeighbor++;
                }
            }
        }
        if (numNeighbor > 0)
        {
            centerOfNeighbors /= numNeighbor;
            return Steer(centerOfNeighbors);
        }
        return Vector3.zero;
    }

    public Vector3 Separation()
    {

        Vector3 centerOfNeighbors = Vector3.zero;
        int numNeighbor = 0;
        foreach (Boid boid in box.boids)
        {
            if (boid != this)
            {
                float t = (boid.transform.position - transform.position).magnitude;
                if (t < box.separationDistance)
                {
                    centerOfNeighbors += boid.transform.position;
                    numNeighbor++;
                }
            }
        }
        if (numNeighbor > 0)
        {
            centerOfNeighbors /= numNeighbor;


            return Steer(transform.position - (centerOfNeighbors - transform.position));
        }
        return Vector3.zero;
    }

    public Vector3 Steer(Vector3 target)
    {
        Vector3 desired = (target - transform.position);
        desired = desired.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude(desired, maxForce);
    }
}
