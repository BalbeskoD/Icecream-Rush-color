using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class FinishBonus : MonoBehaviour
    {
        private TextMeshProUGUI _bonusText;

        private SignalBus _signalBus;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _bonusText = GetComponentInChildren<TextMeshProUGUI>();
            _signalBus.Subscribe<FinishBonusAddSignal>(OnFinishBonusAdd);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<FinishBonusAddSignal>(OnFinishBonusAdd);
        }

        private void OnEnable()
        {
            _bonusText.text = string.Empty;
        }

        private void OnFinishBonusAdd(FinishBonusAddSignal signal)
        {
            _bonusText.text = $"x{signal.Value}";
            var start = _bonusText.fontSize;
            var target = start * 1.5f;
            var s = DOTween.Sequence();
            s.Append(DOTween.To(() => _bonusText.fontSize, x => _bonusText.fontSize = x, target, 0.25f));
            s.Append(DOTween.To(() => _bonusText.fontSize, x => _bonusText.fontSize = x, start, 0.25f));
        }
    }
}