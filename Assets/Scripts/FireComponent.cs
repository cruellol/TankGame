using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent))]
    public class FireComponent : MonoBehaviour
    {
        private MoveComponent _moveComp;
        private bool _canFire = true;
        [SerializeField, Range(0.1f, 2f)]
        private float _delayFire = 0.25f;
        [SerializeField]
        private ProjectileComponent _prefab;
        [SerializeField]
        private SideType _side;
        [SerializeField]
        private Transform _firePoint;

        private void Start()
        {
            _moveComp = GetComponent<MoveComponent>();
        }
        public void OnFire()
        {
            if (!_canFire) return;

            if (_firePoint == null) _firePoint = transform;
            var bullet = Instantiate(_prefab, _firePoint.position, transform.rotation);
            bullet.SetParams(_moveComp.Direction, _side);
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            _canFire = false;
            yield return new WaitForSeconds(_delayFire);
            _canFire = true;
        }

        internal SideType GetSide() => _side;
    }
}
