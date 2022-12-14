using Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Location
{
    public class Gate : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        [FormerlySerializedAs("_colorSet")] [SerializeField] private ColorSet colorSet;
        [SerializeField] private SpriteRenderer plane;
        [SerializeField] private Sprite rainbowSprite;
        public ColorSet ColorSet => colorSet;
        public bool IsPass { get; private set; }

        public void Awake()
        {
            var mr = GetComponentsInChildren<MeshRenderer>();
            if (colorSet.Rainbow)
            {
                plane.sprite = rainbowSprite;
                foreach (var meshRenderer in mr)
                {
                    meshRenderer.material = colorSet.Rainbow;
                }
            }
            else
            {
                foreach (var meshRenderer in mr)
                {
                    meshRenderer.material.SetColor(BaseColor, colorSet.Color);
                }

                plane.color = colorSet.Color;
            }
        }

        public void Pass()
        {
            IsPass = true;
        }
    }
}