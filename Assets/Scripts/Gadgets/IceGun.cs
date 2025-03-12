using UnityEngine;
using UnityEngine.Events;

public class IceGun : MonoBehaviour
{
    public UnityEvent onShoot;

    [SerializeField] 
    private float fireCooldown = 0.05f,
        launchVelocity = 700f;
    
    [SerializeField]
    private GameObject bulletPrefab;
    
    private float lastFire;


    void Start()
    {
        lastFire = fireCooldown;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && lastFire >= fireCooldown)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * launchVelocity);

            lastFire = 0;
        }

        if (lastFire < fireCooldown)
        {
            lastFire += Time.deltaTime;
        }
    }
}
