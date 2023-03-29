using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DefaultAdvertController : MonoBehaviour
{
    [SerializeField]
    Sprite[] _fakeAds;

    Image _image;
    bool _isactive;

    // Start is called before the first frame update
    void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (_isactive == true)
        {
            if (_image && _fakeAds.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, _fakeAds.Length);
                _image.sprite = _fakeAds[randomIndex];
            }
            _isactive = false;
        }
    }

    public void UpdateImage()
    {
        _isactive = true;
    }
}
