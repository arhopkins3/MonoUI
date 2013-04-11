using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoUI
{
    public class MUIToggleButton : MUIControl
    {
        public enum ButtonState
        {
            Disabled,
            Default,
            Hover,
            Pressed
        }
        private FontRenderer fontRenderer;

        public ButtonState State;

        private Vector2 captionPosition;

        public delegate void ToggleButtonEvent(MUIToggleButton sender);

        public string Caption { get { return caption; } set { caption = value; RecalculatePositions(); } }
        private string caption;
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value)
                {
                    // Setting to true
                    enabled = true;
                }
                else
                {
                    // Setting to false
                    enabled = false;
                }
            }
        }
        public event ToggleButtonEvent ToggleChange;
        public event ToggleButtonEvent ToggleUp;
        public event ToggleButtonEvent ToggleDown;
        public Color CaptionColour;
        public bool AllowSelfDepress;
        private Texture2D currentTexture;
        public Texture2D DefaultTexture;
        public Texture2D HoverTexture;
        public Texture2D SelectedTexture;
        public Texture2D DisabledTexture;

        private void RecalculatePositions()
        {
            Vector2 captionSize = new Vector2(10, 10);

            if (caption != null)
            {
                captionSize = fontRenderer.MeasureString(caption);
            }
            captionPosition = Position + new Vector2((Width / 2) - (captionSize.X / 2), (Height / 2) - 5f - (captionSize.Y / 2));

            
            //captionPosition = Position + new Vector2(captionSize.X / 2, captionSize.Y / 2) + new Vector2(Width / 2, Height / 2);
        }

        private void HandleControlMove(Object sender, EventArgs e)
        {
            RecalculatePositions();
        }

        private void HandleParentChange(Object sender, EventArgs e)
        {
            if (Parent != null)
            {
                RecalculatePositions();
                Parent.ControlMoved += HandleControlMove;
            }
        }

        public MUIToggleButton(string _caption, int x, int y, FontRenderer fr)
            : base(x, y)
        {
            CaptionColour = Color.Black;
            HoverTexture = TextureLoader.LoadTexture("toolbar_hover.png");
            SelectedTexture = TextureLoader.LoadTexture("toolbar_selected.png");
            DefaultTexture = TextureLoader.LoadTexture("toolbar_default.png");
            
            Width = HoverTexture.Width;
            Height = HoverTexture.Height;
            fontRenderer = fr;
            Caption = _caption;
            ControlMoved += HandleControlMove;
            ParentChanged += HandleParentChange;
            AllowSelfDepress = true;
 
   
            ChangeState(ButtonState.Default);
        }

        private void ChangeState(ButtonState newState)
        {
            if (newState == State) return;

            switch (newState)
            {
                case ButtonState.Default:
                    currentTexture = DefaultTexture;
                    break;
                case ButtonState.Pressed:
                    currentTexture = SelectedTexture;
                    break;
                case ButtonState.Disabled:
                    currentTexture = DisabledTexture;
                    break;
                case ButtonState.Hover:
                    currentTexture = HoverTexture;
                    break;
            }
            State = newState;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Enabled == false) return;

            var mousePoint = new Point(InputState.MouseState.X, InputState.MouseState.Y);

            if (Bounds.Contains(mousePoint) && InputState.WasLMBPressed() && State != ButtonState.Pressed)
            {
                Press();
            }
            else if (Bounds.Contains(mousePoint) && InputState.WasLMBPressed() && State == ButtonState.Pressed)
            {
                if (AllowSelfDepress) Depress();
            }
            else if (Bounds.Contains(mousePoint))
            {
                if (State != ButtonState.Pressed) ChangeState(ButtonState.Hover);
            }
            else
            {
                if (State != ButtonState.Pressed) ChangeState(ButtonState.Default);
            }
           
        }

        /// <summary>
        /// Method used to press a toggle button down
        /// </summary>
        public void Press()
        {
            ChangeState(ButtonState.Pressed);
            if (ToggleDown != null) ToggleDown(this);
            if (ToggleChange != null) ToggleChange(this);
        }

        /// <summary>
        /// Method used to toggle the button up (depress)
        /// </summary>
        public void Depress()
        {
            ChangeState(ButtonState.Hover);
            if (ToggleUp != null) ToggleUp(this);
            if (ToggleChange != null) ToggleChange(this);
        }

        public override void Draw(SpriteBatch sb)
        {
            Color currentColour;
            if (Caption == "Select")
            {
                Vector2 position = this.Position;
            }
            //if (Enabled) 
            currentColour = Color.White;
            //else currentColour = new Color(0.2f, 0.2f, 0.2f);

            sb.Draw(currentTexture, Bounds, currentColour);
//            sb.DrawString(spriteFont, Caption, captionPosition, Color.Black);
            RecalculatePositions();
            fontRenderer.DrawText(sb, (int)captionPosition.X, (int)captionPosition.Y, Caption, CaptionColour);
            base.Draw(sb); // Base Draw last to draw debug over control
        }






    }
}
