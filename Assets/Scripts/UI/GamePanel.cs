using System;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        private SignalBus _signalBus;
        private ProgressBar _progressBar;
        private FinishBonus _finishBonus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _progressBar = GetComponentInChildren<ProgressBar>();
            _finishBonus = GetComponentInChildren<FinishBonus>();
            _signalBus.Subscribe<PlayerFinishActionSignal>(OnFinish);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PlayerFinishActionSignal>(OnFinish);
        }

        private void OnFinish()
        {
            _progressBar.gameObject.SetActive(false);
            _finishBonus.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            _progressBar.gameObject.SetActive(true);
            _finishBonus.gameObject.SetActive(false);
        }
    }
}