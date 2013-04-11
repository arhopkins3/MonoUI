using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoUI
{
    public class MUISelector : MUIPanel
    {

        private MUIButton btnInc;
        private MUIButton btnDec;
        
        public int MaximumValue = 100;
        public int MinimumValue = 1;

        public int CurrentValue = 50;
        public float Percentage
        {
            get { return CurrentValue/MaximumValue*100;  }
        }
        public event EventHandler ValueChanged;
        public bool LoopValues = true;
        public enum Orientation
        {
            Horizontal,
            Vertical,
            MAXIMUM
        }

        public MUISelector(FontRenderer fontRenderer, Orientation or, int size, int x, int y, int w = 100, int h = 100) : base(x, y, w, h)
        {
            switch (or)
            {
                case Orientation.Horizontal:
                    {
                        btnInc = new MUIButton("+", size, 0, fontRenderer) { ScaleToFit = false, Width = 30, Height = 30 };
                        btnDec = new MUIButton("-", 0, 0, fontRenderer) { ScaleToFit = false, Width = 30, Height = 30 };
                        break;
                    }
                case Orientation.Vertical:
                    {
                        btnInc = new MUIButton("+", 0, size, fontRenderer) { ScaleToFit = false, Width = 30, Height = 30 };
                        btnDec = new MUIButton("-", 0, 0, fontRenderer) { ScaleToFit = false, Width = 30, Height = 30 };
                        break;
                    }
            }

            btnDec.Pressed += Decrease;
            btnInc.Pressed += Increase;

            Increase(btnDec);
            Decrease(btnInc);
            base.AddControl(btnDec);
            base.AddControl(btnInc);
        }
        public void Increase()
        {
            Increase(btnInc);
        }
        public void Decrease()
        {
            Decrease(btnDec);
        }

        private void Decrease(MUIButton sender)
        {
            CurrentValue--;
            if (!LoopValues)
            {
                if (CurrentValue < MinimumValue) CurrentValue = MinimumValue;
            }
            else
            {
                if (CurrentValue < MinimumValue) CurrentValue = MaximumValue;
            }

            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
        }
        private void Increase(MUIButton sender)
        {
            CurrentValue++;
            if (!LoopValues)
            {
                if (CurrentValue > MaximumValue) CurrentValue = MaximumValue;
            }
            else
            {
                if (CurrentValue > MaximumValue) CurrentValue = MinimumValue;
            }

            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
        }


       

    }
}
