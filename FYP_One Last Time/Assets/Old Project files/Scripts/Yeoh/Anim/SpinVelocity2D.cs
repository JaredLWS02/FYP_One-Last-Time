using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinVelocity2D : MonoBehaviour
{
    Rigidbody2D rb;

    public Vector2 speedMult = new Vector2(-360, 360);
    float mult;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();

        mult = Random.Range(speedMult.x, speedMult.y);
    }

    void Update()
    {
        transform.eulerAngles = new Vector3
        (
            0,
            0,
            transform.eulerAngles.z + rb.velocity.magnitude * Time.deltaTime * mult
        );
    }
}
