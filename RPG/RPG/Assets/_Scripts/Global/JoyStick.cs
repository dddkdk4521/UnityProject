using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : BaseObject
{
    static JoyStick _instance = null;
    public static JoyStick Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public Camera UI_Camera;

    public bool NormalizedPower = false;
    public bool IsKeyboardInput = false;
    public bool IsPressed = false;

    private Vector2 _Axis;
    public Vector2 Axis
    {
        get
        {
            if(IsPressed)
            {
                return _Axis;
            }
            else
            {
                return Vector2.zero;
            }
        }
    }

    private Transform PointerTrans;

    private Vector3 CenterPosition;
    private Vector3 InnerPosition;

    private float Radius = 60.0f;
    private float InnerRadius = 10.0f;

    private void OnEnable()
    {
        UI_Camera = UICamera.mainCamera;

        if(UI_Camera == null)
        {
            Debug.LogError("UICamera를 찾지 못했습니다.");
            return;
        }

        // UI World 좌표 <-> Screen 좌표
        CenterPosition = UI_Camera.WorldToScreenPoint(this.transform.position);

        UIWidget widget = this.SelfComponent<UIWidget>();

        Radius = widget.width * 0.5f;

        PointerTrans = this.GetChild("JoyStick_Pointer");
        InnerRadius = PointerTrans.GetComponent<UIWidget>().width * 0.5f;
    }

    void OnPress(bool Pressed)
    {
        if(IsKeyboardInput)
        {
            return;
        }

        if(Pressed)
        {
            IsPressed = true;
            InnerPosition = UICamera.currentTouch.pos;
        }
        else
        {
            IsPressed = false;
            InnerPosition = CenterPosition;
        }

        Movement();
    }

    void OnDrag()
    {
        if (IsPressed)
        {
            InnerPosition = UICamera.currentTouch.pos;
            Movement();
        }   
    }
   
    void Movement()
    {
        Vector2 MovePosition = InnerPosition - CenterPosition;

        if(MovePosition.magnitude < Radius * 0.2f)
        {
            MovePosition = Vector2.zero;
        }
        else if(MovePosition.magnitude >= (Radius - InnerRadius))
        {
            MovePosition = MovePosition.normalized * (Radius - InnerRadius);
        }

        PointerTrans.localPosition = MovePosition;

        if(NormalizedPower)
        {
            MovePosition = MovePosition.normalized * Radius;
        }

        _Axis.x = MovePosition.x / Radius;
        _Axis.y = MovePosition.y / Radius;
    }

    private void Update()
    {
        Vector3 movePosition = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(movePosition != Vector3.zero)
        {
            // 키보드 입력
            IsKeyboardInput = true;
            IsPressed = true;

            InnerPosition = CenterPosition + movePosition * Radius;

            Movement();
        }
        else
        {
            if(IsKeyboardInput == true)
            {
                IsPressed = false;
                IsKeyboardInput = false;
            }
        }
    }
}