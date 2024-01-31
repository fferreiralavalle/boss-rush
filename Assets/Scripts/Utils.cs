using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static int GetPositionIndexClosestToPlayer(List<Transform> position)
    {
        Player player = GameMaster.Instance.Player;
        Vector3 playerPos = player.transform.position;
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(position[closestIndex].position, playerPos);
        for (int i = 1; i < position.Count; i++)
        {
            float distance = Vector3.Distance(position[i].position, playerPos);
            if (distance < closestDistance)
            {
                closestIndex = i;
                closestDistance = distance;
            }
        }
        return closestIndex;
    }

    public static int GetPositionIndexFarthestToPlayer(List<Transform> position)
    {
        Player player = GameMaster.Instance.Player;
        Vector3 playerPos = player.transform.position;
        int farthestIndex = 0;
        float farthest = Vector3.Distance(position[farthestIndex].position, playerPos);
        for (int i = 1; i < position.Count; i++)
        {
            float distance = Vector3.Distance(position[i].position, playerPos);
            if (distance > farthest)
            {
                farthestIndex = i;
                farthest = distance;
            }
        }
        return farthestIndex;
    }

    public static void DeleteAllProjectiles()
    {
        Projectile[] projectiles = Object.FindObjectsOfType<Projectile>();
        foreach(Projectile projectile in projectiles)
        {
            projectile.HandleRemove();
        }
    }
}
