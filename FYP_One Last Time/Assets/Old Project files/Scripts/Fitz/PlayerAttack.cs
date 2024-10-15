using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAtt;
    private float timeBtwLightAtt;
    private int lightAttCount;
    public float startTimeBtwAtt;
    public float startTimeBtwLightAtt;
    public float startTimeBtwHeavyAtt;

    public Transform attPos;
    public LayerMask enemies;
    public LayerMask enemyAttRange;
    public float range;
    private int dmg;
    public int dmgLight;
    public int dmgLightFinisher;
    public int dmgHeavy;

    private bool messageSent = false;

    private void Update()
    {
        if (timeBtwAtt <= 0)
        {
            // Light attack
            if (Input.GetKey(KeyCode.E))
            {
                if (timeBtwLightAtt <= 0)
                {
                    messageSent = false;
                    if (lightAttCount == 2)
                    {
                        timeBtwAtt = startTimeBtwAtt;
                        dmg = dmgLightFinisher;
                        Debug.Log("Light attack 3 combo, resetting.");
                        lightAttCount = 0;
                    }

                    else
                    {
                        lightAttCount++;
                        dmg = dmgLight;
                        Debug.Log("Light attack combo " + lightAttCount);
                    }

                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attPos.position, range, enemies);

                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        // Counter damage
                        if (enemiesToDamage[i].GetComponent<Enemy>().stun)
                            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(dmg * 2);
                        // Normal damage
                        else
                            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(dmg);
                    }

                    timeBtwLightAtt = startTimeBtwLightAtt;
                }
            }

            // Heavy attack
            if (Input.GetKey(KeyCode.Q))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attPos.position, range, enemies);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    // Counter damage
                    if (enemiesToDamage[i].GetComponent<Enemy>().stun)
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(dmgHeavy * 2);
                    // Normal damage
                    else
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(dmgHeavy);
                }

                lightAttCount = 0;
                if (!messageSent)
                {
                    Debug.Log("Heavy attack, resetting combo");
                    messageSent = true;
                }

                timeBtwAtt = startTimeBtwHeavyAtt;
            }
            timeBtwLightAtt -= Time.deltaTime;
        }
        else
        {
            timeBtwAtt -= Time.deltaTime;
        }

        if (timeBtwLightAtt <= 0 - (0.5 * startTimeBtwLightAtt))
        {
            lightAttCount = 0;
            if (!messageSent)
            {
                Debug.Log("Combo timer past cooldown point, resetting combo");
                messageSent = true;
            }
        }

        // Parry
        if (Input.GetKey(KeyCode.F))
        {
            Collider2D[] enemiesToParry = Physics2D.OverlapCircleAll(attPos.position, range, enemyAttRange);

            for (int i = 0; i < enemiesToParry.Length; i++)
            {
                enemiesToParry[i].GetComponentInParent<Enemy>().Stunned();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attPos == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attPos.position, range);
    }
}
