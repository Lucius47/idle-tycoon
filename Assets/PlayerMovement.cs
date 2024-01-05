using UnityEngine;
using TouchControlsKit;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private Vector3 previousPosition;

    private void Update()
    {
        Vector2 movement = TCKInput.GetAxis("Joystick");
        

        // normalize the movement vector so that diagonal movement isn't faster
        movement.Normalize();

        // move the player in different directions based on camera look direction
        transform.Translate(movement.x * speed * Time.deltaTime, 0, movement.y * speed * Time.deltaTime, Camera.main.transform);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        
        if (movement != Vector2.zero)
        {
            // rotate the player to face the direction of movement
            var dir = transform.position - previousPosition;
            transform.rotation = Quaternion.LookRotation(dir);

            // play the walking animation
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }



        previousPosition = transform.position;
    }
}
