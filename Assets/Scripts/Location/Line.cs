using System;
using Configs;
using UnityEngine;

namespace Location
{
    public class Line : MonoBehaviour
    {
        private static readonly int Color = Shader.PropertyToID("_BaseColor");

        [SerializeField] private ParticleSystem destroyEffect;
        [SerializeField] private ParticleSystem collectEffect;

        private float _scaleSpeed;
        public ColorSet ActiveColor { get; private set; }

        public bool IsCanStart { get; private set; } = true;
        public bool IsCompleted => transform.parent.localScale.z == 0f;

        private float _startScale;

        public float CompletedValue => 1f - (transform.parent.localScale.z / _startScale);

        private void Start()
        {
            _startScale = transform.parent.localScale.z;
            destroyEffect.gameObject.SetActive(false);
            collectEffect.gameObject.SetActive(false);
        }


        public void SetColor(ColorSet configColor)
        {
            ActiveColor = configColor;
            if (configColor.Rainbow)
            {
                GetComponent<MeshRenderer>().material = configColor.Rainbow;
            }
            else
            {
                GetComponent<MeshRenderer>().material.SetColor(Color, configColor.Color);
                
            }
        }

        private void FixedUpdate()
        {
            if (!IsCanStart && !IsCompleted)
            {
                Scale(_scaleSpeed);
            }
        }

        private void Scale(float speed)
        {
            var scale = transform.parent.localScale;

            scale.z -= Time.fixedDeltaTime * (speed * ((1f / transform.parent.parent.localScale.z) * 0.1f));

            transform.parent.localScale = scale.z < 0f ? Vector3.zero : scale;
        }

        public void OnStartAction(float speed, float delay)
        {
            GetComponentInParent<Segment>().OnLineAction(this, speed, delay);
        }

        public void ChangeState(float speed, bool isMain)
        {
            IsCanStart = false;
            _scaleSpeed = speed;
            var effect = isMain ? null : destroyEffect;
            if (effect != null)
            {
                effect.gameObject.SetActive(true);
                effect.GetComponent<ParticleSystemRenderer>().material.SetColor(Color, ActiveColor.Color);
            }
            
        }
        public void FxColor(ParticleSystem fx)
        {
            fx.GetComponent<ParticleSystemRenderer>().material.SetColor(Color, ActiveColor.Color);
            //fx.GetComponentInChildren<ParticleSystemRenderer>().material.SetColor(Color, ActiveColor.Color);

        }
    }
}