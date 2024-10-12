using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// A class for helping out with input-related tasks, such as checking if a key or mouse button has been pressed.
/// </summary>
class InputHelper
{
    // The current and previous mouse/keyboard states.
    MouseState currentMouseState, previousMouseState;
    KeyboardState currentKeyboardState, previousKeyboardState;

    public void Update(GameTime gameTime)
    {
        previousMouseState = currentMouseState;
        previousKeyboardState = currentKeyboardState;
        currentMouseState = Mouse.GetState();
        currentKeyboardState = Keyboard.GetState();
    }
    public Vector2 MousePosition
    {
        get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
    }

    public bool MouseLeftButtonPressed()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
    }

    /// <summary>
    /// Returns whether or not a given keyboard key has just been pressed.
    /// </summary>
    /// <param name="k">The key to check.</param>
    /// <returns>true if the given key has just been pressed in this frame; false otherwise.</returns>
    public bool KeyPressed(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyUp(k);
    }

    /// <summary>
    /// Returns whether or not a given keyboard key is currently being held down.
    /// </summary>
    /// <param name="k">The key to check.</param>
    /// <returns>true if the given key is being held down; false otherwise.</returns>
    public bool KeyDown(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k);
    }
}