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

        // Стрельба от персонажа вперёд куда смотрит оружие
        Vector3 shootOrigin = transform.position + Vector3.up * 1.2f;
        Vector3 shootDirection = transform.forward;

        if (Physics.Raycast(shootOrigin, shootDirection, out RaycastHit hit, shootRange))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}