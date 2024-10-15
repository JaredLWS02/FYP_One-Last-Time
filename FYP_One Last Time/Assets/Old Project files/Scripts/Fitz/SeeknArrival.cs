using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeknArrival : MonoBehaviour
{
    float speed = 5f;
    float slowRad = 2.0f;
    float stopRad = 1.0f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("X"))
        //{
        //    float distance = (target.position - this.transform.position).magnitude;
        //    float direction = distance / distance;

        //    if (distance < slowRad)
        //    {
        //        speed *= distance / slowRad;
        //    }

        //    if (distance < stopRad)
        //    {
        //        speed = 0f;
        //    }

        //    rb.velocity = new Vector2(direction*speed, rb.velocity.y);
        //}

        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector2 direction = (target.position - this.transform.position).normalized;
            float distance = (target.position - this.transform.position).magnitude;

            if (distance < stopRad)
            {
                speed = 0f;
            }
            else if (distance < slowRad)
            {
                speed = Mathf.Lerp(0, speed, distance / slowRad);
            }

            rb.velocity = direction * speed;
        }
    }
}
