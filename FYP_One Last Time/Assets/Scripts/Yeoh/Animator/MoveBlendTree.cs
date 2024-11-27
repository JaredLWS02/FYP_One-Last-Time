using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlendTree : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    public MoveScript move;

    // ============================================================================
    
    [Header("Velocity")]
    public bool checkVelocity=true;
    public string velXParamName="VelocityX";
    public string velYParamName="VelocityY";
    public string velZParamName="VelocityZ";

    [Header("Ratio Offset")]
    public float baseSpeedMult = .9f;

    [Header("Axis")]
    public bool checkForward=true;
    public string forwardParamName="ForwardRatio";
    public bool checkRight=true;
    public string rightParamName="RightRatio";
    public bool checkUp;
    public string upParamName="UpRatio";

    [Header("For Sprites")]
    public bool changeBlendTreeSpeed=true;
    public string speedParamName="VelocityRatio";
    
    // ============================================================================
    
    void FixedUpdate()
    {
        if(checkVelocity)
        {
            anim.SetFloat(velXParamName, move.Round(rb.velocity.x, 2));
            anim.SetFloat(velYParamName, move.Round(rb.velocity.y, 2));
            anim.SetFloat(velZParamName, move.Round(rb.velocity.z, 2));
        }

        float velocity_ratio = move.velocity/(move.baseSpeed*baseSpeedMult + .001f); // .001f in case baseSpeed is zero

        Vector3 move_dir = rb.velocity.normalized;
        
        if(checkForward)
        {
            float dot_forward = Vector3.Dot(move.transform.forward, move_dir);

            anim.SetFloat(forwardParamName, dot_forward*velocity_ratio);
        }
        if(checkRight)
        {
            float dot_right = Vector3.Dot(move.transform.right, move_dir);

            anim.SetFloat(rightParamName, dot_right*velocity_ratio);
        }
        if(checkUp)
        {
            float dot_up = Vector3.Dot(move.transform.up, move_dir);

            anim.SetFloat(upParamName, dot_up*velocity_ratio);
        }

        if(changeBlendTreeSpeed)
        {
            anim.SetFloat(speedParamName, velocity_ratio);
        }
    }
}
