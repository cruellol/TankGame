using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public enum AnimationType
    {
        Move,
        Hit,
        Blow,
        Appear
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleSpriteAnimation : MonoBehaviour
    {
        private bool _animating = false;
        private SpriteRenderer _renderer;

        [SerializeField]
        private AnimationType _type;

        public AnimationType GetAnimationType => _type;

        [SerializeField]
        private List<Sprite> _animationSprites;
        [SerializeField]
        private float _animationDelay;

        public event EventHandler AnimationEndEvent;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void StartAnimation()
        {
            if (_animating) return;
            StartCoroutine(Animate());
        }
        private IEnumerator Animate()
        {
            _animating = true;
            foreach (var anim in _animationSprites)
            {
                _renderer.sprite = anim;
                yield return new WaitForSeconds(_animationDelay);
            }
            _animating = false;
            AnimationEndEvent?.Invoke(this, null);
        }
    } 
}
