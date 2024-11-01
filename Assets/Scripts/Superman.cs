using UnityEngine;

public class Superman : MonoBehaviour
{
    private Rigidbody _bulletRigidbody;
    private float _explosiveMassEquivalent;
    private float _standardBulletLifetime;
    private float _explosionRadius;
    private Vector3 _collisionPoint;
    private GameObject _explosionEffectPrefab;
    private Cannon.BulletType _bulletType;

    public void Shoot(Cannon.BulletType bulletType, float shootForce, float standardBulletMass, float standardBulletLifetime, float explosiveMassEquivalent, Vector3 shootDirection, GameObject explosionEffectPrefab)
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
        if (_bulletRigidbody == null) return;

        _bulletType = bulletType;
        _explosionEffectPrefab = explosionEffectPrefab;

        switch (bulletType)
        {
            case Cannon.BulletType.Standard:
                _bulletRigidbody.mass = standardBulletMass;
                _bulletRigidbody.AddForce(shootDirection * shootForce, ForceMode.Impulse);
                _standardBulletLifetime = standardBulletLifetime;
                Destroy(gameObject, standardBulletLifetime);
                break;

            case Cannon.BulletType.Explosive:
                _bulletRigidbody.mass = 1;
                _bulletRigidbody.AddForce(shootDirection * 50, ForceMode.Impulse);
                _explosiveMassEquivalent = explosiveMassEquivalent;
                _explosionRadius = Mathf.Pow(explosiveMassEquivalent * 4184f, 1f/3f);
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_bulletType == Cannon.BulletType.Explosive)
        {
            _collisionPoint = collision.contacts[0].point;
            Destroy(gameObject);
            Explode();
        }
    }

    private void Explode()
    {
        if (_explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(_explosionEffectPrefab, _collisionPoint, Quaternion.identity);
            ParticleSystem particleSystem = explosionEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var mainModule = particleSystem.main;
                mainModule.startSizeMultiplier *= Mathf.Pow(_explosiveMassEquivalent, 1f / 3f);
            }
            Destroy(explosionEffect, 5f);
        }

        // Энергия взрыва (в джоулях)
        float explosionEnergy = _explosiveMassEquivalent * 4184f;

        // Поиск всех объектов в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(_collisionPoint, _explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null && rb != _bulletRigidbody)
            {
                Vector3 direction = (rb.position - _collisionPoint).normalized;
                float distance = Mathf.Max(0.1f, Vector3.Distance(_collisionPoint, rb.position));

                // Проверка на препятствия между взрывом и объектом
                if (Physics.Raycast(_collisionPoint, direction, out RaycastHit hitInfo, distance))
                    if (hitInfo.collider != hit)
                        continue; // Есть препятствие, пропускаем объект

                // Расчёт силы взрыва
                float forceMagnitude = explosionEnergy / (4f * Mathf.PI * distance * distance);

                // Применение силы к объекту
                Vector3 force = direction * forceMagnitude + Vector3.up * 0.5f * forceMagnitude;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
        Debug.Log("Boom! Explosive power: " + _explosiveMassEquivalent);
    }
}