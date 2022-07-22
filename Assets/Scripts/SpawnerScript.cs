using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(SpriteRenderer), typeof(SimpleSpriteAnimation))]
    public class SpawnerScript : MonoBehaviour
    {
        private GameObject _newEnemy;
        private SpriteRenderer _renderer;
        private SimpleSpriteAnimation _appearAnimation;
        private bool _spawning = false;

        [SerializeField]
        private List<GameObject> _enemiesPrefabs;
        [SerializeField]
        private float _spawnerDelay=3f;

        public bool IsSpawning => _spawning;

        // Start is called before the first frame update
        void Start()
        {
            _appearAnimation = GetComponent<SimpleSpriteAnimation>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Spawn()
        {
            StartCoroutine(Spawning());
        }

        private IEnumerator Spawning()
        {
            _spawning = true;
            _renderer.enabled = true;
            _newEnemy = Instantiate(_enemiesPrefabs[UnityEngine.Random.Range(0, _enemiesPrefabs.Count)], transform.localPosition, Quaternion.Euler(Vector3.zero));
            StaticGameManager.CurrentManager.AddEnemy(_newEnemy);
            _newEnemy.SetActive(false);
            _appearAnimation.AnimationEndEvent += _appearAnimation_AnimationEndEvent;
            _appearAnimation.StartAnimation();
            yield return new WaitForSeconds(_spawnerDelay);
            _spawning = false;
        }

        private void _appearAnimation_AnimationEndEvent(object sender, System.EventArgs e)
        {
            _appearAnimation.AnimationEndEvent -= _appearAnimation_AnimationEndEvent;
            _renderer.enabled = false;
            //GameManager.AddEnemy(Instantiate(_enemiesPrefabs[UnityEngine.Random.Range(0, _enemiesPrefabs.Count)], transform.localPosition, Quaternion.Euler(Vector3.zero)));
            _newEnemy.SetActive(true);
        }
    }
}
