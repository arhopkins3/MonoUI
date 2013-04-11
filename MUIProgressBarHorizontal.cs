using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    public class MUIProgressBarHorizontal : MUIControl
    {
        private RasterizerState scissorOn;
        private RasterizerState scissorOff;

        private float onePctWidth;
        private Rectangle barRect;
        private Rectangle borderRect;
        private FontRenderer fontRenderer;
        public Color BackgroundColour;
        public Color ForegroundColour;
        public Color CaptionColour;
        public bool ShowCaption = true;
        public int BorderSize = 5;
        public float Value { get; set; }
        public int IntValue { get { return (int)Value; } set { Value = value; } }

        public MUIProgressBarHorizontal(FontRenderer fr, int x, int y, int w = 100, int h = 100) : base(x, y, w, h)
        {
            fontRenderer = fr;
            scissorOn = new RasterizerState {ScissorTestEnable = true};
            scissorOff = new RasterizerState {ScissorTestEnable = false};
            onePctWidth = (float)Width / 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            barRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(onePctWidth * Value),
                Height);

            borderRect = new Rectangle(
                (int)Position.X - BorderSize,
                (int)Position.Y - BorderSize,
                Width + (BorderSize * 2),
                Height + (BorderSize * 2));
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            MUIRectangle.Draw(sb, borderRect, BackgroundColour);
            MUIRectangle.Draw(sb, barRect, ForegroundColour);
            if (ShowCaption) fontRenderer.DrawText(sb, (int)Position.X + (Width/2) - ((int)fontRenderer.MeasureString(Value + "%").X/2), (int)Position.Y + (Height/2) - (int)fontRenderer.MeasureString(Value + "%").Y/2, Value + "%", CaptionColour);
        }
    }
}
