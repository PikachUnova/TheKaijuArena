using UnityEngine;

public class HomingProjectile : BasePlayerProjectile
{
    public float rotationSpeed = 0.5f;
    private float homeTimer = 0.0f;

    // Homing onto enemies
    private Transform currentTarget;

    void Start()
    {
        base.Start();
        FindNearestTarget();
    }

    void FixedUpdate()
    {
        homeTimer += Time.deltaTime;

        if (currentTarget != null)
        {
            if (homeTimer > 1)
            {
                // Move towards the current target
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

                // Smoothly rotate towards the current target
                Vector3 direction = currentTarget.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            else // If no current target, move forward based on the current rotation
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else // No current target, move forward based on the current rotation
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
    }

    void FindNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= 200 && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = enemy.transform;
            }
        }
        currentTarget = nearestTarget;
    }
}
