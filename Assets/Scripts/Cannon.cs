using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPlace;

    public enum BulletType { Standard, Explosive }
    [Header("Shooting Settings")]
    [SerializeField] private BulletType bulletType;
    [SerializeField] private float reloadTime = 3f;

    [Header("Standard Bullet Settings")]
    [SerializeField] private float standardBulletMass = 1f;
    [SerializeField] private float standardBulletLifetime = 5f;
    [SerializeField] private float shootForce = 500f;

    [Header("Explosive Bullet Settings")]
    [SerializeField] private float explosiveMassEquivalent = 1f;
    [SerializeField] private GameObject explosionEffectPrefab;

    private float reloadTimer = 0f;
    private bool isReloading = false;

    void Update()
    {
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime)
            {
                isReloading = false;
                reloadTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isReloading)
            Shoot();

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchBulletType();
    }

    private void SwitchBulletType()
    {
        if (bulletType == BulletType.Standard)
            bulletType = BulletType.Explosive;
        else
            bulletType = BulletType.Standard;
    }

    private void Shoot()
    {
        isReloading = true;

        GameObject bullet = Instantiate(bulletPrefab, bulletPlace.position, bulletPlace.rotation);

        Superman bulletScript = bullet.GetComponent<Superman>();
        if (bulletScript != null)
            bulletScript.Shoot(bulletType, 
                               shootForce, 
                               standardBulletMass, 
                               standardBulletLifetime, 
                               explosiveMassEquivalent, 
                               bulletPlace.forward, 
                               explosionEffectPrefab);
    }
}