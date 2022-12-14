using UnityEngine;

namespace Finish
{
    public class FinishLine : MonoBehaviour
    {
        public Finish Finish { get; private set; }

        private void Awake()
        {
            Finish = GetComponentInChildren<Finish>();
        }
    }
}