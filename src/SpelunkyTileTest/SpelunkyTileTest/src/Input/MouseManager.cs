using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Input
{
    /// <summary>
    /// Represents a mouse button
    /// </summary>
    public enum MouseButton
    {
        LEFT,
        RIGHT,
        MIDDLE
    }

    /*TODO:
     * Directly hook into mouse inputs
     * Add events to hook into (find efficient way to do provide a hook for every button)
     * Represent mouse4 and mouse5 and such?
     */

    static class MouseManager
    {
        //Keep track of states for comparison
        static MouseState oldState;
        static MouseState State;

        /// <summary>
        /// Update the mouse manager's inputs
        /// </summary>
        public static void Update()
        {
            oldState = State;
            State = Mouse.GetState();
        }

        /// <summary>
        /// Check if button was pressed since last update
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if button just pressed</returns>
        public static bool ButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LEFT:
                    return (State.LeftButton   == ButtonState.Pressed && oldState.LeftButton   == ButtonState.Released);
                case MouseButton.RIGHT:
                    return (State.RightButton  == ButtonState.Pressed && oldState.RightButton  == ButtonState.Released);
                case MouseButton.MIDDLE:
                    return (State.MiddleButton == ButtonState.Pressed && oldState.MiddleButton == ButtonState.Released);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if button was released since last update
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if button just released</returns>
        public static bool ButtonReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LEFT:
                    return (State.LeftButton   == ButtonState.Released && oldState.LeftButton   == ButtonState.Pressed);
                case MouseButton.RIGHT:
                    return (State.RightButton  == ButtonState.Released && oldState.RightButton  == ButtonState.Pressed);
                case MouseButton.MIDDLE:
                    return (State.MiddleButton == ButtonState.Released && oldState.MiddleButton == ButtonState.Pressed);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if button currently down
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if button down</returns>
        public static bool ButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LEFT:
                    return (State.LeftButton   == ButtonState.Pressed);
                case MouseButton.RIGHT:
                    return (State.RightButton  == ButtonState.Pressed);
                case MouseButton.MIDDLE:
                    return (State.MiddleButton == ButtonState.Pressed);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Get the position of the mouse cursor (relative to window)
        /// </summary>
        /// <param name="X">Output X position of the cursor</param>
        /// <param name="Y">Output Y position of the cursor</param>
        public static void GetPosition(out int X, out int Y)
        {
            X = State.X;
            Y = State.Y;
        }
    }
}
