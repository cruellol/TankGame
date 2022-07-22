using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class PlayerConditionComponent : ConditionComponent
    {
        private Vector3 _startPosition;

        [SerializeField]
        private float _immortalTime = 2f;
        [SerializeField]
        private float _opacityShift = 0.01f;
        protected override void Start()
        {
            base.Start();
            _startPosition = transform.localPosition;
        }

        internal override void SetDamage(int damage)
        {
            if (Immortal) return;
            StartCoroutine(SetImmortal());
            transform.localPosition = _startPosition;

        }
        private IEnumerator SetImmortal()
        {
            Immortal = true;
            var animationCoroutine = StartCoroutine(AnimateImmortallity());
            yield return new WaitForSeconds(_immortalTime);
            StopCoroutine(animationCoroutine);
            var color = _renderer.color;
            color.a = 1;
            _renderer.color = color;
            Immortal = false;
        }

        private IEnumerator AnimateImmortallity()
        {
            bool decrease = true;
            while (true)
            {
                var color = _renderer.color;
                yield return null;
                if (decrease)
                {
                    color.a -= _opacityShift;
                }
                else
                {
                    color.a += _opacityShift;
                }
                if (color.a > 0.98) decrease = true;
                if (color.a < 0.3) decrease = false;
                _renderer.color = color;
            }
        }
    }
}
