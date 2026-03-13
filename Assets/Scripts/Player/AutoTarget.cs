using UnityEngine;

public class AutoTarget : MonoBehaviour
{
    public float range = 100f;

    public Enemy GetNearestEnemy()
    {
        Enemy nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Enemy enemy in Enemy.allEnemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            if (dist < minDist && dist <= range)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }
}