using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [Serializable]
        public class AudioWrapper
        {

            public AudioClip audioClip;
            public AudioIdentifier audioIdentifier;
            public PitchType pitchType;
            public bool oneShot = false;
        }
}
