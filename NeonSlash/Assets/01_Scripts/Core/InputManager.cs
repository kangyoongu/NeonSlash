using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : SingleTon<InputManager>
{
    OverallInput _inputs;

    public UnityEvent<InputAction.CallbackContext> OnClickEsc;
    public Action<Vector2> OnMove;
    public Action<Vector2> OnMousePos;

    public Action OnClickDash;
    public Action OnClickSkill;
    public Action OnClickF;
    [HideInInspector] public bool isMouseDown = false;
    private void Awake()
    {
        _inputs = new OverallInput();
        _inputs.Enable();
        _inputs.Overall.Escape.performed += Excape;
        _inputs.Overall.SpawnBoss.started += EnterF;

        _inputs.Player.MouseClick.started += (obj) => isMouseDown = true;
        _inputs.Player.MouseClick.canceled += (obj) => isMouseDown = false;

        _inputs.Player.Dash.started += EnterSpace;
        _inputs.Player.Skill1.started += EnterE;
    }
    private void EnterE(InputAction.CallbackContext obj)
    {
        OnClickSkill?.Invoke();
    }
    private void EnterF(InputAction.CallbackContext obj)
    {
        OnClickF?.Invoke();
    }
    private void EnterSpace(InputAction.CallbackContext obj)
    {
        OnClickDash?.Invoke();
    }
    private void Update()
    {
        OnMove?.Invoke(_inputs.Player.Move.ReadValue<Vector2>());
        OnMousePos?.Invoke(_inputs.Player.MousePosition.ReadValue<Vector2>());
    }

    private void Excape(InputAction.CallbackContext obj)
    {
        OnClickEsc?.Invoke(obj);
    }
}
