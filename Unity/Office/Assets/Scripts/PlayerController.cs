using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float WalkingSpeed = 7.5f;
    [SerializeField] float JumpSpeed = 8.0f;
    [SerializeField] float Gravity = 20.0f;
    [SerializeField] List<Camera> PlayerCameras;
    public static float LookSpeed = 2.0f;
    [SerializeField] float LookXLimit = 70.0f;

    CharacterController CharacterController;
    Vector3 MoveDirection = Vector3.zero;
    float RotationX = 0;

    public static bool CanMove {get {return !(Hud.Instance.MenuOpen || Hud.Instance.GameOverOpen);}}

    public static float CurrentPartTime {private set; get;}
    Vector3 StartPos;

    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        StartPos = transform.position;
    }

    void Update()
    {
        Cursor.lockState = CanMove ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !CanMove;

        if (CanMove)
        {
            CurrentPartTime += Time.deltaTime;
        }

        if (transform.position.y < -10)
        {
            CharacterController.enabled = false;
            transform.position = StartPos;
            CharacterController.enabled = true;
        }

        Movement();
        TryInteract();
    }

    void Movement()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = CanMove ? WalkingSpeed * SimpleInput.GetInputValue(eInput.YMoveAxis): 0;
        float curSpeedY = CanMove ? WalkingSpeed * SimpleInput.GetInputValue(eInput.XMoveAxis) : 0;
        float movementDirectionY = MoveDirection.y;
        MoveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (SimpleInput.GetInputActive(eInput.Jump) && CanMove && CharacterController.isGrounded)
        {
            MoveDirection.y = JumpSpeed;
        }
        else
        {
            MoveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!CharacterController.isGrounded)
        {
            MoveDirection.y -= Gravity * Time.deltaTime;
        }

        // Move the controller
        CharacterController.Move(MoveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (CanMove)
        {
            RotationX += -SimpleInput.GetInputValue(eInput.YLookAxis) * LookSpeed;
            RotationX = Mathf.Clamp(RotationX, -LookXLimit, LookXLimit);
            foreach (var camera in PlayerCameras)
            {
                camera.transform.localRotation = Quaternion.Euler(RotationX, 0, 0);
            }
            transform.rotation *= Quaternion.Euler(0, SimpleInput.GetInputValue(eInput.XLookAxis) * LookSpeed, 0);
        }
    }

    void TryInteract()
    {
        Egg egg = null;
        var lookTransform = PlayerCameras[0].transform;
        if (Physics.Raycast(lookTransform.position, lookTransform.forward, out RaycastHit hitInfo))
        {
            egg = hitInfo.collider.GetComponent<Egg>();
        }

        Hud.Instance.SetShowInteractAnimator(egg != null && !egg.IsFound);

        if (egg != null &&
            (Input.GetMouseButtonDown(0) ||
            SimpleInput.IsInputInState(eInput.Interact, eButtonState.Pressed)))
        {
            egg.Found();
        }
    }
}