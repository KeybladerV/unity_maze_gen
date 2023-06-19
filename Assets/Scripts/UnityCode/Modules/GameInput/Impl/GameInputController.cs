using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Mediation;
using UnityEngine.InputSystem;

namespace Modules.GameInput.Impl
{
    public sealed class GameInputController : IGameInputController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }

        private readonly Dictionary<GameInputType, HashSet<object>> _inputBlockers = new();

        // private DefaultControls _defaultControls;
        //
        // public DefaultControls Controls => _defaultControls;

        [Awake]
        private void OnAwake()
        {
            // _defaultControls = new DefaultControls();
            //
            // _defaultControls.General.esc.performed += EscPerformed;
            // _defaultControls.PlayerControls.jump.performed += JumpPerformed;
            //
            // _defaultControls.Enable();
        }

        [PostConstruct]
        private void PostConstruct()
        {
            foreach (GameInputType input in Enum.GetValues(typeof(GameInputType)))
                _inputBlockers[input] = new HashSet<object>();
        }

        [OnDestroy]
        private void OnDestroy()
        {
            // _defaultControls.General.esc.performed -= EscPerformed;
            // _defaultControls.PlayerControls.jump.performed -= JumpPerformed;
        }

        private void EscPerformed(InputAction.CallbackContext obj)
        {
            Dispatcher.Dispatch(GameInputEvent.EscPerformed);
        }
        
        private void JumpPerformed(InputAction.CallbackContext obj)
        {
            Dispatcher.Dispatch(GameInputEvent.JumpPerformed);
        }

        /*
         * Public.
         */

        public void BlockInput(GameInputType type, object blocker)
        {
            foreach (var inputBlocker in _inputBlockers)
            {
                if ((type & inputBlocker.Key) != 0)
                    inputBlocker.Value.Add(blocker);
            }

            ValidateControlsState();
        }

        public void UnblockInput(GameInputType type, object blocker)
        {
            foreach (var inputBlocker in _inputBlockers)
            {
                if ((type & inputBlocker.Key) != 0)
                    inputBlocker.Value.Remove(blocker);
            }
            
            ValidateControlsState();
        }
        
        private void ValidateControlsState()
        {
            // if (_inputBlockers[GameInputType.PlayerControls].Count > 0)
            //     _defaultControls.PlayerControls.Disable();
            // else
            //     _defaultControls.PlayerControls.Enable();
            //
            // if (_inputBlockers[GameInputType.General].Count > 0)
            //     _defaultControls.General.Disable();
            // else
            //     _defaultControls.General.Enable();
        }
    }
}