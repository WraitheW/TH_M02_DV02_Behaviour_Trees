using UnityEngine;

public class FollowPath : Seek
{
    public GameObject[] path;
    int currentPathIndex;
    float targetRadius = 0.5f;

    public override SteeringOutput getSteering()
    {

        if (target == null)
        {
            int nearestIndex = 0;
            float nearestTarget = float.PositiveInfinity;

            for (int i = 0; i < path.Length; i++)
            {
                GameObject potential = path[i];
                Vector3 vectorToPotential = potential.transform.position - character.transform.position;
                float potentialDistance = vectorToPotential.magnitude;
                if (potentialDistance < nearestTarget)
                {
                    nearestIndex = i;
                    nearestTarget = potentialDistance;
                }
            }

            target = path[nearestIndex];
        }

        float targetDistance = (target.transform.position - character.transform.position).magnitude;

        if (targetDistance < targetRadius)
        {
            currentPathIndex++;
            if (currentPathIndex > path.Length - 1)
            {
                currentPathIndex = 0;
            }
            target = path[currentPathIndex];
        }

        return base.getSteering();
    }
}