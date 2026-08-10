using UnityEngine;

public class WindProjectile : BaseEnemyProjectile
{
    [SerializeField] private Vector3 initialDirection = Vector3.up;
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float phase1Duration = 0.5f;

    [Header("Phase 2: Secondary Movement")]
    [SerializeField] private Vector3 secondaryDirection = Vector3.up;
    [SerializeField] private float secondarySpeed = 12f;

    private float timer = 0f;

    [SerializeField] private float transitionDuration = 0.5f; 

    void Update()
    {
        // Track how long the projectile has been alive
        timer += Time.deltaTime;

        // Calculate how far along the transition we are (0.0 to 1.0)
        float t = (timer - phase1Duration) / transitionDuration;
        t = Mathf.Clamp01(t); // Keep t between 0 and 1

        // Smoothly blend the directions and speeds based on t
        Vector3 currentDirection = Vector3.Lerp(initialDirection, secondaryDirection, t);
        float currentSpeed = Mathf.Lerp(initialSpeed, secondarySpeed, t);

        // Move the projectile
        transform.Translate(currentDirection * currentSpeed * Time.deltaTime, Space.World);
    }
}
