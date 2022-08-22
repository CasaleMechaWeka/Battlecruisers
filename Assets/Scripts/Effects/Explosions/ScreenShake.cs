using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _camera;
    // Transform of the camera to shake. Grabs the gameObject's transform
    private Transform _camTransform;
    private Vector3 _originalPos;
    //easy off and on switch
    public bool useScreenShake = true;
    // How long the object should shake for.
    public float shakeDuration = 0.5f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    

    void Start()
    {
        if (!useScreenShake)
        {
            return;
        }
        //fire at the time of the explosion becoming active
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (_camera == null) {
            return;
        }

        _camTransform = _camera.GetComponent(typeof(Transform)) as Transform;
        _originalPos = _camTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!useScreenShake) {
            return;
        }
        if (shakeDuration > 0)
        {
            _camTransform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            _camTransform.localPosition = _originalPos;
        }
    }
}
