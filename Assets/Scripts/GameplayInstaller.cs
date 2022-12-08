using Gachimaru.Gameplay;
using UnityEngine;
using Zenject;
using Player = Gachimaru.Gameplay.Player;

public class GameplayInstaller : MonoInstaller
{
        [SerializeField] private Player _playerBody;
        [SerializeField] private EnemyManager _enemySpawner;
        [SerializeField] private CharactersContainer _charactersContainer;

        public override void InstallBindings()
        {
                Container.Bind<Player>().FromInstance(_playerBody).AsSingle();
                Container.Bind<EnemyManager>().FromInstance(_enemySpawner).AsSingle();
                Container.Bind<CharactersContainer>().FromInstance(_charactersContainer).AsSingle();
        }
}
