using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuInput : MonoBehaviour
{
    private InputAction openMenu;
    [SerializeField] private GameObject menuPanel; // Reference to the menu panel GameObject
    [SerializeField] private Slider mouseSensibilitySlider; // Reference to the slider for mouse sensitivity (if needed)
    [SerializeField] private bool isMenuOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openMenu = InputSystem.actions.FindAction("UI/Menu"); //event for opening the menu
        openMenu.started += ToggleMenu; // Subscribe to the started event of the ToggleMenu action
        mouseSensibilitySlider.onValueChanged.AddListener(delegate { onValueChangedRuntime(mouseSensibilitySlider.value); }); // Add a listener to the slider's value changed event (if needed)
    }

    private void OnDisable()
    {
        openMenu.started -= ToggleMenu; // Unsubscribe from the started event when the object is disabled
        mouseSensibilitySlider.onValueChanged.RemoveListener(delegate { onValueChangedRuntime(mouseSensibilitySlider.value); }); // Remove all listeners from the slider's value changed event (if needed)
    }
    private void ToggleMenu(InputAction.CallbackContext context) // Method to handle toggling the menu on and off
    {
        Debug.Log("Menu opened pressing P!");
        isMenuOpen = !isMenuOpen;
        // Toggle the isMenuOpen boolean to track the menu state
        menuPanel.SetActive(isMenuOpen); // Set the menu panel active or inactive based on the isMenuOpen state
        if (isMenuOpen)
        {
            GetComponent<Player_Input>().enabled = false; // Disable the component that handles player input to prevent movement while the menu is open
            //OR
            InputSystem.actions.FindActionMap("Player").Disable(); // Disable the Player action map to prevent player input while the menu is open
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor to allow interaction with the menu
            Cursor.visible = true; // Make the cursor visible for menu interaction
        }
        else
        {
            GetComponent<Player_Input>().enabled = true; // Enable the component that handles player input to allow movement when the menu is closed
            //OR
            InputSystem.actions.FindActionMap("Player").Enable(); // Enable the Player action map to allow player input when the menu is closed
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back to the center of the screen for gameplay
            Cursor.visible = false; // Hide the cursor for gameplay
        }
    }
    private void onValueChangedRuntime(float value) // Method to handle changes in the mouse sensitivity slider (if needed)
    {
        Debug.Log($"MenuInput value changed - {value}");
    }

}

//Toggling the menu refers to the action of opening and closing a menu interface in a game.
//When the player presses a specific key (in this case, "P"), the menu will appear on the screen,
//allowing the player to interact with it. Pressing the key again would typically close the menu,
//returning the player to normal gameplay. In this code snippet,
//the ToggleMenu method is responsible for handling the logic of showing the menu and disabling
//player input when the menu is open.