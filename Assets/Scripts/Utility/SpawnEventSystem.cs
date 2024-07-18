using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Utility
{
   
       using UnityEngine.Assertions;
	using UnityEngine.EventSystems;

    public sealed class SpawnEventSystem : MonoBehaviour
    {
        #region Private variables
        [SerializeField]
        private EventSystem eventSystemPrefab;
        [SerializeField]
        private bool persist = true;
        #endregion
        #region Lifecycle
        void Awake()
        {


            if (EventSystem.current == null)
            {
                EventSystem eventSysteminstance = Instantiate<EventSystem>(eventSystemPrefab);
#if UNITY_EDITOR
                eventSysteminstance.name = eventSystemPrefab.name;
                if (persist)
                    eventSysteminstance.name += " (Persistent)";
#endif
                if (persist)
                    DontDestroyOnLoad(eventSysteminstance.gameObject);
            }
        }
        #endregion
    
}
}
