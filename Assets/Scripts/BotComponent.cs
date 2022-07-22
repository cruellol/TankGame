using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent))]
    public class BotComponent : MonoBehaviour
    {
        private MoveComponent _moveComp;
        private FireComponent _fireComp;
        private Coroutine _chooseCoroutine;

        [SerializeField]
        private float _secondsToChangeDirection = 1f;
        [SerializeField]
        private float _secondsToResetPlayerSeen = 2f;

        // Start is called before the first frame update
        void Start()
        {
            _moveComp = GetComponent<MoveComponent>();
            _fireComp = GetComponent<FireComponent>();
            _chooseCoroutine = StartCoroutine(WhenToChooseNext());
        }

        IEnumerator WhenToChooseNext()
        {
            while (true)
            {
                yield return new WaitForSeconds(_secondsToChangeDirection);
                chooseNext = true;
            }
        }

        private void PreventiveChoose(bool choose = true)
        {
            chooseNext = choose;
            StopCoroutine(_chooseCoroutine);
            _chooseCoroutine = StartCoroutine(WhenToChooseNext());
        }

        private bool chooseNext = true;
        private DirectionType _direction;
        // Update is called once per frame
        void Update()
        {
            SearchWalls();

            if (chooseNext)
            {
                var lastdirection = _direction;

                while (lastdirection == _direction)
                {
                    _direction = (DirectionType)UnityEngine.Random.Range(1, 5);
                }

                chooseNext = false;
            }

            _moveComp.OnMove(_direction);
            _fireComp.OnFire();
        }

        private void SearchWalls()
        {
            bool playerSearch = SearchPlayer();
            if (!playerSearch && !_playerSeen)
            {
                RaycastHit2D[] searchWallHits = Physics2D.RaycastAll(transform.position, _moveComp.Direction.GetVectorFromDirection(), 1f);
                foreach (var hit in searchWallHits)
                {
                    var collider = hit.collider.GetComponent<ColliderComponent>();
                    if (collider != null)
                    {
                        if (!collider.DestroyCell)
                        {
                            PreventiveChoose();
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        private Coroutine _playerSeenCoroutine;
        private bool _playerSeen = false;
        private IEnumerator PlayerWasSeen()
        {
            _playerSeen = true;
            yield return new WaitForSeconds(_secondsToResetPlayerSeen);
            _playerSeen = false;
        }

        public bool SearchPlayer()
        {
            bool result = false;
            foreach (DirectionType direction in Enum.GetValues(typeof(DirectionType)))
            {
                if (direction == DirectionType.Error) continue;
                RaycastHit2D[] searchPlayerHits = Physics2D.RaycastAll(transform.position, direction.GetVectorFromDirection(), 10f);
                foreach (var hit in searchPlayerHits)
                {
                    var collider = hit.collider.GetComponent<ColliderComponent>();
                    if (collider != null)
                    {
                        if (!collider.DestroyCell && collider.DestroyProjectile)
                        {
                            break;
                        }
                    }

                    var playerComponent = hit.collider.GetComponent<PlayerConditionComponent>();
                    if (playerComponent != null)
                    {
                        PreventiveChoose(false);
                        _direction = direction;
                        if (_playerSeenCoroutine != null)
                        {
                            StopCoroutine(_playerSeenCoroutine);
                        }
                        _playerSeenCoroutine = StartCoroutine(PlayerWasSeen());
                        return true;
                    }
                }
            }
            return result;
        }
    }
}
