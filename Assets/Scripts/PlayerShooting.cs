using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public float fireRate = 0.2f;
    public float shootRange = 50f;
    public int damage = 25;
    public AudioClip shootSound;

    private Animator animator;
    private Camera mainCamera;
    private AudioSource audioSource;
    private float lastFireTime;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
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
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit[] hits = Physics.RaycastAll(ray, shootRange);
        foreach (RaycastHit hit in hits)
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                break;
            }
        }
    }
}