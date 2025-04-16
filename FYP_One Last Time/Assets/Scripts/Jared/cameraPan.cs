using UnityEngine;
using Cinemachine;

public class cameraPan : MonoBehaviour
{
    public CameraPanSettings settings;

    private Collider _coll;

    private void Start()
    {
        _coll = GetComponent<Collider>();
        if (settings == null)
        {
            settings = Resources.Load<CameraPanSettings>("PanUp");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        var other_rb = col.attachedRigidbody;
        if (!other_rb) return;

        if (other_rb.CompareTag("Player") && settings.panCameraOnContact)
        {
            Debug.Log("Pan triggered");
            CameraManager.Current.PanCameraOnContact(
                settings.panDistance,
                settings.panTime,
                settings.panDirection,
                false
            );
        }
    }

    private void OnTriggerExit(Collider col)
    {
        var other_rb = col.attachedRigidbody;
        if (!other_rb) return;

        if (other_rb.CompareTag("Player") && settings.panCameraOnContact)
        {
            CameraManager.Current.PanCameraOnContact(
                settings.panDistance,
                settings.panTime,
                settings.panDirection,
                true
            );
        }
    }

    public enum PanDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
