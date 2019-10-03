using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float size;

    public GameObject boidPrefab;
    public int numBoid = 10;

    [Range(0, 3)]
    public float alignementCoef = 1, cohesionCoef = 1, separationCoef = 1;

    public float separationDistance = 3f;
    public float neighborDistance = 10f;

    [HideInInspector]
    public List<Boid> boids = new List<Boid>();

    private void Start()
    {
        for (int i = 0; i < numBoid; i++)
        {
            Boid boid = Instantiate(boidPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Boid>();
            boid.box = this;
            boid.transform.position = new Vector3(Random.Range(-size / 2f, size / 2f), Random.Range(-size / 2f, size / 2f), Random.Range(-size / 2f, size / 2f));
            boids.Add(boid);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(size, size, size));
    }
}
