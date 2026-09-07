using UnityEngine;

public class EnemyIceProjectile : BaseEnemyProjectile
{
    [SerializeField] private float freezeTime = 3f;
    public GameObject iceFractures;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Damage Player
        {
            other.GetComponent<PlayerHealth>().TakeDamage(attackPower);
            //other.GetComponent<PlayerMovement>().Freeze(freezeTime);
            if (impact != null)
                Instantiate(impact, transform.position, transform.rotation);
            AudioManager.audioManager.PlaySFX(2);
            IceParticles();
            Destroy(this.gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            if (impact != null)
                Instantiate(impact, transform.position, transform.rotation);
            AudioManager.audioManager.PlaySFX(2);
            IceParticles();
            Destroy(this.gameObject);
        }
    }

    private void IceParticles()
    {
        if (iceFractures != null)
        {
            GameObject fractObj = Instantiate(iceFractures, transform.position, transform.rotation) as GameObject;

            foreach (Transform child in fractObj.transform)
            {
                var rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    child.parent = null;

                    // Add random force for scattering
                    Vector3 randomDirection = Random.insideUnitSphere; // Random direction
                    float randomForce = Random.Range(5f, 8f); // Random force magnitude
                    rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);

                    // Add random torque for rotation
                    Vector3 randomTorque = Random.insideUnitSphere * Random.Range(5f, 10f);
                    rb.AddTorque(randomTorque, ForceMode.Impulse);
                    Destroy(child.gameObject, 1.5f);
                }
            }
            Destroy(fractObj, 5f);
        }
    }
}
