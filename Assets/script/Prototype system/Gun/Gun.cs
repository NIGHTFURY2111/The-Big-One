using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Material debugSelectedMaterial;
    public Material debugSeenMaterial;
    public Material debugNotMaterial;
    public GameObject debugPoint;


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
    private LayerMask EnemyMask;
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
    List<GameObject> magnetableEnemies = new();
    Transform cameraTransform;

    float playerVelMult;
    private float LastShootTime;

    PlayerStateMachine psm;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
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
            //if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit3, float.MaxValue, HitMask))
            //{
            //    initalDistance = Vector3.Distance(cameraTransform.position,hit3.point);
            //}


            Vector3 direction = (cameraTransform.forward * Speed + psm._getPCC._getvelocityVector).normalized;
            //Vector3 direction = (magnetableEnemies[0].transform.position - cameraTransform.position).normalized;
            //magnetableEnemies[0].TryGetComponent<ReturnEdges>(out ReturnEdges edgeScript);
            //Vector3 direction = (
            //    edgeScript.edgeCoordinate(transform) - cameraTransform.position).normalized;

            bool impactMade = (Physics.Raycast(cameraTransform.position, direction, out RaycastHit hit, float.MaxValue, HitMask))? true: false;
            bool bounceImpact = false;


                if (hit.IsUnityNull() && hit.collider.gameObject.layer == EnemyMask )
                {
                    Debug.Log("auihghaui");
                    bounceImpact = (Physics.Raycast(cameraTransform.position, direction, float.MaxValue, BounceMask)) ? true : false;
                }
                else 
                {
                    if (magnetableEnemies.Count > 0)
                    {
                        DetectNearestEnemy();

                        Vector3 vectorToEnemy = magnetableEnemies[0].transform.position - cameraTransform.position;
                        float dotEnemyToRayCentre = Vector3.Dot(vectorToEnemy, cameraTransform.forward);
                        Vector3 projectedPoint = (cameraTransform.position) + (dotEnemyToRayCentre * 0.99f* cameraTransform.forward);
                        Vector3 PointToShoot = magnetableEnemies[0].GetComponent<Collider>().ClosestPointOnBounds(projectedPoint);


                        debugPoint.transform.position = PointToShoot;
                        direction = (PointToShoot - cameraTransform.position).normalized;

                        impactMade = (Physics.Raycast(cameraTransform.position, direction, out hit, float.MaxValue, HitMask))? true: false;
                        bounceImpact = (Physics.Raycast(cameraTransform.position, direction, float.MaxValue, BounceMask)) ? true : false;
                        //StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, BounceDistance, impactMade, bounceImpact ));

                    }
                    else
                    {
                        hit.point = BulletSpawnPoint.position + direction * 100;
                        hit.normal = Vector3.zero;
                        impactMade = true;
                        bounceImpact = false;
                        //StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + direction * 100, Vector3.zero, BounceDistance, false));
                    }
    
                }
                debugPoint.transform.position = hit.point;
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, BounceDistance, impactMade, bounceImpact ));


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


    
    private void OnTriggerEnter(Collider other)
    {   // uses the cone collider and checks only on the "enemy layer"
     //if (other.CompareTag("Enemy") || other.)
     //   {
            magnetableEnemies.Add(other.gameObject);
            other.GetComponent<MeshRenderer>().material = debugSeenMaterial;
            Debug.Log(magnetableEnemies.Count);
        //}
        //Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
     //if (other.CompareTag("Enemy"))
     //   {
            magnetableEnemies.Remove(other.gameObject);
            other.GetComponent<MeshRenderer>().material = debugNotMaterial;
            Debug.Log(magnetableEnemies.Count);
        //}
    }


    //float Compensator(float initialDistance)
    //{
    //    return bulletCorrectionGraph.Evaluate(initialDistance/distanceDivider);
    //}

    void DetectNearestEnemy()
    {
        magnetableEnemies.Sort((enemy1, enemy2) =>
        {
            float angle1 = Vector3.Angle(
                cameraTransform.forward,
                (enemy1.transform.position - cameraTransform.position).normalized
            );
            float angle2 = Vector3.Angle(
                cameraTransform.forward,
                (enemy2.transform.position - cameraTransform.position).normalized
            );
            return angle1.CompareTo(angle2);
        });
    }

    private void Update()
    {
        string jjj = "";
        // sorting the list


        //if direct hit, then put that enemy at the top
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Enemy")))
        {
            // Remove and reinsert the hit transform if it exists in the list
            if (magnetableEnemies.Remove(hit.transform.gameObject))
            {
                magnetableEnemies.Insert(0, hit.transform.gameObject);
            }
        }
      
        #region coloring the enemies for debuggign
        foreach (GameObject enemy in magnetableEnemies)
        {
            if (enemy == magnetableEnemies[0])
            {
                magnetableEnemies[0].GetComponent<MeshRenderer>().material = debugSelectedMaterial;
            }
            else
            {
                enemy.GetComponent<MeshRenderer>().material = debugSeenMaterial;
            }
            jjj += enemy.name +"   ";

        }
            #endregion

        // get the edges, maths style (deprecated now ig)
        //magnetableEnemies[0].TryGetComponent<ReturnEdges>(out ReturnEdges edgeScript);

        //new way to get closest point on the edge of the selected enemy

        #region debugging sphere to the targeted position
        // Calculate the projected point
        #endregion


    }
}



//working:
//    store the list of enemies that are colliding
//    calculate their distance for the center (using the .forward of camera as centre)
//    shoot the enemy with the lowests angle
//    if not possible, shoot the closest enemy
//    priority list: * Hat
//                   * enemy closest by angle
//                   * enemy closest by distance(?)
//                   * straight forward
                  
// *  