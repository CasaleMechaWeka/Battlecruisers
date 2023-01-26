using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FakeAdBanner : MonoBehaviour
{
    [SerializeField]
    Sprite[] _fakeAds;

    Image _image;

    // Start is called before the first frame update
    void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (_image && _fakeAds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _fakeAds.Length);
            _image.sprite = _fakeAds[randomIndex];
        }
    }
}
