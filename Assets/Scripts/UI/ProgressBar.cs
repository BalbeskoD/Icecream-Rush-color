using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        private Slider _slider;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _signalBus.Subscribe<PlayerProgressBarSignal>(SetValue);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PlayerProgressBarSignal>(SetValue);
        }

        private void SetValue(PlayerProgressBarSignal signal)
        {
            _slider.value = signal.Value;
        }
    }
}