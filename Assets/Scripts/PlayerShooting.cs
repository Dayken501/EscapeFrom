using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public float fireRate = 0.2f;
    public float shootRange = 20f;
    public int damage = 25;

    private Animator animator;
    private Camera mainCamera;
    private float lastFireTime;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            animator.SetBool("isShooting", true);

            if (Time.time - lastFireTime >= fireRate)
            {
                lastFireTime = Time.time;
                Shoot();
            }
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Hit enemy! Damage: " + damage);
            }
        }
    }
}