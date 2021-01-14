using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Math = System.Math;

namespace DoodleJump.Entity
{
    public class Platform : Drawable		// класс Платформа (наследуется от интерфейса, который позволяет отрисовывать данный класс в окне)
    {
        public Texture Texture { get => sprite.Texture; set => sprite.Texture = value; }

        public Vector2f		position;			// спрайт платормы
        private Sprite		sprite;             // позиция платформы

	    public Platform()
	    {
			sprite = new Sprite();
	    }

	    public void Update(float time)
	    {
            if(position.Y > 533) Spawn();

		    sprite.Position = position;
	    }					// обновляет логику платформы

	    public void MoveDown(float offset)
	    {
		    position.Y += Math.Abs(offset);             // сдвигает позицию по вертикали вниз (с взятием смещения по модулю)
        }               // двигает вниз

        public void MoveUp(float offset)
	    {
		    position.Y -= Math.Abs(offset);             // сдвигает позицию по вертикали вверх (с взятием смещения по модулю)
            if (position.Y < -15) position.Y = -15;     // если поднялсявыше верхней границы экрана, то просто фикируем
        }           // двигает вверх

        public void Spawn()
	    {
		    position.X = Program.random.Next(0, 400) - 34;      // задаёт рандомную позицию по горизонтали в пределах экрана
            position.Y = 0;										// устанавливает сверху экрана
	    }								// спавнит(создаёт) платформу в новом месте

	    public void Random()
	    {
		    position.X = Program.random.Next(0, 400) - 34;      // задаёт рандомную позицию по горизонтали в пределах экрана
            position.Y = Program.random.Next(0, 533);			// задаёт рандомную позицию по вертикали в пределах экрана
        }								// рандомно раскидывает платформу в новом месте

	    public FloatRect GetGlobalBounds()
	    {
		    return sprite.GetGlobalBounds();
	    }				// рандомно раскидывает платформу в новом месте

        public void Draw(RenderTarget target, RenderStates states)
	    {
			target.Draw(sprite);
        }	  // реализация метода унаследованного от Drawable для отрисовки платформы
    }
}
