using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private float projectileForce = 0f;
    private InputAction attack;

    private void Awake()
    {
        attack = InputSystem.actions.FindAction("Player/Attack");
    }

    private void OnEnable()
    {
        attack.started += Shoot;
    }

    private void OnDisable()
    {
        attack.started -= Shoot;
    }
    private void Shoot(InputAction.CallbackContext context)
    {
        GameObject projectile = GameObject.Instantiate(bullet, projectileSpawn.position, projectileSpawn.rotation); 
        // Create a new projectile at the spawn point
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * projectileForce, ForceMode.Impulse); 
        // Apply force to the projectile to propel it forward
        Destroy(projectile, 1.5f); // Destroy the projectile after 1.5 seconds to clean up
    }
}
