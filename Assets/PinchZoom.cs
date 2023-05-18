// DecompilerFi decompiler from Assembly-CSharp.dll class: PinchZoom
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public Camera nGuiCam;

    public float touchWhell;

    private void Update()
    {
        if (UnityEngine.Input.touchCount == 2)
        {
            Touch touch = UnityEngine.Input.GetTouch(0);
            Touch touch2 = UnityEngine.Input.GetTouch(1);
            if (isValidTouch(touch) && isValidTouch(touch2))
            {
                Vector2 a = touch.position - touch.deltaPosition;
                Vector2 b = touch2.position - touch2.deltaPosition;
                float magnitude = (a - b).magnitude;
                float magnitude2 = (touch.position - touch2.position).magnitude;
                float num = touchWhell = magnitude - magnitude2;
            }
        }
        else
        {
            touchWhell = 0f;
        }
    }

    private bool isValidTouch(Touch drag)
    {
        Ray ray = nGuiCam.ScreenPointToRay(drag.position);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit _, 100f))
        {
            return false;
        }
        return true;
    }
}
