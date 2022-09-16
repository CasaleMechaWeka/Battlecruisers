using BattleCruisers.Utils.Localisation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour
{
    public GameObject _TextMask;
    public GameObject _TextCompanyName;
    private TMP_Text TextBox;
    private BoxCollider2D boxCollider;
    private string _message = "";
    public float scrollSpeed = 50;
    private float _xPos;
    private ILocTable _advertisingTable;

    // Start is called before the first frame update
    void Start()
    {
        TextBox = GetComponent<TMP_Text>();
        _message = TextBox.text;
        boxCollider = _TextMask.GetComponent<BoxCollider2D>();
        setupText();
    }

    // Update is called once per frame
    void Update()
    {
        _xPos -= Time.deltaTime * scrollSpeed;
        transform.position = new Vector3(_xPos, transform.position.y, transform.position.z);
        if ((_xPos + 3000) < 2700) {
            setupText();   
        }
    }

    private async void setupText() {

        _advertisingTable = await LocTableFactory.Instance.LoadAdvertisingTableAsync();
        Text textBoxCompanyName = _TextCompanyName.GetComponent<Text>();
        textBoxCompanyName.text = _advertisingTable.GetString("CompanyName");
        _xPos = boxCollider.size.x;
        transform.position = new Vector3(_xPos, transform.position.y, transform.position.z);
    }

    private void scrollText()
    {
        // Set up the message's rect if we haven't already
        if (messageRect.width == 0)
        {
            Vector2 dimensions = GUI.skin.label.CalcSize(new GUIContent(_message));

            // Start the message past the left side of the screen
            messageRect.x = +dimensions.x;
            messageRect.width = dimensions.x;
            messageRect.height = dimensions.y;
        }

        messageRect.x -= Time.deltaTime * scrollSpeed;

        // If the message has moved past the right side, move it back to the left
        if (messageRect.x > Screen.width)
        {
            messageRect.x = -messageRect.width;
        }

        GUI.Label(messageRect, _message);
    }

    Rect messageRect;

    void OnGUI()
    {
       /* // Set up the message's rect if we haven't already
        if (messageRect.width == 0)
        {
            Vector2 dimensions = GUI.skin.label.CalcSize(new GUIContent(_message));

            // Start the message past the left side of the screen
            messageRect.x = +dimensions.x;
            messageRect.width = dimensions.x;
            messageRect.height = dimensions.y;
        }

        messageRect.x -= Time.deltaTime * scrollSpeed;

        // If the message has moved past the right side, move it back to the left
        if (messageRect.x > Screen.width)
        {
            messageRect.x = -messageRect.width;
        }

        GUI.Label(messageRect, _message);*/
    }
}
