using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour {
    //Gun stats
    public int damage;
    public float shootingCooldown, spread, range, reloadTime, shotCooldown, secShotCooldown;
    public int magSize, bulletsPerTap;
    public bool auto;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading, secFire;

    //Reference
    public Camera fpsCam;
    public GameObject parent;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public RaycastHit secRayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    public LayerMask whatIsGrappleable;
    public float grappleRange;

    public RaycastHit predHit;
    public float predSphereCastR;
    public Transform predPoint;

    void Start() {
        bulletsLeft = magSize;
        readyToShoot = true;
        predPoint.gameObject.SetActive(false);
    }
    private void Update() {
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magSize);

        if (secFire) {
            parent.GetComponent<PlayerController>().StartSwing(secRayHit);
            secFire = false;
        }

        //Debug.Log(secFire);
    }

    private void MyInput() {
        if (auto)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        /*
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
            Reload();
        */
        /*
        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        */
        //Secondary Fire
        if (Input.GetKeyDown(KeyCode.Mouse1) && !secFire) {
            secShoot();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1)) {
            secFire = false;
            parent.GetComponent<PlayerController>().StopSwing();
        }
    }
    private void Shoot() {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy)) {
            //Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<EnemyController>().TakeDamage(damage);
        }

        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.FromToRotation(Vector3.forward, rayHit.normal));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //Debug.Log(attackPoint.position.x + " " + attackPoint.position.y + " " + attackPoint.position.z);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", shootingCooldown);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", shotCooldown);
    }

    private void secShoot() {
        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward;

        //RayCast
        /*
        if (Physics.Raycast(fpsCam.transform.position, direction, out secRayHit, grappleRange, whatIsGrappleable)) {
            //Debug.Log(rayHit.collider.name);

            secFire = true;
        }
        */

        if (predHit.point == Vector3.zero)
            return;
        secRayHit = predHit;

        //Graphics
        Instantiate(bulletHoleGraphic, secRayHit.point, Quaternion.FromToRotation(Vector3.forward, secRayHit.normal));
        //Debug.Log(attackPoint.position.x + " " + attackPoint.position.y + " " + attackPoint.position.z);

        Invoke("ResetSecShot", shootingCooldown);
    }

    public void CheckSwingPoints() {
        RaycastHit sphereCastHit;
        if (Physics.SphereCast(fpsCam.transform.position, predSphereCastR, fpsCam.transform.forward, out sphereCastHit, grappleRange, whatIsGrappleable) && Input.GetKeyDown(KeyCode.Mouse1) && !secFire)
            secFire = true;

        RaycastHit raycastHit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out raycastHit, grappleRange, whatIsGrappleable) && Input.GetKeyDown(KeyCode.Mouse1) && !secFire)
            secFire = true;

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;
        else
            realHitPoint = Vector3.zero;

        if (realHitPoint != Vector3.zero) {
            //predPoint.gameObject.SetActive(true);
            predPoint.position = realHitPoint;
        }
        else {
            //predPoint.gameObject.SetActive(false);
        }
        predHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }

    private void ResetShot() {
        readyToShoot = true;
    }

    private void ResetSecShot() {

    }

    private void Reload() {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished() {
        bulletsLeft = magSize;
        reloading = false;
    }
}
