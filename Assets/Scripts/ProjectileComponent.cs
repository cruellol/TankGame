using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent))]
    public class ProjectileComponent : MonoBehaviour
    {
        private SideType _side;
        private DirectionType _direction;
        private MoveComponent _moveComp;
        private SimpleSpriteAnimation _tankHitAnimation;

        [SerializeField]
        private int _damage = 1;
        [SerializeField]
        private float _lifeTime = 3f;

        private void Start()
        {
            _moveComp = GetComponent<MoveComponent>();
            var animations = GetComponents<SimpleSpriteAnimation>();
            _tankHitAnimation = animations.FirstOrDefault(a => a.GetAnimationType == AnimationType.Hit);
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            _moveComp.OnMove(_direction);
        }

        internal void SetParams(DirectionType direction, SideType side)
        {
            _side = side;
            _direction = direction;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var projectile = collision.GetComponent<ProjectileComponent>();
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
                Destroy(gameObject);
                return;
            }

            var fire = collision.GetComponent<FireComponent>();
            if (fire != null)
            {
                if (_side == SideType.None || fire.GetSide() == _side)
                {
                    Destroy(gameObject);
                    return;
                }
                var condition = fire.GetComponent<ConditionComponent>();
                if (condition != null)
                {
                    condition.SetDamage(_damage);
                }
                if (_tankHitAnimation != null)
                {
                    _direction = DirectionType.Error;
                    _tankHitAnimation.StartAnimation();
                    _tankHitAnimation.AnimationEndEvent += _tankHitAnimation_AnimationEndEvent;
                }
                else
                {
                    Destroy(gameObject);
                }                
            }            

            var collider = collision.GetComponent<ColliderComponent>();
            if (collider != null)
            {
                if (collider.DestroyCell) Destroy(collision.gameObject);
                if (collider.DestroyProjectile) Destroy(gameObject);
            }
        }

        private void _tankHitAnimation_AnimationEndEvent(object sender, EventArgs e)
        {
            _tankHitAnimation.AnimationEndEvent -= _tankHitAnimation_AnimationEndEvent;
            Destroy(gameObject);
        }
    }
}
