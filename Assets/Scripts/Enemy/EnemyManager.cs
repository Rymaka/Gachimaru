using System.Collections;
using System.Collections.Generic;
using Gachimaru.Gameplay;
using UnityEngine;
using Zenject;

public class EnemyManager : MonoBehaviour
{
    [Inject] private CharactersContainer _charactersContainer;
    
    [SerializeField] private Enemy _pfEnemy;
    [SerializeField] private Transform _spawner;
    public List<Enemy> AliveEnemies = new List<Enemy>();
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    private IEnumerator EnemySpawner()
    {
        while (gameObject.activeSelf)
        {
            Spawn();
            yield return new WaitForSeconds(2);
        }
    }

    
    private void Spawn()
    {
        var enemy = Instantiate(_pfEnemy, _spawner.position, Quaternion.identity);
        AliveEnemies.Add(enemy);
        _charactersContainer.AddCharacter(enemy);
    }
}
