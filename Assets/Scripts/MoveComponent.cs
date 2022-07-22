using System.Linq;
using UnityEngine;

namespace Tanks
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private bool _animate = true;

        private DirectionType _lastDirection = DirectionType.Top;
        public DirectionType Direction => _lastDirection;
        private SimpleSpriteAnimation _moveAnimation;


        private void Start()
        {
            var animations = GetComponents<SimpleSpriteAnimation>();
            _moveAnimation = animations.FirstOrDefault(a => a.GetAnimationType == AnimationType.Move);
        }

        public void OnMove(DirectionType type)
        {
            if (type == DirectionType.Error) return;
            transform.position += type.GetVectorFromDirection() * (Time.deltaTime * _speed);
            transform.eulerAngles = type.GetVectorFromRotation();
            _lastDirection = type;

            if (_moveAnimation != null) _moveAnimation.StartAnimation();
        }
    }
}