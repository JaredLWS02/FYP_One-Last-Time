using UnityEngine;

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
            Debug.Log("Pan + Zoom triggered");

            CameraManager.Current.PanCameraOnContact(
                settings.panDistance,
                settings.panTime,
                settings.panDirection,
                false
            );

            if (settings.enableZoom)
            {
                CameraManager.Current.ZoomCameraOnContact(settings.zoomFOV, settings.zoomDuration);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        var other_rb = col.attachedRigidbody;
        if (!other_rb) return;

        if (other_rb.CompareTag("Player") && settings.panCameraOnContact)
        {
            Debug.Log("Pan + Zoom Reset");

            CameraManager.Current.PanCameraOnContact(
                settings.panDistance,
                settings.panTime,
                settings.panDirection,
                true
            );

            if (settings.enableZoom)
            {
                CameraManager.Current.TweenDefaultFOV(settings.zoomDuration);
            }
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
