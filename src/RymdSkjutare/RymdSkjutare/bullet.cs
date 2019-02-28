using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RymdSkjutare
{
    class Bullet
    {
        Texture2D sprite;
        Vector2 position;
        Vector2 direction;
        Rectangle box;

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction)
        {
            sprite = texture;
            this.position = position;
            this.direction = direction;
            this.direction.Normalize();
            box = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);

        }

        public Rectangle Box
        {
            get { return box; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public void Update()
        {
            position += direction * 5;
        }
    }
}
