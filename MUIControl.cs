using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoUI
{


    public class Animation
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public bool Finished;
        public TimeSpan Length;
        public TimeSpan CurrentTime;
        public event EventHandler Ended;
        public event EventHandler Started;

        public Animation()
        {
            Finished = false;
            StartPosition = Vector2.Zero;
            EndPosition = Vector2.Zero;
            Length = TimeSpan.Zero;
            CurrentTime = TimeSpan.Zero;
        }

        public Vector2 GetNewPosition(GameTime gameTime)
        {
            if (CurrentTime < Length)
            {
                CurrentTime += gameTime.ElapsedGameTime;

                if (CurrentTime >= Length)
                {
                    CurrentTime = Length;
                }
            }

            float lerpAmount = (float)CurrentTime.Ticks / Length.Ticks;
            if (lerpAmount >= 1.0f)
            {
                // The animation has finished lerping
                if (Ended != null)
                {
                    // If a delegate is attached to ENDED, let that handle the finish state
                    Ended(this, EventArgs.Empty);
                }
                else Finished = true;
            }
            return Vector2.SmoothStep(StartPosition, EndPosition, lerpAmount);
        }

    }



    /// <summary>
    /// Represents the base control object for the UI system
    /// </summary>
    public class MUIControl : IComparer<MUIControl>
    {
        public delegate void DragDropEventHandler(MUIControl droppedControl);

        public static List<MUIControl> Controls;


        public bool AcceptsDragDrop = true;
        public bool DragDropEnabled = false;
        private bool dragInProgress = false;
        private Vector2 dragOffset;
        public event DragDropEventHandler ControlDropReceived;
        public event DragDropEventHandler ControlDragStarted;
        public event DragDropEventHandler ControlDragStopped;
        public event EventHandler ControlResized;
        public event EventHandler ControlMoved;
        public event EventHandler ParentChanged;

        public Stack<Animation> Animations;
        public int Tag = 0;
        public bool AllowAnimationStacking = false;
        public event EventHandler AnimationsComplete;
        private Vector2 bounceFromPos;
        private Vector2 bounceToPos;
        private bool isBouncing = false;
        private float bounceTime = 0;
        private float bounceOffset = 0;

        public Rectangle Bounds
        {
            get {
                    return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height); 
                }
        }

        public DebugRect BoundsView; // A graphics view of the bounding area for the control
        private MUIControl parent;

        public MUIControl Parent
        {
            get { return parent;  }
            set { parent = value; if (ParentChanged != null) ParentChanged(this, EventArgs.Empty); }
        }

        // The parent of the control
        public bool Visible;
        public bool DebugVisible;
        public void MoveTo(int x, int y)
        {
            position = new Vector2(x, y);
            if (ControlMoved != null) ControlMoved(this, EventArgs.Empty);
        }

        /// <summary>
        /// Animated the control position from one point to anoter
        /// </summary>
        /// <param name="pos">Where to LERP to</param>
        /// <param name="seconds">Time in Seconds to take</param>
        public void LerpTo(Vector2 pos, float seconds)
        {
            if (AllowAnimationStacking || Animations.Count < 1) // Dont add another animation if one already exists or if stacking is disabled
            {
                var newAnimation = new Animation();

                newAnimation.CurrentTime = TimeSpan.Zero;
                newAnimation.StartPosition = Position;
                newAnimation.EndPosition = pos;
                newAnimation.Length = TimeSpan.FromSeconds(seconds);

                Animations.Push(newAnimation);
            }   
        }

        public void LerpBetween(Vector2 from, Vector2 to, float seconds)
        {
            if (AllowAnimationStacking || Animations.Count < 1)
            {
                var newAnimation = new Animation
                                       {
                                           CurrentTime = TimeSpan.Zero,
                                           StartPosition = @from,
                                           EndPosition = to,
                                           Length = TimeSpan.FromSeconds(seconds)
                                       };

                Animations.Push(newAnimation);
            }
        }

        /// <summary>
        /// Method to bounce (smooth step) between two positions [overrides all other animations currently]
        /// </summary>
        /// <param name="from">Starting position</param>
        /// <param name="to">End position</param>
        /// <param name="seconds">Time span for the animation</param>
        public void BounceForeverBetween(Vector2 from, Vector2 to, float seconds, float offset = 0f)
        {
            Animations.Clear(); // Clear out all old animations
            isBouncing = true;
            bounceFromPos = from;
            bounceOffset = offset;
            bounceToPos = to;
            bounceTime = seconds;

            var newAnimation = new Animation
                                   {
                                       CurrentTime = TimeSpan.Zero + TimeSpan.FromMilliseconds(offset),
                                       StartPosition = bounceFromPos,
                                       EndPosition = bounceToPos,
                                       Length = TimeSpan.FromSeconds(bounceTime)
                                   };

            Animations.Push(newAnimation);

            newAnimation.Ended += NewAnimationOnEnded;
        }

        private void NewAnimationOnEnded(object sender, EventArgs eventArgs)
        {
            Vector2 temp;
            temp = bounceFromPos;
            bounceFromPos = bounceToPos;
            bounceToPos = temp;
            BounceForeverBetween(bounceFromPos, bounceToPos, bounceTime, bounceOffset );
        }

        public void StopAnimating()
        {
            Animations.Clear();
        }

        /// <summary>
        /// Moves the control by a delta amount
        /// </summary>
        /// <param name="x">X delta</param>
        /// <param name="y">Y delta</param>
        public void MoveBy(int x, int y)
        {
            position += new Vector2(x, y);
            if (ControlMoved != null) ControlMoved(this, EventArgs.Empty);
        }
        protected Vector2 position;
        
        private static int guidCount;

        private int guid;

        private int height;

        private int width;


        /// <summary>
        /// Constructor for the Control class
        /// </summary>
        /// <param name="x">X coordinate for the control</param>
        /// <param name="y">Y coordinate for the control</param>
        /// <param name="w">Width of the control</param>
        /// <param name="h">Heiht of the control</param>
        public MUIControl(int x, int y, int w = 100, int h = 100)
        {
            if (Controls == null) Controls = new List<MUIControl>();
            guid = guidCount++;
            Width = w;
            Height = h;
            position = new Vector2(x, y);
            Parent = null;
            Visible = true;
            DrawOrder = 100; // Default DrawOrder value.
            var r = new Random(DateTime.Now.Millisecond);
            Animations = new Stack<Animation>();
            dragOffset = Vector2.Zero;
            var red = (float)r.NextDouble();
            var green = (float)r.NextDouble();
            var blue = (float)r.NextDouble();
            
            BoundsView = new DebugRect(new Color(red, green, blue, 0.2f)) {Parent = this};

            Controls.Add(this);
        }

        private int drawOrder;
        // Controls when the control is drawn
        public int DrawOrder            // Controls when the control is drawn
        {
            get { return drawOrder; }
            set { drawOrder = (int)MathHelper.Clamp(value, 0, 999); }
        }

        public int GUID// The unique GUID for the control
        {
            get { return guid; }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (ControlResized != null) ControlResized(this, EventArgs.Empty);
                height = value;
            }
        }

        public Vector2 Position
        {
            get {
                if (Parent == null) return new Vector2(position.X, (int)position.Y);
                else return new Vector2(position.X, (int)position.Y) + Parent.Position;
            }
        }// The position of the control (relative to parent)
        public int Width
        {
            get { return width; }
            set
            {
                if (ControlResized != null) ControlResized(this, EventArgs.Empty);
                width = value;
            }
        }

        int IComparer<MUIControl>.Compare(MUIControl x, MUIControl y)
        {
            if (x == null || y == null)
                throw new InvalidOperationException("Control being compared to a null value");

            if (x.DrawOrder < y.DrawOrder) return -1;
            if (x.DrawOrder > y.DrawOrder) return 1;
            return 0;
        }

        public static void UpdateAll(GameTime gameTime)
        {
            if (Controls == null) Controls = new List<MUIControl>();
            foreach (var control in Controls)
            {
                control.Update(gameTime);
            }
        }

        public static void DrawAll(SpriteBatch spriteBatch)
        {
            if (Controls == null) Controls = new List<MUIControl>();
            foreach (var control in Controls)
            {
                if (control != null) control.Draw(spriteBatch);
            }
        }


        /// <summary>
        /// Update the control and update animation positions
        /// </summary>
        /// <param name="gameTime">The GameTime parameter</param>
        public virtual void Update(GameTime gameTime)
        {
            var mousePoint = new Point(InputState.MouseState.X, InputState.MouseState.Y);

            if (DragDropEnabled)
            {
                if (dragInProgress && Bounds.Contains(mousePoint) &&  InputState.MouseState.LeftButton == ButtonState.Pressed)
                {
                    position = new Vector2(mousePoint.X + dragOffset.X, mousePoint.Y + dragOffset.Y);
                }
                
                if (!dragInProgress && Bounds.Contains(mousePoint) && InputState.MouseState.LeftButton == ButtonState.Pressed)
                {
                    dragInProgress = true;
                    if (ControlDragStarted != null) ControlDragStarted(this);
                    dragOffset = new Vector2(Position.X - mousePoint.X, Position.Y - mousePoint.Y);
                }
                if (dragInProgress && InputState.MouseState.LeftButton == ButtonState.Released)
                {
                    dragInProgress = false;
                    if (ControlDragStopped != null) ControlDragStopped(this);
                    dragOffset = Vector2.Zero;
                }
                
            }

            if (Animations.Count > 0) // If there any animations pending
            {
                Animation currentAnimation = Animations.Peek(); // Take the topmost animation

                Vector2 newPosition = currentAnimation.GetNewPosition(gameTime); // Calculate the new position from the animation 
                
                MoveTo((int)newPosition.X, (int)newPosition.Y); // Move the control to the new position
                    
                if (currentAnimation.Finished) // If the animation has finished, remove it
                {
                    Animations.Pop();

                    if (Animations.Count < 1) // If the animation stack is now empty, fire the AnimationsComplete evet
                    {
                        if (AnimationsComplete != null) AnimationsComplete(this, EventArgs.Empty);
                    }
                }    
            }
            
            if (InputState.WasKeyPressed(Keys.F1))
            {
                BoundsView.Visible = !BoundsView.Visible;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            BoundsView.Draw(sb);
        }
    }
}
