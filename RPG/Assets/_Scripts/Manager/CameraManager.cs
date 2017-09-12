using UnityEngine;
using System.Collections;

public class CameraManager : MonoSingleton<CameraManager>
{
    public Camera MainCamera;
    public Transform Target;

    public float Distance = 10.0f;
    public float Height = 5.0f;

    public float HeightDamping = 2.0f;
    public float WidthDamping = 3.0f;

    public void CameraInit(Actor Player)
    {
        this.MainCamera = Camera.main;
        this.Target = Player.SelfTransform;
    }

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        float wantedHeight = Target.position.y + Height;
        float currentHeight = this.MainCamera.transform.position.y;

        float wantedWidth = Target.position.x;
        float currentWidth = this.MainCamera.transform.position.x;

        currentHeight = Mathf.Lerp(
            currentHeight, wantedHeight, HeightDamping * Time.deltaTime);

        currentWidth = Mathf.Lerp(
            currentWidth, wantedWidth, WidthDamping * Time.deltaTime);

        Vector3 pos = Target.position;
        pos -= MainCamera.transform.forward * Distance;

        this.MainCamera.transform.position = new Vector3(currentWidth, currentHeight, pos.z);
        this.MainCamera.transform.LookAt(Target);
    }
}
