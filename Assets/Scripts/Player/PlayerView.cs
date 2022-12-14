using System;
using Configs;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        [SerializeField] private MeshRenderer mainPart;
        [SerializeField] private Transform mainTransform;
        [SerializeField] private Sprite rainbowSprite;
        private TrailRenderer[] _trails;
        private Material _playerMaterial;
        private Sequence _colorSequence;
        private Sequence _moveSequence;
        private Sequence _rotateSequence;
        private PlayerSettings _playerSettings;

        [Inject]
        public void Construct(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }

        private void Awake()
        {
            _trails = GetComponentsInChildren<TrailRenderer>();
            _playerMaterial = mainPart.material;
            SwitchTrails(false);
        }

        public void ChangeColor(ColorSet color)
        {
            _rotateSequence?.Kill(true);
            var view = transform.GetChild(0);
            _rotateSequence = DOTween.Sequence();
            _rotateSequence.AppendCallback(() => StopTrails(false));
            _rotateSequence.Append(view.DOLocalMoveY(0, 0.1f));
            _rotateSequence.Append(view.DOLocalRotate(new Vector3(0, 180, 0), 0.1f, RotateMode.LocalAxisAdd));
            _rotateSequence.AppendCallback(() =>
            {
                if (color.Rainbow)
                {
                    mainPart.material = color.Rainbow;
                }
                else
                {
                    mainPart.material = _playerMaterial;
                    foreach (var trailRenderer in _trails)
                    {
                        trailRenderer.startColor = color.Color;
                    }

                    mainPart.material.SetColor(BaseColor, color.Color);
                }
            });
            _rotateSequence.Append(view.DOLocalRotate(new Vector3(0, 180, 0), 0.1f, RotateMode.LocalAxisAdd));
            _rotateSequence.Append(view.DOLocalMoveY(-0.6f, 0.1f));
            _rotateSequence.AppendCallback(() => StopTrails(true));
        }

        public void SwitchTrails(bool isActive)
        {
            foreach (var trailRenderer in _trails)
            {
                trailRenderer.enabled = isActive;
                trailRenderer.Clear();
            }
        }

        private void StopTrails(bool isActive)
        {
            foreach (var trailRenderer in _trails)
            {
                trailRenderer.emitting = isActive;
            }
        }

        public void Blink()
        {
            _colorSequence?.Kill(true);

            var startColor = _playerMaterial.color;
            _colorSequence = DOTween.Sequence();
            _colorSequence.Append(_playerMaterial.DOColor(Color.white, 0.1f)).Join(mainTransform.DOLocalRotate(_playerSettings.WrongRollRotation, 0.1f));
            _colorSequence.Append(_playerMaterial.DOColor(startColor, 0.1f)).Join(mainTransform.DOLocalRotate(_playerSettings.BaseRotation, 0.1f));
        }
        public void BlinkWithoutRotate()
        {
            _colorSequence?.Kill(true);

            var startColor = _playerMaterial.color;
            _colorSequence = DOTween.Sequence();
            _colorSequence.Append(_playerMaterial.DOColor(Color.white, 0.1f));
            _colorSequence.Append(_playerMaterial.DOColor(startColor, 0.1f));
        }

        public void ThrowRoll()
        {
            _moveSequence?.Kill(true);
            _moveSequence = DOTween.Sequence();
            _moveSequence.Append(mainTransform.DOLocalMove(_playerSettings.ThrowRollPosition, 0.1f));
            _moveSequence.Append(mainTransform.DOLocalMove(_playerSettings.BasePosition, 0.1f)).Join(mainTransform.DOLocalRotate(_playerSettings.BaseRotation, 0.1f));
        }

        public void OnRoll(float value)
        {
            _moveSequence?.Kill(true);
            _colorSequence?.Kill(true);
            mainTransform.localRotation = Quaternion.Lerp(mainTransform.localRotation, Quaternion.Euler(_playerSettings.CompletedRollRotation), value);
            mainTransform.localPosition = Vector3.Lerp(mainTransform.localPosition, _playerSettings.CompletedRollPosition, value);
        }
    }
}