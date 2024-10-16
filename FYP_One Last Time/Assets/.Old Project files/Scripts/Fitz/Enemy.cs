using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    private SpriteRenderer sprite;

    private float timeBtwAtt;
    public float startTimeBtwAtt;
    private float stunTimer;
    public float stunTimerDuration;
    public float attackTime;

    public GameObject attObj;

    public bool stun = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stun)
        {
            if (timeBtwAtt <= 0)
            {
                StartCoroutine(EnemyAttack());

                timeBtwAtt = startTimeBtwAtt;
            }

            else
            {
                timeBtwAtt -= Time.deltaTime;
            }
        }

        else
        {
            if (stunTimer <= 0)
            {
                stun = false;
                sprite.color = Color.cyan;
            }

            else
            {
                stunTimer -= Time.deltaTime;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            Debug.Log("Take " + damage + " Dmg, health now at " + health);
            if (health <= 0)
            {
                sprite.color = Color.magenta;
            }
        }
    }

    private IEnumerator EnemyAttack()
    {
        attObj.SetActive (true);
        yield return new WaitForSeconds(attackTime);
        attObj.SetActive(false);
    }

    public void Stunned()
    {
        stun = true;
        stunTimer = stunTimerDuration;
        sprite.color = Color.yellow;
    }
}
