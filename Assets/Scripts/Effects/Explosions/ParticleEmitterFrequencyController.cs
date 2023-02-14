using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitterFrequencyController : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float minParticleSize = 2;
    public float maxParticleSize = 8;

    private float particleInterval = 1.0f;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        particleInterval -= Time.deltaTime;
        if (particleInterval <= 0)
        {
            float particleSize = Random.Range(minParticleSize, maxParticleSize);
            float particleEmissionRate = 10 / particleSize;
            int particleCount = (int)(particleEmissionRate * particleInterval);
            particleSystem.Emit(particleCount);
            particleInterval = 1.0f;
        }
    }
}
