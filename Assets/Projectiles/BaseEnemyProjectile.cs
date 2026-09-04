using UnityEngine;

public class BaseEnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 20;
    [SerializeField] protected int attackPower = 8;

    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected GameObject impact;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5.0f); // Exist until 5 seconds passed
    }

    void FixedUpdate()
    {
        // Move projectile
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Damage Player
        {
            other.GetComponent<PlayerHealth>().TakeDamage(attackPower);
            if (impact != null)
                Instantiate(impact, transform.position, transform.rotation);
            AudioManager.audioManager.PlaySFX(2, this.gameObject);
            Destroy(this.gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            if (impact != null)
                Instantiate(impact, transform.position, transform.rotation);
            AudioManager.audioManager.PlaySFX(2, this.gameObject);
            Destroy(this.gameObject);
        }
    }

}
