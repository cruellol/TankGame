using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ConditionComponent : MonoBehaviour
    {
        private SimpleSpriteAnimation _tankDeathAnimation;
        protected SpriteRenderer _renderer;

        [SerializeField, Min(1)]
        private int _health = 1;

        protected virtual void Start()
        {
            Debug.Log(name+" start");
            _renderer = GetComponent<SpriteRenderer>();
            var animations = GetComponents<SimpleSpriteAnimation>();
            _tankDeathAnimation = animations.FirstOrDefault(a => a.GetAnimationType == AnimationType.Blow);
        }

        public virtual bool Immortal { get; protected set; }
        internal virtual void SetDamage(int damage)
        {
            if (Immortal) return;
            _health -= damage;
            if (_health <= 0)
            {
                Debug.Log($"{name} : is dead");
                if (_tankDeathAnimation != null)
                {
                    _tankDeathAnimation.StartAnimation();
                    _tankDeathAnimation.AnimationEndEvent += _tankDeathAnimation_AnimationEndEvent;
                }
                else
                {
                    DeleteTank();
                }
            }
        }

        private void _tankDeathAnimation_AnimationEndEvent(object sender, EventArgs e)
        {
            _tankDeathAnimation.AnimationEndEvent -= _tankDeathAnimation_AnimationEndEvent;
            DeleteTank();
        }

        private void DeleteTank()
        {
            StaticGameManager.CurrentManager.DeleteTank(gameObject);
        }
    }
}
