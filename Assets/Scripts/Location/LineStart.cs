using System;
using UnityEngine;

namespace Location
{
    public class LineStart : MonoBehaviour
    {
        public Line Line { get; private set; }

        private void Awake()
        {
            Line = GetComponentInParent<Line>();
        }
    }
}