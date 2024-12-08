using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.1f;
    [SerializeField]
    private float Speed = 100;
    [SerializeField]
    private LayerMask HitMask;
    [SerializeField]
    LayerMask BounceMask;
    [SerializeField]
    private bool BouncingBullets;
    [SerializeField]
    private float BounceDistance = 10f;
    [SerializeField]
    GameObject Enemies;
    [SerializeField]
    float maxBulletBounceDist = 1000f;
    //[SerializeField]
    //AnimationCurve bulletCorrectionGraph;
    //[SerializeField]
    //float distanceDivider = 1f;

    GameObject[] enemies;

    float playerVelMult;
    private float LastShootTime;

    PlayerStateMachine psm;

    private void Awake()
    {
        psm = GetComponent<PlayerStateMachine>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void Shoot()
    {

        if (LastShootTime + ShootDelay < Time.time)
        {
            // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
            // for more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

            ShootingSystem.Play();

            //Vector3 direction = BulletSpawnPoint.forward;
            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

            //float initalDistance = 10000f;
            //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit3, float.MaxValue, HitMask))
            //{
            //    initalDistance = Vector3.Distance(Camera.main.transform.position,hit3.point);
            //}


            Vector3 direction = (Camera.main.transform.forward * Speed + psm._getPCC._getvelocityVector).normalized;

            if (Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit hit, float.MaxValue, HitMask))
            {
                if(Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit hit1, float.MaxValue, BounceMask))
                {
                    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, BounceDistance, true, true));
                }
                else
                {
                    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, BounceDistance, true, false));

                }

            }
            else
            {
                StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + direction * 100, Vector3.zero, BounceDistance, false, false));
            }

            LastShootTime = Time.time;
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, float BounceDistance, bool MadeImpact, bool BounceImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        Vector3 direction = (HitPoint - Trail.transform.position).normalized;

        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * Speed;

            yield return null;
        }

        Trail.transform.position = HitPoint;

        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));

            if (BouncingBullets && BounceDistance > 0 && BounceImpact)
            {
                Vector3 bounceDirection = (findEnemy(HitPoint) - HitPoint).normalized;

                if (Physics.Raycast(HitPoint, bounceDirection, out RaycastHit hit, BounceDistance, HitMask))
                {
                    if(Physics.Raycast(HitPoint, bounceDirection, out RaycastHit hit1, BounceDistance, BounceMask))
                    {


                       yield return StartCoroutine(SpawnTrail(
                       Trail,
                       hit1.point,
                       hit.normal,
                       BounceDistance - Vector3.Distance(hit.point, HitPoint),
                       true,
                       true
                       ));
                    }
                    else
                    {
                      yield return StartCoroutine(SpawnTrail(
                      Trail,
                      hit.point,
                      hit.normal,
                      BounceDistance - Vector3.Distance(hit.point, HitPoint),
                      true,
                      false
                      ));
                    }
                   
                }
                else
                {
                    yield return StartCoroutine(SpawnTrail(
                        Trail,
                        HitPoint + bounceDirection * BounceDistance,
                        Vector3.zero,
                        0,
                        false,
                        false
                    ));
                }
            }
        }

        Destroy(Trail.gameObject, Trail.time);
    }


    Vector3 findEnemy(Vector3 hitPoint)
    {
       
        GameObject trackedEnemy = null;
        float minDist = maxBulletBounceDist;

        foreach (GameObject enemy in enemies) { 
            float Dist = Vector3.Distance(hitPoint, enemy.transform.position);
            if (Dist < minDist) { 
                minDist = Dist;
                trackedEnemy = enemy;
            }
        }
        if (trackedEnemy != null)
        {
            return trackedEnemy.transform.position;
        }
        else 
        { 
            return Vector3.zero;
        }
    }

    //float Compensator(float initialDistance)
    //{
    //    return bulletCorrectionGraph.Evaluate(initialDistance/distanceDivider);
    //}
}