using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth
{
    [CreateAssetMenu(fileName = "DreamLoData", menuName = "Dream Lo/Key Data File")]
    public class DreamLoData : ScriptableObject
    {
        public string privateKey;
        public string publicKey;
    }
}