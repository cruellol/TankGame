using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    public static class StaticGameManager
    {
        public static GameManager CurrentManager;        

        public static int BotCount;
    }

    public class GameManager : MonoBehaviour
    {
        private List<GameObject> _enemiesList = new List<GameObject>();

        [SerializeField]
        private List<SpawnerScript> _spawners;
        [SerializeField]
        private List<InputComponent> _players;


        private void Start()
        {
            StaticGameManager.CurrentManager = this;
        }

        private void Update()
        {
            if(_enemiesList.Count < StaticGameManager.BotCount)
            {
                List<SpawnerScript> availibleSpawners = _spawners.Where(s => !s.IsSpawning).ToList();
                if (availibleSpawners != null && availibleSpawners.Count > 0)
                {
                    var currspawner = availibleSpawners[UnityEngine.Random.Range(0, availibleSpawners.Count)];
                    currspawner.Spawn(); 
                }
            }
        }

        public void AddEnemy(GameObject newEnemy)
        {
            _enemiesList.Add(newEnemy);
        }

        public void DeleteTank(GameObject tankToDelete)
        {
            if (_enemiesList.Contains(tankToDelete))
            {
                _enemiesList.Remove(tankToDelete);
            }
            Destroy(tankToDelete);
        }
    } 
}
