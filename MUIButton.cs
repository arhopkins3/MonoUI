using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoUI
{
    public class ButtonTheme
    {
        public Texture2D Left;
        public Texture2D Centre;
        public Texture2D Right;
    }

    public class ButtonThemePack
    {
        public ButtonTheme Hover;
        public ButtonTheme Pressed;
        public ButtonTheme Disabled;
        public ButtonTheme Default;

        public ButtonThemePack()
        {
            Hover = new ButtonTheme();
            Pressed = new ButtonTheme();
            Disabled = new ButtonTheme();
            Default = new ButtonTheme();
        }
    }

    public enum ButtonType
    {
        ImageButton,
        ImageCaptionButton,
        CaptionButton
    }

    public enum ImagePlacement
    {
        Left,
        Right,
        Top,
        Bottom,
        Centre
    }

    public class MUIButton : MUIControl
    {
        public enum ButtonState
        {
            Disabled,
            Default,
            Hover,
            Pressed
        }

        public Color CaptionColour = Color.Black;
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value)
                {
                    // Setting to true
                    ChangeState(ButtonState.Default);
                    enabled = true;
                }
                else
                {
                    // Setting to false
                    ChangeState(ButtonState.Default);
                    enabled = false;
                }
            }
        }
        private bool mousePressedWhileInside;
        private bool wasMouseInsideLastFrame;
        private SpriteFont spriteFont;
        private FontRenderer fontRenderer;
        private ButtonThemePack theme;
        private ButtonState state;
        public ButtonState State
        {
            get { return state; }
            set { ChangeState(value); }
        }

        private ButtonTheme currentTheme;
        private Vector2 leftTexturePosition;
        private Vector2 rightTexturePosition;
        private Vector2 centreTextureStartPosition;
        private Vector2 imageTexturePosition;

        private Texture2D buttonImage;

        public bool ScaleToFit = true;
        private Vector2 captionPosition;

        public delegate void ButtonEvent(MUIButton sender);

        public ButtonType ButtonType;
        public ImagePlacement ImagePlacement;

        private string caption;

        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                // Recalculate Position for new string

                if (ScaleToFit) RecalculatePositions();

            }
        }


        public event ButtonEvent Pressed;
        public event ButtonEvent MouseEnter;
        public event ButtonEvent MouseLeave;
        public event ButtonEvent MouseDown;
        public event ButtonEvent MouseUp;

        private void RecalculatePositions()
        {

            var captionSize = new Vector2(10, 10);
            
            if (caption != null)
            {
                captionSize = fontRenderer.MeasureString(caption);
            }

            if (currentTheme != null && currentTheme.Left != null)
            {
                // Only do this once the textures are set
                                

           
                switch (ButtonType)
                {
                    case ButtonType.CaptionButton:
                        {
                            if (ScaleToFit)
                            {
                                Width = (int)captionSize.X + (8 * currentTheme.Left.Width);
                                Height = (int) (currentTheme.Left.Height*1.5f);
                            }
                            captionPosition = Position + new Vector2((Width * 0.5f) - (captionSize.X * 0.5f), (Height * 0.5f) - 5f - (captionSize.Y * 0.5f));    
                        }
                        break;
                    case ButtonType.ImageButton:
                        {
                            if (ScaleToFit)
                            {
                                Width = buttonImage.Width + (8*currentTheme.Left.Width);
                                Height = (int) (buttonImage.Height*1.5f);
                            }
                            break;
                        }
                    case ButtonType.ImageCaptionButton:
                        {
                            if (ScaleToFit)
                            {
                                
                                switch (ImagePlacement)
                                {
                                    case ImagePlacement.Right:
                                        {
                                            break;
                                        }
                                    case ImagePlacement.Centre:
                                        {
                                            // Image is at the centre, must be overlaying the text caption
                                            captionPosition = Position + new Vector2((Width * 0.5f) - (captionSize.X * 0.5f), (Height * 0.5f) - (captionSize.Y * 0.5f));    
                                            break;
                                        }
                                    case ImagePlacement.Left:
                                        {
                                            // Image is on the right, must move the caption to the left
                                            Width = buttonImage.Width + (int)captionSize.X + (8 * currentTheme.Left.Width);
                                            Height = (int)(buttonImage.Height * 1.5f);
                                            captionPosition = Position + new Vector2(leftTexturePosition.X + (currentTheme.Left.Width * 4), leftTexturePosition.Y + ((currentTheme.Left.Height * 0.5f) - (captionSize.Y * 0.5f)));
                                            imageTexturePosition = Position +
                                                                   new Vector2(leftTexturePosition.X,
                                                                               leftTexturePosition.Y);
                                            break;
                                        }
                                    case ImagePlacement.Bottom:
                                        {
                                            break;
                                        }
                                    case ImagePlacement.Top:
                                        {
                                            break;
                                        }

                                }

                                    
                                    
                            }
                        }
                        break;
                }

                leftTexturePosition = new Vector2(Position.X, Position.Y);
                rightTexturePosition = new Vector2(Position.X + (Width - currentTheme.Right.Width), Position.Y);
                centreTextureStartPosition = new Vector2(leftTexturePosition.X + currentTheme.Left.Width, Position.Y);

                    
                    
                }


        }

        private void HandleControlMove(Object sender, EventArgs e)
        {
            RecalculatePositions();
        }

        private void HandleParentChange(Object sender, EventArgs e)
        {
            if (Parent != null)
            {
                Parent.ControlMoved += HandleControlMove;
            }
        }

        public MUIButton(string caption, int x, int y, FontRenderer fr, ButtonType type = ButtonType.CaptionButton, ImagePlacement imgPlacement = ImagePlacement.Left, Texture2D image = null)
            : base(x, y)
        {
            ButtonType = type;
            ImagePlacement = imgPlacement;
            buttonImage = image;
            theme = new ButtonThemePack();
            fontRenderer = fr;
            Caption = caption;
            ControlMoved += HandleControlMove;
            ParentChanged += HandleParentChange;

            theme.Default.Left = TextureLoader.LoadTexture("button_default_left.png");
            theme.Default.Right = TextureLoader.LoadTexture("button_default_right.png");
            theme.Default.Centre = TextureLoader.LoadTexture("button_default_centre.png");

            theme.Hover.Left = TextureLoader.LoadTexture("button_hover_left.png");
            theme.Hover.Right = TextureLoader.LoadTexture("button_hover_right.png");
            theme.Hover.Centre = TextureLoader.LoadTexture("button_hover_centre.png");

            theme.Pressed.Left = TextureLoader.LoadTexture("button_pressed_left.png");
            theme.Pressed.Right = TextureLoader.LoadTexture("button_pressed_right.png");
            theme.Pressed.Centre = TextureLoader.LoadTexture("button_pressed_centre.png");

            currentTheme = theme.Default;
            //theme.Disabled.Left = TextureLoader.LoadTexture("button_disabled_left.png");
            //theme.Disabled.Right = TextureLoader.LoadTexture("button_disabled_right.png");
            //theme.Disabled.Centre = TextureLoader.LoadTexture("button_disabled_centre.png");

            ChangeState(ButtonState.Pressed);
        }

        private void ChangeState(ButtonState newState)
        {
            if (newState == state) return;

            switch (newState)
            {
                case ButtonState.Default:
                    currentTheme = theme.Default;
                    break;
                case ButtonState.Pressed:
                    currentTheme = theme.Pressed;
                    break;
                case ButtonState.Disabled:
                    currentTheme = theme.Disabled;
                    break;
                case ButtonState.Hover:
                    currentTheme = theme.Hover;
                    break;
            }
            RecalculatePositions();
            state = newState;
        }

        public override void Update(GameTime gameTime)
        {
            RecalculatePositions();
            base.Update(gameTime);
            if (Enabled == false) return; // Don't process button update if button is disabled

            var mousePoint = new Point(InputState.MouseState.X, InputState.MouseState.Y);

            // First check if the mouse is in the button or not
            var mouseInsideNow = Bounds.Contains(mousePoint);

            if (mouseInsideNow && wasMouseInsideLastFrame)
            {
                // Mouse is inside now and was last frame
                // MOUSE CONTINUES TO BE INSIDE
                
                wasMouseInsideLastFrame = true; // Reset

                // Only check for mouse down here. Can't enter the frame with mouse down and receive button press

                if (InputState.WasLMBPressed())
                {
                    // The LMB is pressed while inside
                    if (MouseDown != null) MouseDown(this);
                    ChangeState(ButtonState.Pressed);
                    mousePressedWhileInside = true;
                }
                else if (InputState.WasLMBReleased())
                {
                    // The LMB was released while inside
                    if (MouseUp != null) MouseUp(this);
                    if (mousePressedWhileInside)
                    {
                        // The mouse was released after being pressed down inside, must be button press
                        if (Pressed != null) Pressed(this);
                        mousePressedWhileInside = false;
                        ChangeState(ButtonState.Hover);
                    }
                }
            }
            else if (mouseInsideNow && !wasMouseInsideLastFrame)
            {
                // Mouse is inside now but wasn't last frame
                // MOUSE JUST ENTERED THE BUTTON
                wasMouseInsideLastFrame = true;

                if (MouseEnter != null) MouseEnter(this);

                ChangeState(mousePressedWhileInside ? ButtonState.Pressed : ButtonState.Hover);
            }
            else if (!mouseInsideNow && wasMouseInsideLastFrame)
            {
                // Mouse is not inside now but was last frame
                // MOUSE JUST LEFT THE BUTTON
                wasMouseInsideLastFrame = false; // Reset;
                if (MouseLeave != null) MouseLeave(this);
            }
            else if (!mouseInsideNow && !wasMouseInsideLastFrame)
            {
                // Mouse is not inside now but wasnt last frame either
                // MOUSE IS NOT OVER THE BUTTON
                if (InputState.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    // LMB was released outside the button
                    mousePressedWhileInside = false;
                    ChangeState(ButtonState.Default);
                }
            }
           
        }
        
        public override void Draw(SpriteBatch sb)
        {
            if (Visible)
            {
                Color currentColour = Color.Red;
                currentColour = enabled ? Color.White : new Color(0.1f, 0.1f, 0.1f); // Set colour to White if enabled, nearly black if disabled
                
                sb.Draw(currentTheme.Centre, Bounds, currentColour);
                sb.Draw(currentTheme.Left,
                        new Rectangle((int) leftTexturePosition.X, (int) leftTexturePosition.Y, currentTheme.Left.Width,
                                      Bounds.Height), currentColour);
                sb.Draw(currentTheme.Right,
                        new Rectangle((int) rightTexturePosition.X, (int) rightTexturePosition.Y,
                                      currentTheme.Right.Width, Bounds.Height), currentColour);
                if (ButtonType == ButtonType.ImageButton
                    || ButtonType == ButtonType.ImageCaptionButton)
                    sb.Draw(buttonImage, imageTexturePosition, currentColour);
                if (ButtonType == ButtonType.CaptionButton
                    || ButtonType == ButtonType.ImageCaptionButton)
                    fontRenderer.DrawText(sb, (int) captionPosition.X, (int) captionPosition.Y, caption, CaptionColour);
            }
            base.Draw(sb); // Base Draw last to draw debug over control
        }
        





    }
}
