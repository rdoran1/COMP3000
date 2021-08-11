using UnityEngine;
using TMPro;

public class GunBehaviour : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    private LineRenderer laserLine;

    //graphics
    public TextMeshProUGUI text;

    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
        text.SetText(bulletsLeft + " / " + magazineSize);

    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;

            laserLine.enabled = true;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        Vector3 direction = fpsCam.transform.forward;
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        laserLine.SetPosition(0, attackPoint.position);

        if (Physics.Raycast(rayOrigin, direction, out hit, range))
        {
            Debug.Log(hit.collider.name);
            laserLine.SetPosition(1, hit.point);
            //if (hit.collider.CompareTag("Enemy"))
            //    hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);
            EnemyScript health = hit.collider.GetComponent<EnemyScript>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (direction * range));
        }

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        laserLine.enabled = false;
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
