using DG.Tweening;
using UnityEngine;

namespace Location
{
    public class RotatePlatform : MonoBehaviour
    {
        [SerializeField] private Transform button;
        [SerializeField] private Transform platform;
        [SerializeField] private float rotateDuration = 0.5f;
        [SerializeField] private float buttonOffsetY = -1;
        [SerializeField] private float buttonDownDuration = 0.25f;

        public void DoRotate()
        {
            var s = DOTween.Sequence();
            s.Append(button.DOLocalMoveY(buttonOffsetY, buttonDownDuration));
            s.Append(platform.DORotate(new Vector3(0, 0, 180), rotateDuration));
        }
    }
}