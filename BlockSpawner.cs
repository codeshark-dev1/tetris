using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace TetrisTemplate
{
    class BlockSpawner
    {
        private TetrisBlock currentBlock;

        private string[] possibleShapes = { "O", "I", "S", "Z", "L", "J", "T" };

        public BlockSpawner()
        {
            SpawnBlock();
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void SpawnBlock()
        {
            var rand = new Random();
            string newShape = possibleShapes[rand.Next(possibleShapes.Length)];
            currentBlock = new TetrisBlock(newShape);
        }

        public TetrisBlock GetNewBlock()
        {
            return currentBlock;
        }
    }
}