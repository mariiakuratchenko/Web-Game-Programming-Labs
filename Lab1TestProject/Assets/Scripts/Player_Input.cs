using UnityEngine;
using UnityEngine.InputSystem;
using KBCore.Refs; // Provides [Self], [Child], and ValidateRefs()

// Ensures a CharacterController component always exists on this GameObject
[RequireComponent(typeof(CharacterController))]
public class Player_Input : MonoBehaviour
{
    // Input actions (from the Input System)
    private InputAction move;   // Stores Move (WASD / left stick)
    private InputAction look;   // Stores Look (mouse / right stick)

    // Movement settings
    [SerializeField] private float maxSpeed = 10.0f;   // Player movement speed
    [SerializeField] private float gravity = -10.0f;   // Downward force applied every frame

    // Stores vertical movement (used for gravity)
    private Vector3 velocity;

    // Rotation / look settings
    [SerializeField] private float rotationSpeed = 40.0f; // Horizontal turn speed
    [SerializeField] private float mouseSensY = 5.0f;     // Vertical mouse sensitivity
    private float camXRotation;                     // Holds the final result of our moveement input for the camera's vertical rotation

    // References (auto-validated by KBCore.Refs)
    [SerializeField, Self] private CharacterController controller; // Player controller
    [SerializeField, Child] private Camera cam;                     // Player camera

    // Runs in the editor when values change
    // Automatically assigns references using KBCore
    private void OnValidate()
    {
        this.ValidateRefs();
    }

    // Called once when the object starts
    void Start()
    {
        // Get input actions from the global Input Actions asset
        // Requires "Default Input Actions" to be set in Project Settings
        move = InputSystem.actions.FindAction("Player/Move");
        look = InputSystem.actions.FindAction("Player/Look");

        // ---- Lock the mouse inside the game window ---- //
        //Option 1
        Cursor.lockState = CursorLockMode.Locked;
        //Option 2
        Cursor.visible = false;

        // Ensure CharacterController exists
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    // Called once per frame
    void Update()
    {
        // Read movement input (WASD / joystick)
        Vector2 readMove = move.ReadValue<Vector2>();

        // Read look input (mouse / right stick)
        Vector2 readLook = look.ReadValue<Vector2>();

        /* ---------------- PLAYER MOVEMENT ---------------- */

        // Convert 2D input into world-space movement
        Vector3 movement =
            transform.right * readMove.x +
            transform.forward * readMove.y;

        // Move the player horizontally
        controller.Move(movement * maxSpeed * Time.deltaTime);

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (gravity)
        controller.Move(velocity * Time.deltaTime);

        /* ---------------- PLAYER ROTATION ---------------- */

        // Rotate the player left/right (yaw)
        transform.Rotate(Vector3.up, readLook.x * rotationSpeed * Time.deltaTime);

        /* ---------------- CAMERA LOOK ---------------- */

        // Addition of mouse sensitivity and mouse input for vertical rotation (pitch)
        camXRotation += mouseSensY * readLook.y *Time.deltaTime * -1; //multiply by -1 to get rid of the invertion

        // Clamp vertical rotation so camera doesn't flip, can be less than 90 if you want to allow some flipping
        camXRotation = Mathf.Clamp(camXRotation, -90f, 90f);

        // Apply vertical rotation to the camera only( up and down rotation)
        cam.transform.localRotation = Quaternion.Euler(camXRotation, 0f, 0f);
    }

    public void ChangeMouseSencibility(float value)
    {
        Debug.Log($"Value changed - {value}");
    }
}
