using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableProp : MonoBehaviour
{    
    [SerializeField]
    private LayerMask playerHurtboxLayer;
    private Rigidbody rb;

    [SerializeField]
    private GameObject BrokenPrefab;

    [SerializeField]
    private float ExplosiveForce = 1000;
    [SerializeField]
    private float ExplosiveRadius = 2f;

    [SerializeField]
    private float PieceFadeSpeed = 0.25f;
    [SerializeField]
    private float PieceDestroyDelay = 5.0f;
    [SerializeField]
    private float PieceSleepCheckDelay = 0.5f;

    [SerializeField]
    private int health = 3;

    [SerializeField]
    private UnityEvent Hit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Explode()
    {
        if (rb != null)
        {
            Destroy(rb);
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        GameObject brokenInstance = Instantiate(BrokenPrefab, transform.position, transform.rotation);

        Rigidbody[] rigidbodies = brokenInstance.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody body in rigidbodies)
        {
            if (rb != null)
            {
                body.velocity = rb.velocity;
            }

            body.AddExplosionForce(ExplosiveForce, transform.position, ExplosiveRadius);
        }

        StartCoroutine(FadeOutRigidbodies(rigidbodies));
    }

    private IEnumerator FadeOutRigidbodies(Rigidbody[] rigidbodies)
    {
        WaitForSeconds Wait = new WaitForSeconds(PieceSleepCheckDelay);
        int activeRigidbodies = rigidbodies.Length;

        while (activeRigidbodies > 0)
        {
            yield return Wait;

            foreach (Rigidbody rigidbody in rigidbodies)
            {
                if (rigidbody.IsSleeping())
                {
                    activeRigidbodies--;
                }
            }
        }

        yield return new WaitForSeconds(PieceDestroyDelay);

        float time = 0;
        Renderer[] renderers = Array.ConvertAll(rigidbodies, GetRendererFromRigidbody);

        foreach(Rigidbody body in rigidbodies)
        {
            Destroy(body.GetComponent<Rigidbody>());
            Destroy(body);
        }

        while (time < 1)
        {
            float step = Time.deltaTime * PieceFadeSpeed;
            foreach(Renderer renderer in renderers)
            {
                renderer.transform.Translate(Vector3.down * (step / renderer.bounds.size.y), Space.World);
            }

            time += step;
            yield return null;
        }

        foreach(Renderer renderer in renderers)
        {
            Destroy(renderer.gameObject);
        }

        Destroy(gameObject);
    }

    private Renderer GetRendererFromRigidbody(Rigidbody rb)
    {
        return rb.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool playerHit = ((1 << other.gameObject.layer) & playerHurtboxLayer) != 0;

        if (playerHit)
        {
            health--;

            if (health <= 0)
            {
                Hit?.Invoke();
            }
        }
    }
}
