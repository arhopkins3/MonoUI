using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoUI
{
    public class MUITabPanel : MUIPanel
    {
        public delegate void TabSelectionChangedEvent(MUIToggleButton tb);

        public event TabSelectionChangedEvent  TabSelectionChanged;

        public MUITabPanel(int x, int y, int w, int h) : base(x, y, w, h)
        {
        }

        public override void AddControl(MUIControl control)
        {
            var tb = control as MUIToggleButton;
            tb.ToggleChange += ToggleButtonPressed;
            base.AddControl(tb);
        }

        /// <summary>
        /// Method called by ToggleButtonPressed event. Used to change selection
        /// </summary>
        /// <param name="tb">Which toggle button was just pressed</param>
        private void ToggleButtonPressed(MUIToggleButton tb)
        {
            if (tb.State == MUIToggleButton.ButtonState.Pressed)
            {
                // The control was allowed to be pressed, so just select that control in panel
                SelectTab(tb);
                
            }
        }

        public void SelectTab(MUIToggleButton button)
        {
            foreach (MUIToggleButton tb in Children.OfType<MUIToggleButton>().Select(control => control as MUIToggleButton))
            {
                if (tb.Equals(button))
                {
                    //    tb.ToggleChange -= ToggleButtonPressed;
                    //    tb.Press(); // Press the selected button down
                    //    tb.ToggleChange += ToggleButtonPressed;
                }
                else
                {
                    tb.ToggleChange -= ToggleButtonPressed;
                    tb.Depress(); // Depress all others
                    tb.ToggleChange += ToggleButtonPressed;
                }
            }
            if (TabSelectionChanged != null) TabSelectionChanged(button);
        }
    }
}
