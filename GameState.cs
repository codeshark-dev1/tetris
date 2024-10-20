using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    class GameState
    {
        private Song song;
        public SpriteFont font;

        float fadeIncrement = 0.00300f;

        public enum state
        {
            Playing, GameOver
        }

        public state currentState = state.Playing;

        public GameState()
        {
            song = TetrisGame.ContentManager.Load<Song>("Running Errands - TrackTribe");
            MediaPlayer.Volume = 0.3f;

        }

        public void CheckState(SpriteBatch spriteBatch)
        {
            if (currentState == state.GameOver)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(600 - font.MeasureString("Game Over").X / 2, 800 / 2), Color.Black);
                spriteBatch.DrawString(font, "\nPress spacebar to play again!", new Vector2(600 - font.MeasureString("\nPress spacebar to play again!").X / 2, 800 / 2), Color.Black);

                if (MediaPlayer.Volume >= 0)
                {
                    MediaPlayer.Volume -= fadeIncrement;
                }
                else
                {
                    MediaPlayer.Stop();
                }
            }
            else
            {
                if (MediaPlayer.Volume <= 0.3f)
                {
                    MediaPlayer.Volume += fadeIncrement;
                }
                    if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(song);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CheckState(spriteBatch);
        }
    }
}