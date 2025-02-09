using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPan : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    
    void OntriggerStay(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            cam.transform.position = player.transform.position + offset;
        }
    }
}
