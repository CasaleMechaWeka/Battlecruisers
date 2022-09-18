using BattleCruisers.Utils.Localisation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour
{
    public GameObject _TextMask;
    public GameObject _TextCompanyName;
    private TMP_Text TextBox;
    private BoxCollider2D boxCollider;
    private TMP_Text _TextBox;
    public float scrollSpeed = 50;
    private float _xPos;
    private ILocTable _advertisingTable;
    private int _scrollAdjustment;
    private int[] _randomiserArray = new int[7];
    private int _numberOfRandomAttempts = 0;

    // Start is called before the first frame update
    void Start()
    {
        _TextBox = GetComponent<TMP_Text>();
        boxCollider = _TextMask.GetComponent<BoxCollider2D>();
        setupText();
    }

    // Update is called once per frame
    void Update()
    {
        _xPos -= Time.deltaTime * scrollSpeed;
        transform.position = new Vector3(_xPos, transform.position.y, transform.position.z);
        if ((_xPos + 3000) < (2700-_scrollAdjustment)) {
            setupText();   
        }
    }

    private async void setupText() {

        _advertisingTable = await LocTableFactory.Instance.LoadAdvertisingTableAsync();
        Text textBoxCompanyName = _TextCompanyName.GetComponent<Text>();
        textBoxCompanyName.text = _advertisingTable.GetString("CompanyName");
        _xPos = boxCollider.size.x;
        transform.position = new Vector3(_xPos, transform.position.y, transform.position.z);
        int randomnumber = UnityEngine.Random.Range(1, 8);
        int numberOfRandomAttempts = 0;
        while (numberOfRandomAttempts < 6) {
            numberOfRandomAttempts += 1;
            if (_randomiserArray.Contains(randomnumber))
            {
                randomnumber = UnityEngine.Random.Range(1, 8);
            }
            else {
                break;
            }
        }

        _numberOfRandomAttempts += 1;

        _randomiserArray[_numberOfRandomAttempts] = randomnumber;
        if (_numberOfRandomAttempts > 5) {//we have to much history, clear it
            Array.Clear(_randomiserArray, 0, _randomiserArray.Length);
            _numberOfRandomAttempts = 0;
        }

        _TextBox.text = _advertisingTable.GetString("ScrollingAd/" + randomnumber);
        _scrollAdjustment = (int)(_TextBox.text.Length * 3);

    }

 
}
