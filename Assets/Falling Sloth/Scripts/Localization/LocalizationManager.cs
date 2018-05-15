using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FallingSloth.Localization
{
    public class LocalizationManager : SingletonBehaviour<LocalizationManager>
    {
        public bool useLanguageOverride = false;
        public SystemLanguage languageOverride = SystemLanguage.English;

        public List<LanguageData> languageFiles;

        SystemLanguage currentLanguage
        {
            get
            {
                if (useLanguageOverride)
                    return languageOverride;
                else
                    return Application.systemLanguage;
            }
        }

        LanguageData currentData
        {
            get
            {
                return languageFiles.Where((lang) => { return lang.language == currentLanguage; }).ToArray()[0];
            }
        }

        protected override void Awake()
        {
            base.Awake();

            Debug.Log("LocalizationManager: Current language = " + currentLanguage);

            if (languageFiles.Count < 1)
                Debug.LogError("Must have at least one language file!");
        }

        public static string GetLocalizedString(KeywordList keyword)
        {
            return Instance.currentData[keyword];
        }
    }
}