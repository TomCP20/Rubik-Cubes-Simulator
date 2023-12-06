using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

/*
A component that controlls the bars in the bar chart.
bar is a reference to the image on the screen that represents the bar in the bar chart.
label is a text box shown at the bottom of the bar that shows the range the bar represents.
barValueis a text box shown at the top of the bar that shows the value of the bar.
setBarHeight sets the height of the bar image.
SetBarColor sets the colour of the bar image.
SetLabelText sets the Text of label.
SetBarValueText sets the text of barValue
*/

namespace BarChart
{
    public class Bar : MonoBehaviour
    {
        [SerializeField]
        private Image bar;
        [SerializeField]
        private TextMeshProUGUI label;
        [SerializeField]
        private TextMeshProUGUI barValue;

        public void setBarHeight(float height)
        {
            RectTransform rt = bar.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
        }

        public void SetLabelText(string newText)
        {
            label.text = newText;
        }

        public void SetBarValueText(string newText)
        {
            barValue.text = newText;
        }

        public void SetBarColor(Color newColor)
        {
            bar.color = newColor;
        }
    }
}