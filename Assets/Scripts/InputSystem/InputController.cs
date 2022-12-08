using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gachimaru.InputSystem
{
    public class InputController : MonoBehaviour
    {
        private static List<KeyCode> _listeningKeys = new List<KeyCode>();

        private static Dictionary<KeyCode, List<Action>> _bindedActions = new Dictionary<KeyCode, List<Action>>();

        public static Vector2 Axis => _axis;
        public  float FloatAxisVertical => _floatAxisVertical;
        public  float FloatAxisHorizontal => _floatAxisHorizontal;


        private static Vector2 _axis;
        private static float _floatAxisHorizontal;
        private static float _floatAxisVertical;
        

        private void Update()
        {
            ListenKeys();
            ListenAxis();
        }

        public static void AddActionOnKey(KeyCode key, Action action)
        {
            TryAddBindedKey(key);
            TryAddNewListeningKey(key);
            
            var actions = _bindedActions[key];
            actions.Add(action);
        }

        private static void TryAddBindedKey(KeyCode key)
        {
            if (!_bindedActions.ContainsKey(key))
            {
                _bindedActions.Add(key, new List<Action>());
            }
        }
        
        private static void TryAddNewListeningKey(KeyCode key)
        {
            if (_listeningKeys.Contains(key)) return;
            _listeningKeys.Add(key);
        }
        

        private void ListenKeys()
        {
            foreach (var key in _listeningKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    var actions = _bindedActions[key];

                    foreach (var action in actions)
                    {
                        action.Invoke();
                    }
                }
            }
        }

        private void ListenAxis()
        {
            _axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _floatAxisHorizontal = Input.GetAxisRaw("Horizontal");
            _floatAxisVertical = Input.GetAxisRaw("Vertical");
        }
    }
}

