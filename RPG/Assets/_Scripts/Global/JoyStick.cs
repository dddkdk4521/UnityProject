using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : BaseObject
{
    static JoyStick _instance = null;
    public static JoyStick Instance
    {
        get { return _instance; }
    }

    public Camera UI_Camera;
    public bool NormalizedPower = false;
    public bool IsKeyboardInput = false;
    public bool IsPressed = false;

    private Vector2 _Axis;
    public Vector2 Axis
    {
        get { return IsPressed ? _Axis : Vector2.zero; }
    }

    public Transform PointerTrans;

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
        }

        CenterPosition = UI_Camera.WorldToScreenPoint(this.transform.position);

        UIWidget widget = this.SelfComponent<UIWidget>();
        Radius = widget.width * 0.5f;

        PointerTrans = this.GetChild("JoyStick_Pointer");
        InnerRadius = PointerTrans.GetComponent<UIWidget>().width * 0.5f;
    }

    // NGUI
    void OnPress(bool Pressed)
    {
        if (IsKeyboardInput)
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

    // NGUI
    void OnDrag()
    {
        if(IsPressed)
        {
            InnerPosition = UICamera.currentTouch.pos;
            Movement();
        }
    }

    void Movement()
    {
        Vector2 MovePosition = InnerPosition - CenterPosition;

        if (MovePosition.magnitude < Radius * 0.2f)
        {
            MovePosition = Vector2.zero;
        }
        else if (MovePosition.magnitude >= (Radius - InnerRadius))
        {
            float temp = Radius - InnerRadius;
            MovePosition = MovePosition.normalized * (temp);
        }

        PointerTrans.localPosition = MovePosition;
        //PointerTrans.localPosition = new Vector2(15f, 15f);

        if (NormalizedPower)
        {
            MovePosition = MovePosition.normalized * Radius;
        }

        _Axis.x = MovePosition.x / Radius;
        _Axis.y = MovePosition.y / Radius;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        Vector3 movePosition = 
            new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (movePosition != Vector3.zero)
        {
            this.IsKeyboardInput = true;
            this.IsPressed = true;
            this.InnerPosition = CenterPosition + movePosition * Radius;
            Movement();
        }
        else
        {
            if (IsKeyboardInput == true)
            {
                this.IsPressed = false;
                this.IsKeyboardInput = false;
            }                
        }
    }
}
