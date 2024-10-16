using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAnim : MonoBehaviour
{
    public Vector3Int spinAxis = new(0, 1, 0);
    public bool localRotation=true;

    public float spinSpeed=200;
    public bool ignoreTime;

    public bool fixedUpdate=true;

    void Update()
    {
        if(!fixedUpdate) UpdateSpin();
    }
    void FixedUpdate()
    {
        if(fixedUpdate) UpdateSpin();
    }
    
    void UpdateSpin()
    {
        float speed = spinSpeed * (ignoreTime ? Time.unscaledDeltaTime : Time.deltaTime);

        Quaternion rotationChange = Quaternion.Euler(
            spinAxis.x > 0 ? speed : 0,
            spinAxis.y > 0 ? speed : 0,
            spinAxis.z > 0 ? speed : 0);

        if(localRotation)
        transform.localRotation *= rotationChange;
        
        else
        transform.rotation *= rotationChange;
    }
}
