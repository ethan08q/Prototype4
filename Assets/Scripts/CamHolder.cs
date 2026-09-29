using UnityEngine;

public class CamHolder : MonoBehaviour
{
    public Transform cameraPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

