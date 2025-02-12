using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForceController : MonoBehaviour
{
    public bool pull;
    public float strength;
    public float range;
    public float radius;

    public Collider[] targetObj;

    public Transform holdPos;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Force(true);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            Force(false);
        }

        DrawRaycastAndRadius();
    }

    public void GetObjects()
    {
        targetObj = null;
        RaycastHit hit;
        if(Physics.Raycast(holdPos.position, holdPos.TransformDirection(Vector3.forward), out hit, range))
        {
            targetObj = Physics.OverlapSphere(hit.point, radius);
        }
    }

    public void Force(bool pull)
    {
        GetObjects();
        if (targetObj != null && targetObj.Length > 0)
        {
            foreach(Collider col in targetObj)
            {
                var other_rb = col.attachedRigidbody;
                if (!other_rb) continue;

                int mult = pull ? 1 : -1;

                Vector3 direction = (holdPos.position - other_rb.transform.position).normalized;

                Debug.Log("Object Tag: " + other_rb.gameObject.tag);

                if (other_rb && other_rb.gameObject.CompareTag("Enemy"))
                { 
                    Debug.Log("Force applied");
                    // Do Tikus stuff here

                    // End tikus stuff
                    other_rb.velocity = direction * strength * mult * Time.deltaTime;
                }
            }
        }
    }

    private void DrawRaycastAndRadius()
    {
        // Draw the ray in the scene view
        Debug.DrawRay(holdPos.position, holdPos.TransformDirection(Vector3.forward) * range, Color.green);
    }
}
