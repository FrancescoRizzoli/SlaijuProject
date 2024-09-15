using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UnityEngine.UI
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        TMP_Text rankT;
        [SerializeField]
        TMP_Text idT;
        [SerializeField]
        TMP_Text movesT;
        [SerializeField]
        TMP_Text TimeT;

        public void setUpData(string rank, string id, string moves, string time)
        {
            rankT.text = rank;
            idT.text = id;
            movesT.text = moves;
            TimeT.text = time;
        }
    }
}
