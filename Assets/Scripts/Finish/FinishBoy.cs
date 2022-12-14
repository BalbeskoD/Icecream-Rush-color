using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Finish
{
    public class FinishBoy : MonoBehaviour
    {
        [SerializeField] private Transform point;
        [SerializeField] private float openSpeed = 100;
        [SerializeField] private SkinnedMeshRenderer face;
        private Animator _animator;
        private static readonly int Mouth = Animator.StringToHash("OpenMouth");
        private static readonly int Close = Animator.StringToHash("CloseMouth");
        private static readonly int Stakan = Animator.StringToHash("Stakan");


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public Transform OpenMouth()
        {
            _animator.SetTrigger(Mouth);
            return point;
        }

        public void CloseMouth()
        {
            _animator.SetTrigger(Close);
        }
        public void StakanTake()
        {
            _animator.SetTrigger(Stakan);
        }

        public void DoScale()
        {
            
        }
    }
}