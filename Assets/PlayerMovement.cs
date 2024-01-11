using UnityEngine;
using TouchControlsKit;

public class PlayerMovement : MonoBehaviour
{
    public float velocity = 5f;
    public float turnSpeed = 10f;

    private Vector2 input;
    private float angle;

    private Quaternion targetRotation;
    private Transform cam;

    public CharacterController controller;


    [SerializeField] private Animator animator;

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{

    //}

    private void Awake()
    {
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }

        cam = Camera.main.transform;

        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GetInput();
        Animate();

        if ((input.x) == 0 && (input.y) == 0) return;

        CalculateDirection();
        Rotate();
        Move();
    }

    private void GetInput()
    {
        var movement = TCKInput.GetAxis("Joystick");

        //if (movement.x > 0.5f)
        //{
        //    movement.x = 1;
        //}
        //else if (movement.x < -0.5f)
        //{
        //    movement.x = -1;
        //}
        //else
        //{
        //    movement.x = 0;
        //}

        //if (movement.y > 0.5f)
        //{
        //    movement.y = 1;
        //}
        //else if (movement.y < -0.5f)
        //{
        //    movement.y = -1;
        //}
        //else
        //{
        //    movement.y = 0;
        //}

        input.x = movement.x;
        input.y = movement.y;
    }

    private void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    private void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        controller.Move(Time.deltaTime * velocity * transform.forward);
    }

    private void Animate()
    {
        animator.SetFloat("Speed", input.magnitude);
    }
}
