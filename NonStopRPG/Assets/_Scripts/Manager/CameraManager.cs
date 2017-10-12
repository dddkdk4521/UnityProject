using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
	public GameObject CameraRoot;
	public Transform Target;

    public Camera MainCamera;

	public float Distance = 10.0f;
	public float Height = 5.0f;

	public float HeightDamping = 2.0f;
	public float WidthDamping = 3.0f;

	public void CameraInit(Actor Player)
	{
        MainCamera = Camera.main;

        CameraRoot = GameObject.Find("CameraRoot");
		Target = Player.SelfTransform;
	}

	private void LateUpdate()
	{
		if (Target == null)
			return;

		float wantedHeight = Target.position.y + Height;
		float wantedWidth = Target.position.x;

        float currentHeight = CameraRoot.transform.position.y;
		float currentWidth = CameraRoot.transform.position.x;

		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, HeightDamping * Time.deltaTime);

		currentWidth = Mathf.Lerp(currentWidth, wantedWidth, WidthDamping * Time.deltaTime);

		Vector3 pos = Target.position;
		pos -= CameraRoot.transform.forward * Distance;

		CameraRoot.transform.position = new Vector3(currentWidth, currentHeight, pos.z);

		CameraRoot.transform.LookAt(Target);
	}

    public void Shake()
    {
        StartCoroutine(CameraShakeProcess(0.8f, 0.2f));
    }
    
    IEnumerator CameraShakeProcess(float shakeTime, float shakeSense)
    {
        float deltaTime = 0.0f;

        while (deltaTime < shakeTime)
        {
            deltaTime += Time.deltaTime;

            Vector3 pos = Vector3.zero;
            {
                pos.x = Random.Range(-shakeSense, shakeSense);
                pos.y = Random.Range(-shakeSense, shakeSense);
                pos.z = Random.Range(-shakeSense, shakeSense);
            }

            MainCamera.transform.localPosition = pos;

            yield return new WaitForEndOfFrame();
        }
    }
}
