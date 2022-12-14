using System;
using Configs;
using Cysharp.Threading.Tasks;
using Location;
using UI;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Player
{
    public class PlayerMoveController : MonoBehaviour
    {
        private PlayerTouchInput _playerTouchInput;
        private PlayerDeathCube _deathCube;
        private Rigidbody _rigidbody;
        private bool _isActive;
        private PlayerSettings _playerSettings;
        private int _modification;
        private bool _isOnLine;
        private int _activeIndex;
        private float _limit;
        private bool _isFinishMove;
        private float _deathTimer;
        private SignalBus _signalBus;
        private LevelLimits _levelLimits;
        private ColorSet _colorSet;
        private Line _activeLine;
        private Player _player;


        [Inject]
        public void Construct(PlayerSettings playerSettings, SignalBus signalBus)
        {
            _playerSettings = playerSettings;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _playerTouchInput = GetComponent<PlayerTouchInput>();
            _deathCube = GetComponentInChildren<PlayerDeathCube>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetActive(bool isActive, LevelLimits levelLimits = null)
        {
            _levelLimits = levelLimits;
            _isActive = isActive;
            _modification = _isActive ? 1 : 0;
            _deathTimer = 0;
            _isFinishMove = false;
        }

        private void FixedUpdate()
        {
            if (!_isActive || _isFinishMove) return;
            if (_activeLine)
            {
                if (_activeLine.IsCompleted)
                {
                    _activeLine = null;
                    _player.ThrowRoll();
                }
                else
                {
                    _player.OnLineMove(_activeLine.CompletedValue);
                }
            }

            Movement();
            DeathCubeAction();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void DeathCubeAction()
        {
            if (!_activeLine && _modification == 0)
            {
                _deathCube.MoveToPlayer();
            }
            else if (!_activeLine)
            {
                _deathCube.MoveToStart();
            }
        }

        private void Movement()
        {
            var deltaOffset = _playerTouchInput.IsHoldTouch ? _playerTouchInput.Offset : 0;
            var x = _activeLine ? 0 : _playerSettings.SpeedSide * Time.deltaTime * deltaOffset;
            var z = (_activeLine
                ? _playerSettings.SpeedForwardOnLine
                : _playerSettings.SpeedForward) * Time.deltaTime * _modification;
            var offset = new Vector3(x, 0, z);
            var target = _rigidbody.position + offset;
            if (_levelLimits != null)
                target.x = Mathf.Clamp(target.x, _levelLimits.LeftLimit, _levelLimits.RightLimit);
            if (_activeLine)
            {
                target.x = Mathf.Lerp(target.x, _activeLine.transform.position.x,
                    Time.deltaTime * _playerSettings.SpeedSide * 0.1f);
            }

            _rigidbody.MovePosition(target);
        }

        public bool OnLine(Line line)
        {
            if (_activeLine || !line.IsCanStart)
                return false;
            if (_colorSet.Rainbow || line.ActiveColor == _colorSet)
            {
                _activeLine = line;
                _modification = 1;
            }
            else
            {
                _modification = 0;
            }

            return _modification == 1;
        }

        public void OnGate(ColorSet colorSet)
        {
            _colorSet = colorSet;
        }
    }
}