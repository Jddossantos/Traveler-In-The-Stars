using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class InputManager : Singleton<InputManager>
{
    #region Events
    //define events for when the touch starts
    public static event Action<Vector2, float> OnStartTouch;
    public static event Action<Vector2, float> OnEndTouch;
    #endregion

    //instance of the generated inputs class to handle input actions
    private Inputs playerInput;

    protected override void Awake()
    {
        base.Awake();

        //initiallizing the inputs instance
        playerInput = new Inputs();
        //Debug.Log("InputManager: Awake - Inputs initialized.");
    }

    void OnEnable()
    {
        //enables the inputs and subscribes to the touch events
        playerInput.Enable();
        playerInput.Touch.PrimaryContact.started += OnPrimaryContactStarted;
        playerInput.Touch.PrimaryContact.canceled += OnPrimaryContactCanceled;
        //Debug.Log("InputManager: OnEnable - Inputs eneabled and events subscribed.");
    }

    void OnDisable()
    {
        //disables the inputs and unsubsribes to the touch events
        playerInput.Disable();
        playerInput.Touch.PrimaryContact.started -= OnPrimaryContactStarted;
        playerInput.Touch.PrimaryContact.canceled -= OnPrimaryContactCanceled;
        //Debug.Log("InputManager: OnDisable - Inputs Disabled and events unsubscribed.");
    }

    //called when the primary contact starts
    private void OnPrimaryContactStarted(InputAction.CallbackContext context)
    {
        //getting the position of the touch and triggers the OnScreenTouch event
        Vector2 position = PrimaryPosition();
        //Debug.Log($"InputManager : OnPrimaryContactStarted - Position: {position}, Time: {context.time}");
        OnStartTouch?.Invoke(position, (float)context.time);
    }

    //called when the primary contact ends
    private void OnPrimaryContactCanceled(InputAction.CallbackContext context)
    {
        //getting the position of the touch and triggers the OnEndTouch event
        Vector2 position = PrimaryPosition();
        //Debug.Log($"InputManager: OnPrimaryContactCancelled - Position: {position}, Time: {context.time}");
        OnEndTouch?.Invoke(position, (float)context.time);
    }

    //gets the current touch position and converts it to world coordinates
    public Vector2 PrimaryPosition()
    {
        //reading the screen position frm the input and converts it to world coordinates
        Vector2 position = playerInput.Touch.PrimaryPosition.ReadValue<Vector2>();
        //Debug.Log($"InputManager: PrimaryPosition - Screen Position {position}");
        Vector3 worldPosition = ScreenToWorld(position);
        //Debug.Log($"InputManager: PrimaryPosition - World Position {position}");
        return worldPosition;
    }

    //converts a screen position to a world position
    private Vector3 ScreenToWorld(Vector3 pos)
    {
        //setting the z-coordinates to the near clipping plane to get the correct depth
        pos.z = Camera.main.nearClipPlane;

        //converts the screen position to world position
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
