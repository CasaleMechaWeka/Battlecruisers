using UnityEngine;

public class ParticleSpeedTest : MonoBehaviour
{
    public Vector2 speed = new Vector2(10000, 0);
    public float x = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = x * speed;
        x += Time.fixedDeltaTime;
    }
}
