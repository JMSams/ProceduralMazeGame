using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace FallingSloth.Localization
{
    public class TextLocalizer : MonoBehaviour
    {
        Text textField;
        TextMeshProUGUI tmpField;

        bool tmp = true;

        public KeywordList keyword;

        void Start()
        {
            tmpField = GetComponent<TextMeshProUGUI>();
            if (tmpField == null)
            {
                tmp = false;
                textField = GetComponent<Text>();
            }

            if (tmp)
                tmpField.text = LocalizationManager.GetLocalizedString(keyword);
            else
                textField.text = LocalizationManager.GetLocalizedString(keyword);
        }
    }
}