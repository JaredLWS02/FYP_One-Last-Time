using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatRoller : MonoBehaviour
{

    [SerializeField] private float rotz;
    [SerializeField] private float speed;
    private Rigidbody rb;
    private float dirX;
    private Vector3 localScale;
    private bool facingRight;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        rotz = -30.0f;
        speed = 15.0f;
        StartCoroutine(StartRow());
        facingRight = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0.0f,0.0f,rotz);
        rb.velocity = new Vector3(dirX * speed, rb.velocity.y, rb.velocity.z);
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.CompareTag("turnright"))
        {
            rotz = -30.0f;
            StartCoroutine(Chargingright());
        }

        if (c.CompareTag("turnleft"))
        {
            rotz = 30.0f;
            StartCoroutine(Chargingleft());
        }
    }

    void faceDir()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;
        transform.localScale = localScale;
    }

    IEnumerator StartRow()
    {
        yield return new WaitForSeconds(1.0f);
        dirX = 1f;
    }

    IEnumerator Chargingleft()
    {
        yield return new WaitForSeconds(1.5f);
        dirX *= -1f;
    }

    IEnumerator Chargingright()
    {
        yield return new WaitForSeconds(1.5f);
        dirX *= -1f;
    }
}
