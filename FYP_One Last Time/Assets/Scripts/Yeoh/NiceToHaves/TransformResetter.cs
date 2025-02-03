using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformResetter : SlowUpdate
{
    [Header("TransformResetter")]
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
    public struct ResetAxis
    {
        public Vector3Int localPosition;
        public Vector3Int localEulerAngles;
        public Vector3Int localScale;
    };

    public ResetAxis resetAxis;

    public override void OnSlowUpdate()
    {
        Transform tr = owner.transform;

        tr.localPosition = new Vector3
        (
            resetAxis.localPosition.x > 0 ? defaults.localPosition.x : tr.localPosition.x,
            resetAxis.localPosition.y > 0 ? defaults.localPosition.y : tr.localPosition.y,
            resetAxis.localPosition.z > 0 ? defaults.localPosition.z : tr.localPosition.z
        );

        tr.localEulerAngles = new Vector3
        (
            resetAxis.localEulerAngles.x > 0 ? defaults.localEulerAngles.x : tr.localEulerAngles.x,
            resetAxis.localEulerAngles.y > 0 ? defaults.localEulerAngles.y : tr.localEulerAngles.y,
            resetAxis.localEulerAngles.z > 0 ? defaults.localEulerAngles.z : tr.localEulerAngles.z
        );

        tr.localScale = new Vector3
        (
            resetAxis.localScale.x > 0 ? defaults.localScale.x : tr.localScale.x,
            resetAxis.localScale.y > 0 ? defaults.localScale.y : tr.localScale.y,
            resetAxis.localScale.z > 0 ? defaults.localScale.z : tr.localScale.z
        );
    }
}
