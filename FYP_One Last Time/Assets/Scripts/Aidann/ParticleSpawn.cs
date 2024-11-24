using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;

    public void Spawn()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}
