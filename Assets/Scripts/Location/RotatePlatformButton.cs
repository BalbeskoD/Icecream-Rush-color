using UnityEngine;

namespace Location
{
    public class RotatePlatformButton : MonoBehaviour
    {
        private RotatePlatform _platform;
        private bool _isClicked;

        private void Start()
        {
            _platform = GetComponentInParent<RotatePlatform>();
        }
        public void OnClick()
        {
            if (_isClicked)
                return;
            _isClicked = true;
            _platform.DoRotate();
        }
    }
}