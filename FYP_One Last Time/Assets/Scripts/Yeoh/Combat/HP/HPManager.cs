using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

        StartUpdatingUIBar();
    }

    // ============================================================================
    
    void Start()
    {
        EventM.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    void Update()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);     

        if(prevRegenInterval != regenInterval)
        {
            prevRegenInterval = regenInterval;

            // update hp regen rate
            StartHpRegen();
        }
    }

    // Regen ============================================================================

    [Header("Regeneration")]
    public bool regen;
    public bool regenWhenEmpty;

    public float regenHp=.2f;
    public float regenInterval=.1f;

    [HideInInspector] public float defaultRegenHp;
    [HideInInspector] public float defaultRegenInterval;
    float prevRegenInterval;

    void RecordDefaults()
    {
        defaultRegenHp=regenHp;
        defaultRegenInterval=regenInterval;
    }

    void StartHpRegen()
    {
        if(hpRegeneratingRt!=null) StopCoroutine(hpRegeneratingRt);
        hpRegeneratingRt = StartCoroutine(HpRegenerating());
    }

    Coroutine hpRegeneratingRt;
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

    // UI Bar ============================================================================

    void StartUpdatingUIBar()
    {
        if(updatingUIBarRt!=null) StopCoroutine(updatingUIBarRt);
        updatingUIBarRt = StartCoroutine(UpdatingUIBar());
    }

    Coroutine updatingUIBarRt;
    IEnumerator UpdatingUIBar()
    {
        while(true)
        {
            yield return new WaitForSeconds(.2f);
            EventM.OnUIBarUpdate(gameObject, hp, hpMax);
        }
    }

    // Setters ============================================================================

    public void Hurt(float dmg)
    {
        if(dmg>0)
        {
            if(hp>dmg) hp-=dmg;
            else hp=0;
        }
        
        EventM.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    public void Add(float amount)
    {
        hp+=amount;
        if(hp>hpMax) hp=hpMax;

        EventM.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    public void SetHPPercent(float percent)
    {
        percent = Mathf.Clamp(percent, .1f, 100);

        hp = hpMax * percent/100;

        EventM.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    // Getters ============================================================================

    public float GetHPPercent()
    {
        return hp/hpMax*100;
    }
}
