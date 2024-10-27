using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlendTree : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    public MoveScript move;

    void FixedUpdate()
    {
        AnimBlendTree();
    }

    // ============================================================================

    public float baseSpeedMult = .88f;

    [Header("Axis")]
    public bool checkForward=true;
    public string forwardParamName="MoveZ";
    [Space]
    public bool checkRight=true;
    public string rightParamName="MoveX";
    [Space]
    public bool checkUp;
    public string upParamName="MoveY";

    [Header("For Sprites")]
    public bool changeBlendTreeSpeed=true;
    public string speedParamName="MoveSpeed";
    
    void AnimBlendTree()
    {
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
