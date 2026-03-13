using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public AutoTarget targetFinder;
    public RaycastWeapon rayWeapon;

    void Update()
    {
        if (targetFinder == null || rayWeapon == null) return;

        Enemy enemy = targetFinder.GetNearestEnemy();

        if (enemy != null)
        {
            Vector3 targetPos = enemy.transform.position;
            Collider col = enemy.GetComponent<Collider>();
            if (col != null)
                targetPos = col.bounds.center;

            Vector3 dir = (targetPos - rayWeapon.raycastOrigin.position).normalized;
            rayWeapon.transform.rotation = Quaternion.LookRotation(dir);

            if (!rayWeapon.isFiring)
                rayWeapon.StartFiring();

            rayWeapon.UpdateWeapon(Time.deltaTime, targetPos);
        }
        else
        {
            if (rayWeapon.isFiring)
                rayWeapon.StopFiring();
        }
    }
}