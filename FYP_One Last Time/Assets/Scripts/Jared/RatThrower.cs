using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatThrower : MonoBehaviour
{
    [SerializeField] private GameObject spear;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private BoxCollider col;
    private GameObject target;
    private Rigidbody rb;
    private Vector3 spawnLocation;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spawnLocation = throwPoint.transform.position;
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.transform.root.CompareTag("Player"))
        {
            Debug.Log("activated throw attack");
            col.enabled = false;
            if(col.enabled == false)
            {
                StartCoroutine(throwing());
            }
            else
            {
                col.enabled = false;
            }
        }
    }

    private IEnumerator throwing()
    {
        yield return new WaitForSeconds(3);
        Instantiate(spear, new Vector3(spawnLocation.x + 3,  spawnLocation.y, spawnLocation.z),Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        col.enabled = true;
    }
}
