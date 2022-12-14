using System;
using Configs;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Player
{
    public class PlayerDeathCube : MonoBehaviour
    {
        private float _startPosition;
        private Tween _tween;
        private float _timer;
        private PlayerSettings _playerSettings;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(PlayerSettings playerSettings, SignalBus signalBus)
        {
            _playerSettings = playerSettings;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _startPosition = transform.localPosition.z;
            _signalBus.Subscribe<GameRestartSignal>(OnRestart);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
        }

        private void OnRestart()
        {
            var pos = transform.localPosition;
            pos.z = _startPosition;
            transform.localPosition = pos;
        }

        public void MoveToPlayer()
        {
            _timer += Time.fixedDeltaTime;
            if (_timer < 0.5f)
                return;

            var pos = transform.localPosition;
            pos.z += Time.fixedDeltaTime * _playerSettings.DeathCubeSpeed;
            transform.localPosition = pos;
            if (!(pos.z < -1))
            {
                _signalBus.Fire<PlayerFailSignal>();
            }
        }

        public void MoveToStart()
        {
            _timer = 0;
            if (_tween == null && Math.Abs(transform.localPosition.z - _startPosition) > 0)
            {
                _tween = transform.DOLocalMoveZ(_startPosition, 0.25f).OnComplete(() => _tween = null);
            }
        }
    }
}