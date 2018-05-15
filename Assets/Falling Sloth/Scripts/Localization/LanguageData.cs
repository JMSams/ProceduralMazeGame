using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FallingSloth.Localization
{
    [CreateAssetMenu(fileName = "language.asset", menuName = "Localization/Language Asset", order = 0)]
    public class LanguageData : ScriptableObject
    {
        public SystemLanguage language;
        public Keyword[] values;

        public string this[KeywordList keyword]
        {
            get
            {
                Keyword[] found = values.Where<Keyword>((kw) => { return kw.key == keyword; }).ToArray();
                if (found.Length > 0) return found[0].value;
                else throw new System.ArgumentOutOfRangeException();
            }
        }

        [System.Serializable]
        public class Keyword
        {
            public KeywordList key;
            public string value;
        }
    }
}