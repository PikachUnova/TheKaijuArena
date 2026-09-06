using UnityEngine;

public class PlayerProjectile : BasePlayerProjectile
{
    protected override void  Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        // Move projectile
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
