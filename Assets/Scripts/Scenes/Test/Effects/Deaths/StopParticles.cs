using UnityEngine;
using UnityEngine.UI;

public class StopParticlesOnStage : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(StopParticles);
    }

    private void StopParticles()
    {
        GameObject stage = GameObject.Find("Stage");
        if (stage != null)
        {
            ParticleSystem[] particleSystems = stage.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Stop();
            }
        }
    }
}
