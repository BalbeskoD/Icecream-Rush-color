using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Location
{
    public class HorizontalPlatform : MonoBehaviour
    {
        [SerializeField] private bool isSingleMove;
        [SerializeField] private float xOffset;

        [SerializeField] private float moveDuration;
        [SerializeField] private float pauseDuration;
        [SerializeField] private Ease moveEase = Ease.Linear;
        private bool _isCanMove;
        private float _startX;
        private bool _isOnStartPosition;
        private Transform _platform;
        private Coroutine _coroutine;

        private void Start()
        {
            _platform = transform.GetChild(0);
            if (!isSingleMove)
            {
                _isOnStartPosition = true;
                _isCanMove = true;
                _startX = _platform.localPosition.x;
                StartMove();
            }
        }

        private void StartMove()
        {
            if (!_isCanMove)
                return;
            _platform.DOLocalMoveX(_isOnStartPosition ? xOffset : _startX, moveDuration).SetDelay(pauseDuration)
                .SetEase(moveEase).OnComplete(() =>
                {
                    _isOnStartPosition = !_isOnStartPosition;
                    _coroutine = StartCoroutine(StartAgain());
                });
        }

        private IEnumerator StartAgain()
        {
            yield return new WaitForSeconds(pauseDuration);

            StartMove();
        }

        public void DoMove()
        {
            if (isSingleMove)
            {
                _platform.DOLocalMoveX(xOffset, moveDuration).SetEase(moveEase);
            }
            else
            {
                StopCoroutine(_coroutine);
                _isCanMove = false;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StopCoroutine(StartAgain());
            }
        }
    }
}