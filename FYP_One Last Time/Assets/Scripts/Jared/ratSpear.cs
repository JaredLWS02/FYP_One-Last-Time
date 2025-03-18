using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratSpear : MonoBehaviour
{
    private GameObject target;
    private Rigidbody rb;
    private float force;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        force = 10.0f;
        StartCoroutine(selfDestruct());

        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.root.CompareTag("Player"))
        {
            //deal dmg
            Destroy(gameObject);
        }
    }

    private IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
