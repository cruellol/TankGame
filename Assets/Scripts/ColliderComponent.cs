using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class ColliderComponent : MonoBehaviour
    {
        [SerializeField]
        private bool _destroyProjectile;
        [SerializeField]
        private bool _destroyCell;

        public bool DestroyProjectile => _destroyProjectile;
        public bool DestroyCell => _destroyCell;
    } 
}
