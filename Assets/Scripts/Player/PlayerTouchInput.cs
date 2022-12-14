using UnityEngine;

namespace Player
{
    public class PlayerTouchInput : MonoBehaviour
    {
        private bool isHold;
        private float offset;
        public bool IsHoldTouch => isHold;
        public float Offset => -offset;

        private float prevPosition;
        private float screenWidth;

        private void Awake()
        {
            Input.multiTouchEnabled = false;
            screenWidth = Screen.width;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHold = true;
                prevPosition = Input.mousePosition.x;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isHold = false;
                prevPosition = 0;
                offset = 0;
            }

            if (isHold)
            {
                var currentPos = Input.mousePosition.x;
                offset = prevPosition - currentPos;
                offset /= screenWidth;
                prevPosition = currentPos;
            }
        }
    }
}