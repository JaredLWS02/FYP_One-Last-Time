using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParts : MonoBehaviour
{
    [SerializeField]
    Transform owner;
    
    [SerializeField]
    Transform head;
    public Transform GetHead() => head ? head : owner;
    
    [SerializeField]
    Transform body;
    public Transform GetBody() => body ? body : owner;

    [SerializeField]
    Transform hips;
    public Transform GetHips() => hips ? hips : owner;

    [SerializeField]
    Transform legs;
    public Transform GetLegs() => legs ? legs : owner;
    
}
