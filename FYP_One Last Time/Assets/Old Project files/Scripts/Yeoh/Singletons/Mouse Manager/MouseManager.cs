using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Current;

    void Awake()
    {
        if(!Current) Current=this;        
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Vector2 mousePos;

    [Header("Raycast")]
    public bool canClick=true;
    public LayerMask layers;
    public float clickRange=5;
    public bool ignoreTimescale;

    [Header("Leniency")]
    public float clickRadius=.01f;
    Vector2 startClickPos, endClickPos;
    float lastClickedTime;
    public float minSwipeDistance=2; // distance for a tap to be considered a swipe
    public float minSwipeTime=.25f; // time for a tap to be considered a swipe

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(canClick) CheckClick();
    }

    void CheckClick()
    {
        if(!ignoreTimescale && Time.timeScale==0) return;

        // Check if the current pointer event is over a UI element
        if(IsPointerOverUI(Input.mousePosition)) return;

        if(Input.GetMouseButtonDown(0))
        {
            startClickPos = mousePos; // Record the start position and time of the tap
            lastClickedTime = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            endClickPos = mousePos;

            float swipeDistance = Vector2.Distance(startClickPos, endClickPos); // Calculate the distance moved and time taken

            if(swipeDistance<minSwipeDistance && Time.time-lastClickedTime < minSwipeTime) // if not swipe
            {
                EventManager.Current.OnClick2D(mousePos);
                DoSphereCast();
            }
            else // if swipe
            {
                Vector2 swipeDirection = (endClickPos-startClickPos).normalized;
                EventManager.Current.OnSwipe2D(startClickPos, swipeDistance, swipeDirection, endClickPos);
            }
        }
    }

    List<RaycastResult> raycastResults = new List<RaycastResult>();

    bool IsPointerOverUI(Vector2 touchPos)
    {
        PointerEventData eventDataPos = new PointerEventData(EventSystem.current);

        eventDataPos.position = touchPos;

        EventSystem.current.RaycastAll(eventDataPos, raycastResults);

        if(raycastResults.Count>0) // if more than 0, then UI is touched
        {
            foreach(RaycastResult result in raycastResults)
            {
                if(result.gameObject.tag!="TouchField") return true; // ignore UI elements with this tag
            }
        }

        return false;
    }

    void DoSphereCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.SphereCast(ray, clickRadius, out RaycastHit hit, clickRange, layers, QueryTriggerInteraction.Collide))
        {
            Collider other = hit.collider;
            
            Rigidbody otherRb = other.attachedRigidbody;

            if(otherRb) EventManager.Current.OnClickObject(otherRb.gameObject);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void LockMouse(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.Locked : CursorLockMode.None;

        Cursor.visible = !toggle;
    }
}
