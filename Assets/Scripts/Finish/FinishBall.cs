using System;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Finish
{
    public class FinishBall : MonoBehaviour
    {
        [SerializeField] private ColorSet colorSet;
        [SerializeField] private float[] speed;
        [SerializeField] private MeshRenderer mainPart;
        private Sequence _colorSequence;
        private Material _ballMaterial;

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private Rigidbody _rigidbody;
        private bool _isMoveRight = true;

        
        private int speedIndex;

        public bool IsCanBeDrop { get; private set; } = true;
        private bool _isFloor;
        private ParticleSystem _particleSystem;
       
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _particleSystem = GetComponentInChildren<ParticleSystem>(true);
            _ballMaterial = mainPart.material;
            if (_particleSystem)
                _particleSystem.gameObject.SetActive(false);
            foreach (var material in GetComponent<MeshRenderer>().materials)
            {
                material.SetColor(BaseColor, colorSet == null ? Random.ColorHSV() : colorSet.Color);
            }
            speedIndex = Random.Range(0, speed.Length);
        }

        private void FixedUpdate()
        {
            if (!IsCanBeDrop)
                return;
            if (_isMoveRight)
            {
                _rigidbody.MovePosition(_rigidbody.position + Vector3.right * (speed[speedIndex]  * Time.deltaTime));
                if (_rigidbody.position.x >= 2)
                {
                    _isMoveRight = false;
                }
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position + Vector3.left * (speed[speedIndex] * Time.deltaTime));
                if (_rigidbody.position.x <= -2)
                {
                    _isMoveRight = true;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Finish>())
            {
                _isFloor = true;
            }

            if (_particleSystem && !_particleSystem.gameObject.activeInHierarchy)
            {
                _particleSystem.startColor = colorSet.Color;
                _particleSystem.gameObject.SetActive(true);
                Blink();
            }
        }


        public async UniTask<bool> Drop()
        {
            IsCanBeDrop = false;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Physics.gravity;
            await UniTask.WaitForFixedUpdate();
            await UniTask.WaitUntil(() => _rigidbody.velocity == Vector3.zero || _isFloor);

            _rigidbody.isKinematic = true;
            return !_isFloor;
        }

        public void Blink()
        {
            _colorSequence?.Kill(true);

            var startColor = _ballMaterial.color;
            _colorSequence = DOTween.Sequence();
            _colorSequence.Append(_ballMaterial.DOColor(Color.white, 0.1f));
            _colorSequence.Append(_ballMaterial.DOColor(startColor, 0.1f));
        }
    }
}