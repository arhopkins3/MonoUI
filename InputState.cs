using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoUI
{
    public class InputState
    {
        private static KeyboardState keyboardState;
        private static KeyboardState oldKeyboardState;

        public InputState()
        {
        }

        public static KeyboardState KeyboardState
        {
            get
            {
                return keyboardState;
            }
        }

        private static MouseState mouseState;
        private static MouseState oldMouseState;

        public static Vector2 MouseDelta
        {
            get { var delta = new Vector2(mouseState.X - oldMouseState.X, mouseState.Y - oldMouseState.Y);
                return delta;
            }
        }

        public static MouseState MouseState
        {
            get { return mouseState; }
        }

        public static bool WasKeyPressed(Keys key)
        {
            return oldKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
        }

        public static bool WasLMBPressed()
        {
            return oldMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed;
        }
        public static bool WasLMBReleased()
        {
            return oldMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released;
        }

        public void Update(GameTime gameTime)
        {
            oldKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();
        }
    }
}
