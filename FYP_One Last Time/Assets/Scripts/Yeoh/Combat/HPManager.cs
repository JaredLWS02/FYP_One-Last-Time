using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public float hp=100;
    public float hpMax=100;

    void Awake()
    {
        RecordDefaults();
    }

    // ============================================================================
    
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        StartHpRegen();
    }

    // Regen ============================================================================
    
    void Update()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);

        EventM.OnUIBarUpdate(gameObject, hp, hpMax);

        if(prevRegenInterval != regenInterval)
        {
            prevRegenInterval = regenInterval;
            
            StartHpRegen(); // update hp regen rate
        }
    }

    // ============================================================================

    [Header("Regeneration")]
    public bool regen=true;
    public bool regenWhenEmpty;

    public float regenHp=.2f;
    public float regenInterval=.1f;

    public float defaultRegenHp {get; private set;}
    public float defaultRegenInterval {get; private set;}
    float prevRegenInterval;

    void RecordDefaults()
    {
        defaultRegenHp=regenHp;
        defaultRegenInterval=regenInterval;
    }

    void StartHpRegen()
    {
        if(hpRegenerating_crt!=null) StopCoroutine(hpRegenerating_crt);
        hpRegenerating_crt = StartCoroutine(HpRegenerating());
    }

    Coroutine hpRegenerating_crt;
    IEnumerator HpRegenerating()
    {
        while(true)
        {
            yield return new WaitForSeconds(regenInterval);

            if(hp<hpMax && (hp>0 || regenWhenEmpty) )
            {
                if(regen) Add(regenHp);
            }
        }
    }

    // Setters ============================================================================

    public void Deplete(float dmg)
    {
        if(dmg<=0) return;

        if(hp>dmg) hp-=dmg;
        else hp=0;
    }

    public void Add(float amount)
    {
        hp += amount;
        if(hp>hpMax) hp=hpMax;
    }

    public void SetHPPercent(float percent)
    {
        percent = Mathf.Clamp(percent, .01f, 100);

        hp = hpMax * percent/100;
    }

    // Getters ============================================================================

    public float GetHPPercent()
    {
        return hp/hpMax*100;
    }
}
