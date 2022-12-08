using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Gachimaru.InputSystem;
using UnityEditor;

namespace Gachimaru.Gameplay
{

    public class BlinkShaderSkript : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private float _dissapearingTime;
        [SerializeField] private float _appearingTime;
        //should be changed and synched with dashing
        public float DissapearingTime => _dissapearingTime;
        //will be updating from dashing scrpipt
         private bool _isDissapearing => _player.SummonersDash.IsDissapearing;
         private bool _isAppearing => _player.SummonersDash.IsAppearing;
        [SerializeField] private float _blinking;
        [SerializeField] private float _appearing;
        [SerializeField] private float _dissapearingPoint = 0.6f;
        [SerializeField] private float _zeroPoint = -3f;
        [SerializeField] private float _startingPoint;
        [SerializeField] private Material _material;
        private void Start()
        {
            _blinking = _zeroPoint;
            _appearing = _dissapearingPoint;
        }

        private void FixedUpdate()
        {
            BlinkAnimation();
        }

        private void BlinkAnimation()
        {
            var test = true;
            
            if (_isDissapearing)
            {
                Dissapear();
            }

            if (!_isAppearing)
            {
                _appearing = _dissapearingPoint;

            }

            if (_isAppearing && !_isDissapearing)
            {
                
                Appear();

            }
 
        }

        private void Dissapear()
        {
            DOTween.To(() => _blinking, x => _blinking = x, _dissapearingPoint, _dissapearingTime);
            _material.SetFloat("EffectTimer", _blinking);
        }

        private void Appear()
        {
            DOTween.To(() => _appearing, x => _appearing = x, _zeroPoint, _appearingTime);
            _material.SetFloat("EffectTimer", _appearing);
            _blinking = _zeroPoint;
        }
    }
}