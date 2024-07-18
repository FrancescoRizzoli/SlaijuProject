using UnityEngine;
using UnityEngine.Audio;
using Utility;

namespace Grid.Cell
{
    public class ShieldedCityCell : CityCell
    {
        [SerializeField] private GameObject shieldEffectGameobject = null;
        [SerializeField] private AudioMixerGroup mixerGroup = null;
        [SerializeField] private AudioClip shieldOffAudioClip = null;

        public void RemoveShields()
        {
            EnableAllSafeSides();
            shieldEffectGameobject.SetActive(false);
            AudioManager.instance.PlayAudioClip(shieldOffAudioClip, mixerGroup);
        }
    }
}
