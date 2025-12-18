using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private ParticleSystem _exposionEffect;

    private Rigidbody _rb;
    private Vector3 _velocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _velocity = _rb.linearVelocity;
    }

    private void OnMouseUpAsButton()
    {
        StartCoroutine(Explode());
    }

    private void Update()
    {
        if (_velocity != _rb.linearVelocity)
        {
            StartCoroutine(Explode(2));
        }
    }

    private IEnumerator Explode(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        ApplyForceToObjectsAround();
        Instantiate(_exposionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void ApplyForceToObjectsAround()
    {
        List<Rigidbody> explodableObjects = GetExplodableObjects();
        foreach (Rigidbody explodableObject in explodableObjects)
        {
            explodableObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
        List<Rigidbody> objects = new List<Rigidbody>();
        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                objects.Add(hit.attachedRigidbody);
            }
        }

        return objects;
    }


}
