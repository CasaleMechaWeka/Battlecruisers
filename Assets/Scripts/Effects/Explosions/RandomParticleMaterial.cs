using UnityEngine;

public class RandomParticleMaterial : MonoBehaviour
{
    public Material[] materials;
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        int index = Random.Range(0, materials.Length);
        ps.GetComponent<ParticleSystemRenderer>().material = materials[index];
        ps.Play();
    }
}
