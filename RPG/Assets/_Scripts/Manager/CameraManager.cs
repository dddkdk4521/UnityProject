using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		MainCamera = Camera.main;
		Target = Player.SelfTransform;
	}

	private void LateUpdate()
	{
		if (Target == null)
			return;

		float wantedHeight = Target.position.y + Height;
		float currentHeight = MainCamera.transform.position.y;

		float wantedWidth = Target.position.x;
		float currentWidht = MainCamera.transform.position.x;

		currentHeight = Mathf.Lerp(
			currentHeight, wantedHeight, 
			HeightDamping * Time.deltaTime);

		currentWidht = Mathf.Lerp(
			currentWidht, wantedWidth,
			WidthDamping * Time.deltaTime);

		Vector3 pos = Target.position;
		pos -= MainCamera.transform.forward * Distance;

		MainCamera.transform.position
			= new Vector3(currentWidht, currentHeight, pos.z);

		MainCamera.transform.LookAt(Target);
	}

}
