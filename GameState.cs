using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace TetrisTemplate
{
    class GameState
    {
        public SpriteFont font;
        public SpriteBatch spriteBatch;
        GameWorld gameWorld;

        public void CheckState(int gamestate)
        {
            if (gamestate == 0)
            {
                
            }
            else if (gamestate == 1)
            {
                 
            }
            else
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(800 / 2 - font.MeasureString("Game Over").X / 2, 800 / 2), Color.Black);
            }
        }

        public void Draw()
        {
            spriteBatch.Begin();
            CheckState(0);
            spriteBatch.End();
        }
    }
}
