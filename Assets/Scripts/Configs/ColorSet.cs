using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ColorSet", menuName = "Configs/ColorSet", order = 0)]
    public class ColorSet : ScriptableObject
    {
        [SerializeField] private Material rainbow;
        [SerializeField] private Color color;
        public Color Color => color;

        public Material Rainbow => rainbow;
    }
}