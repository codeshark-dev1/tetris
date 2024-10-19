using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TetrisTemplate
{
    class GameState
    {
        public SpriteFont font;

        public enum state
        {
            Playing, GameOver
        }

        public state currentState = state.Playing;

        public void CheckState(SpriteBatch spriteBatch)
        {
            if (currentState == state.GameOver)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(800 / 2 - font.MeasureString("Game Over").X / 2, 800 / 2), Color.Black);
                spriteBatch.DrawString(font, "\nPress spacebar to play again!", new Vector2(800 / 2 - font.MeasureString("\nPress spacebar to play again!").X / 2, 800 / 2), Color.Black);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CheckState(spriteBatch);
        }
    }
}