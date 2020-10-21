using System.Collections;
using UnityEngine;
/// <summary>
/// Handles the players's rotation on the basis of mouse input
/// </summary>

public class MouseLook : MonoBehaviour
{
    public float lookSensitivity = 2f;
    public float lookSmoothDamp = 0.5f;
    [HideInInspector]
    public float yRot, xRot;
    [HideInInspector]
    public float currentY, currentX;
    [HideInInspector]
    public float yRotationV, xRotationV;

    private void LateUpdate()
    {
        // todo: virtualize for alternative platforms
        yRot += Input.GetAxis("Mouse X") * lookSensitivity; // Reading the values from Mouse Axes
        xRot += Input.GetAxis("Mouse Y") * lookSensitivity;

        // SmoothDamp moves a float value from current to desired value over a period of time
        currentX = Mathf.SmoothDamp(currentX, xRot, ref xRotationV, lookSmoothDamp);
        currentY = Mathf.SmoothDamp(currentY, yRot, ref yRotationV, lookSmoothDamp);

        // Restricts the xrotation value to beless than 80 and more than -80, prevents backflip
        xRot = Mathf.Clamp(xRot, -80, 80);
        //  Setting the rotation of camera according to mouse input
        transform.rotation = Quaternion.Euler(-currentX, currentY, 0);
    }
}
