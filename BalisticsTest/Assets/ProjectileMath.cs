using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMath : MonoBehaviour
{

    //given a projectile start position and speed, a target start position and speed,
    //calculate an approximate aim direction for the projectile to hit the target, as well as a time of impact.
    public static bool CalculateProjectileAim(Vector3 projectileStartPos, float projectileSpeed, Vector3 targetStartPos, Vector3 targetVelocity, int iterations, out float timeToTarget, out Vector3 aimDirection)
    {

        Vector3 toTarget = targetStartPos - projectileStartPos;


        aimDirection = (targetStartPos - projectileStartPos).normalized;
        timeToTarget = (targetStartPos - projectileStartPos).magnitude / projectileSpeed;
        Vector3 predictedPos = Vector3.zero;

        for (int i = 0; i < iterations; i++)
        {
            timeToTarget = toTarget.magnitude / projectileSpeed;

            //predicted time after n seconds, where n is the amount of time it would take to hit the player where they're at now.
            predictedPos = targetStartPos + targetVelocity * timeToTarget;

            //repeat equation but with new time calculation based on adjusted aim.
            toTarget = (predictedPos - projectileStartPos);
        }

        aimDirection = (predictedPos - projectileStartPos).normalized;
        return true;
    }
}
