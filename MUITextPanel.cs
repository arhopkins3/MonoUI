using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoUI
{
    public class TextElement
    {
        public Vector2 Position;
        public string Caption;
        public string Name;
        public Color Colour;
        public TextElement(string n, string c, Color col)
        {
            Name = n;
            Position = Vector2.Zero;
            Caption = c;
            Colour = col;

        }
    }

    public class MUITextPanel : MUIPanel
    {
        public Vector2 LineSpacing = new Vector2(0, 20);
        public Vector2 TextOffset;
        private Vector2 currentTextOffset;
        private SpriteFont spriteFont;

        public MUITextPanel(int x, int y, int w, int h, SpriteFont sf, Vector2 to) : base(x, y, w, h)
        {
            spriteFont = sf;
            TextOffset = to;
        }
        private Dictionary<string, TextElement> TextElements = new Dictionary<string, TextElement>();
        
        public void AddText(TextElement textElement)
        {
            textElement.Position = this.Position + (currentTextOffset += TextOffset);
            TextElements.Add(textElement.Name, textElement);
        }
       
        public void SetTextValue(string name, string value)
        {
            if (TextElements.ContainsKey(name))
            {
                TextElements[name].Caption = value;
            }
        }

        public void RemoveText(string name)
        {
            if (TextElements.ContainsKey(name))
            {
                TextElements.Remove(name);
            }
        }

        public void SetTextColour(string name, Color colour)
        {
            if (TextElements.ContainsKey(name))
            {
                TextElements[name].Colour = colour;
            }
        }

        public void ClearAllText()
        {
            TextElements.Clear();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            foreach (KeyValuePair<string, TextElement> text in TextElements)
            {
                sb.DrawString(spriteFont, text.Value.Caption, text.Value.Position, text.Value.Colour);
            }
        }
    }
}
