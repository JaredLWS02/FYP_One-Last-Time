using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject gObject;

    public void Spawn()
    {
        Instantiate(gObject, transform.position, Quaternion.identity);
    }
}
