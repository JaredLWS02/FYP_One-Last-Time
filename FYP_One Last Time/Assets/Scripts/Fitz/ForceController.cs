using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForceController : MonoBehaviour
{
    public GameObject owner;

    public bool pull;
    public float strength;
    public float range;
    public float radius;

    public Collider[] targetColls;

    public HurtboxSO stunboxSO;

    // Update is called once per frame
    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.G))
    //     {
    //         Force(true);
    //     }

    //     if(Input.GetKeyDown(KeyCode.F))
    //     {
    //         Force(false);
    //     }

    //     DrawRaycastAndRadius();
    // }

    public void GetObjects()
    {
        targetColls = null;
        RaycastHit hit;
        if(Physics.Raycast(owner.transform.position, owner.transform.forward, out hit, range))
        {
            targetColls = Physics.OverlapSphere(hit.point, radius);
        }
    }

    public void Force(bool pull)
    {
        GetObjects();

        if (targetColls != null && targetColls.Length > 0)
        {
            foreach(var coll in targetColls)
            {
                var other_rb = coll.attachedRigidbody;
                if (!other_rb) continue;

                int mult = pull ? 1 : -1;

                Vector3 direction = (owner.transform.position - other_rb.transform.position).normalized;

                Debug.Log("Object Tag: " + other_rb.gameObject.tag);

                if (other_rb && other_rb.gameObject.CompareTag("Enemy"))
                { 
                    Debug.Log("Force applied");
                    // Do Tikus stuff here
                    EventManager.Current.OnTryStun(other_rb.gameObject, owner, stunboxSO, owner.transform.position);
                    // End tikus stuff
                    //other_rb.velocity = direction * strength * mult * Time.deltaTime;
                    other_rb.AddForce(direction * strength * mult, ForceMode.Impulse);
                }
            }
        }
    }

    private void DrawRaycastAndRadius()
    {
        // Draw the ray in the scene view
        Debug.DrawRay(owner.transform.position, owner.transform.forward * range, Color.green);
    }
}
