//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/script/Controller.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controller: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controller"",
    ""maps"": [
        {
            ""name"": ""player"",
            ""id"": ""f5537731-d246-4718-a621-c8d96c21c63a"",
            ""actions"": [
                {
                    ""name"": ""movement"",
                    ""type"": ""Value"",
                    ""id"": ""c9c7fecd-ef2e-4543-b7ca-f7beb85a25ac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""dash"",
                    ""type"": ""Button"",
                    ""id"": ""1970f09e-5584-4d35-a5c4-4bf5ac0c51e9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""jump"",
                    ""type"": ""Button"",
                    ""id"": ""29ba044e-cd4e-4ee9-a6e7-38e27ad22fc5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""camera"",
                    ""type"": ""Value"",
                    ""id"": ""bd91cc9e-82c0-480b-85e9-568728f4e099"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""slide"",
                    ""type"": ""Button"",
                    ""id"": ""5d82174b-5d0f-4607-bf9f-f8e4b3b5781e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Grapple"",
                    ""type"": ""Button"",
                    ""id"": ""ff3104c6-eeba-400c-9897-029045274477"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GrappleHold"",
                    ""type"": ""Button"",
                    ""id"": ""dd8c292f-2eab-4a85-9ab1-4f5fd546e4b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""01b0bec8-aef2-47ab-9927-5d78fec25d2e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""571de388-89e1-46dd-9227-9b25bd085c05"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""93b8c03a-3d2a-4c27-8481-57ef5b263edd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a342c4f7-3e35-441d-a108-84240a97a769"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ce4eb428-9437-4ced-8b8d-c202fdc8048b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c6b82302-b7ed-4918-9819-d27fc554e1ea"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc010098-9f62-49e9-bac8-91fe07f52b8e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ce73021-8054-400b-8d75-176781998936"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6418d2d-ac2a-4dd6-84a0-3303b463d97a"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f32c21f-a2a4-42d6-9000-95517b2a9095"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grapple"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""133b51d0-199a-4ecb-b5ba-85dbf1806622"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // player
        m_player = asset.FindActionMap("player", throwIfNotFound: true);
        m_player_movement = m_player.FindAction("movement", throwIfNotFound: true);
        m_player_dash = m_player.FindAction("dash", throwIfNotFound: true);
        m_player_jump = m_player.FindAction("jump", throwIfNotFound: true);
        m_player_camera = m_player.FindAction("camera", throwIfNotFound: true);
        m_player_slide = m_player.FindAction("slide", throwIfNotFound: true);
        m_player_Grapple = m_player.FindAction("Grapple", throwIfNotFound: true);
        m_player_GrappleHold = m_player.FindAction("GrappleHold", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // player
    private readonly InputActionMap m_player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_player_movement;
    private readonly InputAction m_player_dash;
    private readonly InputAction m_player_jump;
    private readonly InputAction m_player_camera;
    private readonly InputAction m_player_slide;
    private readonly InputAction m_player_Grapple;
    private readonly InputAction m_player_GrappleHold;
    public struct PlayerActions
    {
        private @Controller m_Wrapper;
        public PlayerActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @movement => m_Wrapper.m_player_movement;
        public InputAction @dash => m_Wrapper.m_player_dash;
        public InputAction @jump => m_Wrapper.m_player_jump;
        public InputAction @camera => m_Wrapper.m_player_camera;
        public InputAction @slide => m_Wrapper.m_player_slide;
        public InputAction @Grapple => m_Wrapper.m_player_Grapple;
        public InputAction @GrappleHold => m_Wrapper.m_player_GrappleHold;
        public InputActionMap Get() { return m_Wrapper.m_player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @movement.started += instance.OnMovement;
            @movement.performed += instance.OnMovement;
            @movement.canceled += instance.OnMovement;
            @dash.started += instance.OnDash;
            @dash.performed += instance.OnDash;
            @dash.canceled += instance.OnDash;
            @jump.started += instance.OnJump;
            @jump.performed += instance.OnJump;
            @jump.canceled += instance.OnJump;
            @camera.started += instance.OnCamera;
            @camera.performed += instance.OnCamera;
            @camera.canceled += instance.OnCamera;
            @slide.started += instance.OnSlide;
            @slide.performed += instance.OnSlide;
            @slide.canceled += instance.OnSlide;
            @Grapple.started += instance.OnGrapple;
            @Grapple.performed += instance.OnGrapple;
            @Grapple.canceled += instance.OnGrapple;
            @GrappleHold.started += instance.OnGrappleHold;
            @GrappleHold.performed += instance.OnGrappleHold;
            @GrappleHold.canceled += instance.OnGrappleHold;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @movement.started -= instance.OnMovement;
            @movement.performed -= instance.OnMovement;
            @movement.canceled -= instance.OnMovement;
            @dash.started -= instance.OnDash;
            @dash.performed -= instance.OnDash;
            @dash.canceled -= instance.OnDash;
            @jump.started -= instance.OnJump;
            @jump.performed -= instance.OnJump;
            @jump.canceled -= instance.OnJump;
            @camera.started -= instance.OnCamera;
            @camera.performed -= instance.OnCamera;
            @camera.canceled -= instance.OnCamera;
            @slide.started -= instance.OnSlide;
            @slide.performed -= instance.OnSlide;
            @slide.canceled -= instance.OnSlide;
            @Grapple.started -= instance.OnGrapple;
            @Grapple.performed -= instance.OnGrapple;
            @Grapple.canceled -= instance.OnGrapple;
            @GrappleHold.started -= instance.OnGrappleHold;
            @GrappleHold.performed -= instance.OnGrappleHold;
            @GrappleHold.canceled -= instance.OnGrappleHold;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
        void OnGrapple(InputAction.CallbackContext context);
        void OnGrappleHold(InputAction.CallbackContext context);
    }
}