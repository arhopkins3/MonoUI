using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoUI
{
    /// <summary>
    /// Represents a rectangular area control that may contain one or more
    /// child controls
    /// </summary>
    public class MUIPanel : MUIControl
    {
        public List<MUIControl> Children; // The child controls of the panel, sorted by draw order and UID

        public event EventHandler ChildControlAdded;    // Event fired when a child control is added
        public event EventHandler ChildControlRemoved;  // Event fire when a child control is removed

        public MUIPanel(int x, int y, int w = 100, int h = 100) : base(x, y, w, h)
        {
            Children = new List<MUIControl>();
        }
        
        public void Enable()
        {
            foreach (var tb in Children.Select(button => button as MUIToggleButton))
            {
                tb.Enabled = true;
            }
        }

        public void Disable()
        {
            foreach (var tb in Children.Select(button => button as MUIToggleButton))
            {
                tb.Enabled = false;
            }
        }

        /// <summary>
        /// Adds a control to the panels collection of children
        /// </summary>
        /// <param name="control">The control to add</param>
        public virtual void AddControl(MUIControl control)
        {
            control.Parent = this;                           // Set the parent of the control to this panel
            if (ChildControlAdded != null) 
                ChildControlAdded(this, EventArgs.Empty);       // Fire the child control added event
            Children.Add(control);                           // Add the control to the children collection
            //Children.Sort();                                    // Sort the new control into the right position based on the DrawOrder property
        }

        /// <summary>
        /// Removes a control from the panel's collection of children
        /// </summary>
        /// <param name="control">The control to remove</param>
        public void RemoveControl(MUIControl control)
        {
            if (Children.Contains(control)) Children.Remove(control);   // Remove the control from the list
            if (ChildControlRemoved != null) ChildControlRemoved(this, EventArgs.Empty); // Fire the child control removed event
        }

        /// <summary>
        /// Updates the control and the children
        /// </summary>
        /// <param name="gameTime">The GameTime parameter</param>
        public override void Update(GameTime gameTime)
        {
            foreach (MUIControl control in Children)
            {
                if (control != null) control.Update(gameTime);
            }
            base.Update(gameTime); // Update the base class
        }

        /// <summary>
        /// Draws the control and all child controls
        /// </summary>
        /// <param name="sb">The SpriteBatch object to draw using</param>
        public override void Draw(SpriteBatch sb)
        {

            base.Draw(sb); // Call the base class Draw method first (for DebugRect)
            foreach (MUIControl control in Children)
            {
                if (control != null) control.Draw(sb); // Iterate through all child controls and Draw them all
            }
        }

        
    }
}
