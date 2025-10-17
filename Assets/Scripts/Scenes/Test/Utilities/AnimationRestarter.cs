using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class AnimationRestarter : MonoBehaviour
    {
        [Header("Restart Method")]
        [Tooltip("If true, reloads the entire scene. If false, restarts animations and particles in place.")]
        public bool reloadSceneInstead = false;
        
        [ContextMenu("Restart All Animations and Particles")]
        public void RestartAllAnimationsAndParticles()
        {
            if (reloadSceneInstead)
            {
                ReloadCurrentScene();
            }
            else
            {
                int animatorCount = RestartAllAnimations();
                int particleCount = RestartAllParticleSystems();
                
                Debug.Log($"Restarted {animatorCount} animations and {particleCount} particle systems in the scene.");
            }
        }
        
        private void ReloadCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            Debug.Log($"Reloaded scene: {currentScene.name}");
        }
        
        private int RestartAllAnimations()
        {
            // Find all Animator components in the scene
            Animator[] animators = FindObjectsOfType<Animator>();
            
            foreach (Animator animator in animators)
            {
                if (animator != null && animator.runtimeAnimatorController != null)
                {
                    // Store whether it was enabled
                    bool wasEnabled = animator.enabled;
                    
                    // Disable, rebind, re-enable to ensure clean restart
                    animator.enabled = false;
                    animator.Rebind();
                    animator.enabled = wasEnabled;
                    
                    // Force update and ensure it's playing if it was enabled
                    if (wasEnabled)
                    {
                        animator.Update(0f);
                        animator.Play(0, 0, 0f); // Play default state from beginning
                    }
                }
            }
            
            return animators.Length;
        }
        
        private int RestartAllParticleSystems()
        {
            // Find all ParticleSystem components in the scene
            ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
            
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps != null)
                {
                    // Stop and clear existing particles, then restart
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play(true);
                }
            }
            
            return particleSystems.Length;
        }
    }
}