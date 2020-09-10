using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTag : MonoBehaviour
{
    public Text textLabel;
    public int offsetY;
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 textPos = Camera.main.WorldToScreenPoint(this.transform.position);

        textLabel.transform.position = new Vector3(textPos.x, textPos.y + offsetY, textPos.z);
    }

    public void SetText(string newText) {
        textLabel.text = newText;
    }
}
