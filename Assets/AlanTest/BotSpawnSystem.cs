using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEditor.PlayerSettings;

// System for moving the player based on keyboard input
[UpdateAfter(typeof(GatherInputSystem))]
public partial class BaseSpawnSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Grab our DeltaTime out of the system so it is usable by the ForEach lambda
        var deltaTime = Time.DeltaTime;
        //var randVal = UnityEngine.Random.Range(700, 1100);

        Entities
            .WithName("MovePlayer")// ForEach name is helpful for debugging
            .ForEach((
                ref Translation translation, // "ref" keyword makes this parameter ReadWrite
                ref Rotation rotation,
                ref PositionData pos, 
                ref RotationData rot,
                in TargetPositionData tarPos,
                in MoveSpeedData moveSpeed
                ) =>
            {
                //translation.Value = pos.Value;
                //rotation.Value = rot.Value;
                //var tarPos = new float3(randVal, pos.Value.y, randVal);
                var vectorToTarget = tarPos.Value - pos.Value;
                if (math.lengthsq(vectorToTarget) > 1f)
                {
                    // Normalize the vector to our target - this will be our movement direction
                    var moveDirection = math.normalize(vectorToTarget);

                    // Rotate the guard toward the target
                    // Since the camera looks at the scene top-down, we do not rotate in the y direction
                    rotation.Value = quaternion.LookRotation(new float3(moveDirection.x, 0.0f, moveDirection.z), math.up());

                    // Move the guard forward toward the target
                    translation.Value = pos.Value + moveDirection * moveSpeed.Value * deltaTime;

                    pos.Value = translation.Value;
                    rot.Value = rotation.Value;
                }

            }).ScheduleParallel(); // Schedule the ForEach with the job system to run
    }
}
