using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;

    public Bullet bulletPrefab;
    public Transform shootPosition;

    public float attackRate;
    private float _timeSinceLastShoot;
    private void Update()
    {

        _timeSinceLastShoot += Time.deltaTime;
        if (Input.GetMouseButton(0) && _timeSinceLastShoot > attackRate)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = shootPosition.position;


        newBullet.transform.LookAt(shootPosition.position + shootPosition.forward);
        newBullet.StartBullet(this);
        _timeSinceLastShoot = 0;


    }
}


public enum WeaponType
{
    MachinegGun,
    Shotgun,
}