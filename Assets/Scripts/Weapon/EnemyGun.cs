using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform shootingPoint, shootingOrigin;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitWallImpact;

    public float shootingSpread = 0, minimumShootingDelay = .15f, randomShootingDelay = .2f, maxShootingDistance = 100;
    private float nextTimeToFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Transform target)
    {
        if (Time.time < nextTimeToFire)
            return;

        nextTimeToFire = Time.time + minimumShootingDelay + Random.Range(0, randomShootingDelay);

        float randomX = Random.Range(-shootingSpread, shootingSpread);
        float randomY = Random.Range(-shootingSpread, shootingSpread);
        float randomZ = Random.Range(-shootingSpread, shootingSpread);

        Vector3 fireDirection = target.position - shootingOrigin.position;
        fireDirection = Quaternion.LookRotation(fireDirection).eulerAngles;
        fireDirection.x += randomX;
        fireDirection.y = transform.eulerAngles.y + randomY;
        fireDirection.z = transform.eulerAngles.z + randomZ;
        shootingOrigin.eulerAngles = fireDirection;

        animator.SetTrigger("shoot");
        muzzleFlash.Play();

        if (Physics.Raycast(shootingOrigin.position, shootingOrigin.forward, out RaycastHit hit, maxShootingDistance))
        {
            //print(hit.transform.name);
            GameObject tempHitImpact = Instantiate(hitWallImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(tempHitImpact, 3);
        }

        Vector3 rayDirection = (shootingOrigin.forward) * Vector3.Distance(target.position, shootingOrigin.position);
        Debug.DrawRay(shootingOrigin.position, rayDirection);
    }
}
