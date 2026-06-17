using Joive.BurglinGnomes.SwimmingRing.Config;
using Joive.BurglinGnomes.SwimmingRing.Game;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Gameplay
{
    internal static class SwimmingRingMovement
    {
        private const int WaterSurfaceDetectionMask = 1;
        private const int WaterMask = 16;
        private const float InputRiseThreshold = 0.35f;
        private const float IdleInputSqrThreshold = 0.01f;
        private const float SwimAnimationMinSpeed = 0.1f;
        private static readonly int SwimSpeedParameter = Animator.StringToHash("SwimSpeed");

        internal static void Apply(NormalMovement movement, float dt)
        {
            if (!GameApi.IsEnabled || !GameApi.TryGetPlayer(movement, out PlayerNetworking player))
            {
                return;
            }

            SwimmingRingVisualApplier.ApplyToPlayer(player);

            if (!GameApi.HasEquippedSwimmingRing(player))
            {
                return;
            }

            if (!Utils.CheckIsInWater(player.Actor.Center, player.transform.position, WaterSurfaceDetectionMask, WaterMask))
            {
                ResetSwimAnimation(player, dt);
                return;
            }

            player.Actor.ForceNotGrounded();
            ApplyHorizontalSwim(player, dt);
            ApplySwimAnimation(player, dt);

            Vector3 verticalVelocity = player.Actor.VerticalVelocity;
            verticalVelocity.y = Mathf.Max(verticalVelocity.y, -PluginConfig.MaxSinkSpeed.Value);
            verticalVelocity.y = Mathf.Min(PluginConfig.MaxRiseSpeed.Value, verticalVelocity.y + GetRiseForce(player) * dt);
            player.Actor.VerticalVelocity = verticalVelocity;
        }

        private static void ApplyHorizontalSwim(PlayerNetworking player, float dt)
        {
            Vector3 input = player.StateController.InputMovementReference;
            Vector3 current = player.Actor.PlanarVelocity;

            if (input.sqrMagnitude > IdleInputSqrThreshold)
            {
                Vector3 target = input.normalized * PluginConfig.SwimSpeed.Value;
                player.Actor.PlanarVelocity = Vector3.MoveTowards(current, target, PluginConfig.SwimAcceleration.Value * dt);
                return;
            }

            player.Actor.PlanarVelocity = current * PluginConfig.SwimSpeedMultiplier.Value;
        }

        private static void ApplySwimAnimation(PlayerNetworking player, float dt)
        {
            Animator animator = player.Actor.Animator;
            if (animator == null)
            {
                return;
            }

            float inputAmount = player.Controller.Brain.CharacterActions.movement.value.sqrMagnitude;
            float speedAmount = Mathf.InverseLerp(SwimAnimationMinSpeed, PluginConfig.SwimSpeed.Value, player.Actor.PlanarVelocity.magnitude);
            float target = Mathf.Max(inputAmount, speedAmount) > IdleInputSqrThreshold ? PluginConfig.SwimAnimationSpeed.Value : 0f;
            float current = animator.GetFloat(SwimSpeedParameter);
            animator.SetFloat(SwimSpeedParameter, Mathf.Lerp(current, target, PluginConfig.SwimAnimationLerp.Value * dt));
        }

        private static void ResetSwimAnimation(PlayerNetworking player, float dt)
        {
            Animator animator = player.Actor.Animator;
            if (animator == null)
            {
                return;
            }

            float current = animator.GetFloat(SwimSpeedParameter);
            animator.SetFloat(SwimSpeedParameter, Mathf.Lerp(current, 0f, PluginConfig.SwimAnimationLerp.Value * dt));
        }

        private static float GetRiseForce(PlayerNetworking player)
        {
            float force = PluginConfig.BuoyancyForce.Value;
            var brain = player.Controller.Brain;
            if (brain.CharacterActions.jump.value || brain.CharacterActions.movement.value.y > InputRiseThreshold)
            {
                force += PluginConfig.ActiveRiseForce.Value;
            }

            return force;
        }
    }
}
