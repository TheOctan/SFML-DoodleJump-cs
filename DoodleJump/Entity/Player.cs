using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DoodleJump.Entity
{
    public class Player : Drawable		// класс Игрок (наследуется от интерфейса, который позволяет отрисовывать данный класс в окне)
    {
        public Texture		Texture { get => sprite.Texture; set => sprite.Texture = value; }				// текстура персонажа
	    public float		VerticalAcceleration => dy;				// возврашает верстикальное ускорение
	    public bool			IsJump => position.Y < height;							// проверяет, прыгнул ли персонаж
	    public bool			IsFell => position.Y + 70 > 533;                // проверяет, упал ли персонаж

	    public Vector2f			position;			// позиция персонажа
        private Sprite			sprite;             // спрайт персонажа

        private readonly int	height;             // высота, выше которой он не может прыгнуть
        private float			dx;                 // ускорение по горизонтали
        private float			dy;                 // ускорение по вертикали
        private bool			fell;               // статус игрока (упал или нет)

	    public Player(Texture texture, Vector2f position)
        {
	        this.position = position;
			sprite = new Sprite(texture);

		    height = 200;
		    dx = 0;
		    dy = -0.6f;
		    fell = false;

            sprite.Origin = new Vector2f(                       // установка центра вращения спрайта персонажа
                sprite.GetGlobalBounds().Width / 2f + 10,		// по X, равного половине ширины спрайта
	            0);												// по Y = 0
        }

	    public void Update(float time)
	    {
		    if (IsFell) fell = true;                            // если игрок упал, стафим статус УПАЛ = ИСТИНА

            if (!fell)											// если игрок упал		
				Control();										// то отключам управление

            if (dx > 0) sprite.Scale = new Vector2f(-1, 1);		// если ускорение по горизонтали больше нуля, то зеркалим спрайт персонажа
            if (dx < 0) sprite.Scale = new Vector2f(1, 1);		// в противном случае возвращаем как было

		    dy += 0.0006f * time;								// изменяем вертикальное ускорение
		    if (fell) dy = 0f;									// если упал обнуляем вертикальное ускорение

		    position.X += dx * time;							// изменяем координаты по горизонтали
		    position.Y += dy * time;							// изменяем координаты по вертикали

		    dx = 0;												// сбрасываем горизонтальное ускорение, чтобы персонаж не двигался сам по себе без нажатия клавиш

		    if (position.X > 400) position.X = -sprite.GetGlobalBounds().Width;     // если персонаж уходит в правую часть экрана, чтобы он появлялся слева
            if (position.X < -sprite.GetGlobalBounds().Width) position.X = 400f;	// и наоборот

			sprite.Position = new Vector2f(											// устанавливаем спрайт в заданную позицию со смещением центра спрайта
				position.X + sprite.GetGlobalBounds().Width / 2f + 10,
				position.Y
				);

	    }			// обновляет логику персонажа

	    public void Control()
	    {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) dx = 0.2f;   // если нажата клавиша Вправо -> то ускорение по горизонтали больше нуля
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) dx = -0.2f;   // если нажата клавиша Влево <- то ускорение по горизонтали меньше нуля
        }					// управление персонажем

	    public void Collision(FloatRect rect)
	    {
		    if (
			    position.X + 50 > rect.Left			&&
				position.X + 20 < rect.Left + 68	&&
				position.Y + 70 > rect.Top			&&
				position.Y + 70 < rect.Top + 14		&&
				dy > 0
			    )
		    {
			    dy = -0.6f;
		    }
        }					// проверка на столкновение

	    public void Freeze()
	    {
		    position.Y = height;
	    }						// заморозка(фиксирует персонажа на месте)

	    public void MoveDown(float offset)
	    {
            position.Y += Math.Abs(offset);
		    if (position.Y > 533) position.Y = 533;
	    }	  // двигает вниз

	    public void Reset()
	    {
            // сбрасываем параметры персонажа выставляя значения по умолчанию
		    fell = false;
            position = new Vector2f(200, 200);
		    dy = -0.6f;
	    }						// перезапускает

        public void Draw(RenderTarget target, RenderStates states)
        {
			target.Draw(sprite);
        }	  // реализация метода унаследованного от Drawable для отрисовки персонажа
    }
}
