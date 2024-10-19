using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        
        public int gameState = 2;

        public void CheckState(int gState)
        {
            if (gState == 0)
            {
                
            }
            else if (gState == 1)
            {
                 
            }
            else
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(800 / 2 - font.MeasureString("Game Over").X / 2, 800 / 2), Color.Black);
                spriteBatch.DrawString(font, "\nPress spacebar to return to menu", new Vector2(800 / 2 - font.MeasureString("\nPress spacebar to return to menu").X / 2, 800 / 2), Color.Black);
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Space))
                {
                    gameState = 0;
                }
            }
        }

        public void Draw()
        {
            spriteBatch.Begin();
            CheckState(gameState);
            spriteBatch.End();
        }
    }
}
