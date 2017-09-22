using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoard : MonoBehaviour 
{
	BaseObject Target = null;

	Camera UICam = null;
	Camera WorldCam = null;

	Transform BoardTransform = null;

	[SerializeField]
	bool AttackBoard = true;
	Vector3 Position = Vector3.zero;

	[SerializeField]
	protected float DestroyTime = 0.0f;
	protected float CurTime = 0.0f;

	public virtual eBoardType BOARD_TYPE
	{
		get { return eBoardType.BOARD_NONE; }
	}

	public BaseObject TargetComponent
	{
		set
		{
			Target = value;
			BoardTransform = Target.GetChild("Board");

			if(BoardTransform == null)
			{
				BoardTransform = Target.SelfTransform;
			}
		}
	}

	public Camera UICAM
	{
		get
		{
			if(UICam == null)
			{
				UICam = UICamera.mainCamera;
			}
			return UICam;
		}
	}

	public Camera WORLDCAM
	{
		get
		{
			if(WorldCam == null)
			{
				WorldCam = Camera.main;
			}
			return WorldCam;
		}
	}

	public virtual void SetData(string strKey, params object[] datas)
	{

	}

	public virtual void UpdateBoard()
	{
		// Time.deltaTime -> Update();
		// Time.fixedDelataTime -> FixedUpdate();

		CurTime += Time.deltaTime;

		if(UICAM == null || WORLDCAM == null)
		{
			Debug.LogError("Camera를 찾지 못했습니다.");
			return;
		}

		if (BoardTransform == null)
			return;

		if(AttackBoard == true)
		{
			Position = BoardTransform.position;
		}
		else
		{
			if(Position == Vector3.zero)
				Position = BoardTransform.position;
		}

		Vector3 viewPort = WORLDCAM.WorldToViewportPoint(Position);
		Vector3 boardPosition = UICAM.ViewportToWorldPoint(viewPort);

		boardPosition.z = 0f;
		this.transform.position = boardPosition;
	}

	public bool CheckDestroyTime()
	{
		if (DestroyTime == 0.0f)
			return false;

		if (DestroyTime < CurTime)
			return true;

		return false;
	}



}
