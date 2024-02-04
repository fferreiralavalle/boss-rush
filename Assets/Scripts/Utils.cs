using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static int GetPositionIndexClosestToPlayer(List<Transform> position)
    {
        Player player = GameMaster.Instance.Player;
        Transform pos = GetPositionClosestToTransform(position, player.transform);
        return position.IndexOf(pos);
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

    public static Transform GetPositionClosestToTransform(List<Transform> positions, Transform target)
    {
        Vector3 targetPos = target.position;
        Transform closestPos = positions[0];
        float closest = Vector3.Distance(closestPos.position, targetPos);
        for (int i = 1; i < positions.Count; i++)
        {
            float distance = Vector3.Distance(positions[i].position, targetPos);
            if (distance < closest)
            {
                closestPos = positions[i];
                closest = distance;
            }
        }
        return closestPos;

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
