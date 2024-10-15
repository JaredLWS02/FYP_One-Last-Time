using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    [HideInInspector]
    public InventorySlot slot;
    
    // ============================================================================

    void Update()
    {
        UpdateIcon();
        UpdateCount();
    }

    // ============================================================================

    public Animator anim;

    public Image img;
    public SpriteRenderer sr;

    void UpdateIcon()
    {
        // only show if slot is not null and not empty
        anim.gameObject.SetActive(slot!=null && !slot.IsEmpty());

        if(slot==null) return;

        if(anim.gameObject.activeSelf)
        {
            anim.runtimeAnimatorController = slot.item.iconAnimOV;
        }

        // sync ui icon with the animator that is animating the sprite renderer
        if(img.sprite != sr.sprite)
        {
            img.sprite = sr.sprite;
        }
    }

    // ============================================================================

    public TextMeshProUGUI countTMP;

    void UpdateCount()
    {
        // only show if slot is not null and more than 1 quantity
        countTMP.gameObject.SetActive(slot!=null && slot.quantity>1);

        if(slot==null) return;
        
        if(countTMP.gameObject.activeSelf)
        {
            countTMP.text = $"{slot.quantity}";
        }
    }
}
