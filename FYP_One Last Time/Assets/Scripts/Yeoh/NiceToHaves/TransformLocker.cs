using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLocker : SlowUpdate
{
    [Header("TransformLocker")]
    public GameObject owner;

    // ============================================================================

    public struct Defaults
    {
        public Vector3 localPosition;
        public Vector3 localEulerAngles;
        public Vector3 localScale;
    };

    Defaults defaults;

    void Start()
    {
        Transform tr = owner.transform;

        defaults.localPosition = tr.localPosition;
        defaults.localEulerAngles = tr.localEulerAngles;
        defaults.localScale = tr.localScale;
    }

    // ============================================================================

    [System.Serializable]
    public struct LockAxis
    {
        public Vector3Int localPosition;
        public Vector3Int localEulerAngles;
        public Vector3Int localScale;
    };

    public LockAxis lockAxis;

    public override void OnSlowUpdate()
    {
        Transform tr = owner.transform;

        tr.localPosition = new Vector3
        (
            lockAxis.localPosition.x > 0 ? defaults.localPosition.x : tr.localPosition.x,
            lockAxis.localPosition.y > 0 ? defaults.localPosition.y : tr.localPosition.y,
            lockAxis.localPosition.z > 0 ? defaults.localPosition.z : tr.localPosition.z
        );

        tr.localEulerAngles = new Vector3
        (
            lockAxis.localEulerAngles.x > 0 ? defaults.localEulerAngles.x : tr.localEulerAngles.x,
            lockAxis.localEulerAngles.y > 0 ? defaults.localEulerAngles.y : tr.localEulerAngles.y,
            lockAxis.localEulerAngles.z > 0 ? defaults.localEulerAngles.z : tr.localEulerAngles.z
        );

        tr.localScale = new Vector3
        (
            lockAxis.localScale.x > 0 ? defaults.localScale.x : tr.localScale.x,
            lockAxis.localScale.y > 0 ? defaults.localScale.y : tr.localScale.y,
            lockAxis.localScale.z > 0 ? defaults.localScale.z : tr.localScale.z
        );
    }
}
