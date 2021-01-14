using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleJump.Entity;
using SFML.Graphics;
using SFML.System;

namespace DoodleJump
{
    public class Game				// класс Игра
    {
		public bool IsGameOver => gameOverTimer > 1000 && gameOver;

	    private Sprite		background;         // спрайт фона
        private Text		gameOverText;		// текст "Game over"
	    private Text		scoreText;			// текст игрового счёта
	    private Text		continueText;		// текст "для продолжения нажмите любую кнопку"

	    private Player			player;         // игрок
        private List<Platform>	platforms;		// массив платформ

	    private Clock clock;

	    private bool	gameOver;
	    private float	gameOverTimer;
	    private float	score;

        public Game(				// конструктор по умолчанию принимающий
            Texture backgroundTex,	// текстуру фона
			Texture platformTex,	// платформы
			Texture doodleTex,		// персонажа
			Font	font			// и шрифт
            )
        {
            // инициализация полей значениями по умолчанию

			clock = new Clock();

            // устанавливаем текстуру на спрайт фона
            background = new Sprite(backgroundTex);

            // персонажа
	        player = new Player(doodleTex, new Vector2f(200, 200));

            // массива платформ, состоящего из 10 платорм
            platforms = new List<Platform>();
	        for (int i = 0; i < 10; i++)
	        {
				Platform platform = new Platform() { Texture = platformTex };	// создаём плаформу
				platform.Random();                  // раскидываем платформы

                platforms.Add(platform);			// добавляем в список платформ
	        }

            // надпись "Game over" с размером шрифта 100
            gameOverText = new Text()
            {
				DisplayedString = "Game Over",
				Font = font,
				CharacterSize = 100,
				Color = Color.Red,
                Position = new Vector2f(12, 120)
            };

            // надпись счёта равного по умолчанию 0 и размером шрифта 60
	        scoreText = new Text()
	        {
		        DisplayedString = "0",
		        Font = font,
		        CharacterSize = 60,
		        Color = Color.Red,
				Style = Text.Styles.Bold,
		        Position = new Vector2f(10, 0)
	        };

            // аналогично для надписи "нажмите любу клавишу для продолжения"
            continueText = new Text()
            {
                DisplayedString = "Press any key to continue...",
		        Font = font,
		        CharacterSize = 40,
		        Color = Color.Red,
		        Position = new Vector2f(10, 480)
            };

            gameOver = false;
	        gameOverTimer = 0f;		// сбрасываем таймер
	        score = 0f;				// сбрасываем счёт
        }

	    public void Update()
	    {
		    float time = clock.ElapsedTime.AsMicroseconds();        // получаем текущее время в микросекндах
            clock.Restart();										// сбрасывем часы
		    time /= 500;											// делим время на коэффициент, регулирующий скорость протекания игрового времени

            // updating game logic
			player.Update(time);                                    // обновляем логику персонажа
            foreach (var platform in platforms)						// перебираем все платформы
		    {
			    platform.Update(time);								// обновляем логику платформы
				player.Collision(platform.GetGlobalBounds());		// проверяем на коллизию пресонажа и платформы
		    }

		    if (player.IsJump)                                              // если персонаж прыгнул
            {
			    score += Math.Abs(player.VerticalAcceleration) / 30f;		// увеличиваем счёт высоты
			    
			    player.Freeze();											// фиксируем персонажа на одной высоте, чтобы казалось, что он стоит на месте
			    foreach (var platform in platforms)							// (перебираем все платформы)
					platform.MoveDown(player.VerticalAcceleration*time);	// а платфомы двигаются вниз, со скоростью равной ускорению персонажа усноженного на время
		    }

		    if (player.IsFell) gameOver = true;                         // если персонаж упал, то конец игры

		    if (gameOver)                                                           // если конец игры, то
            {
			    foreach (var platform in platforms)
                    platform.MoveUp(0.6f * time);									// двигаем все платфомы вверх, как будто персонаж падает вниз

			    gameOverTimer += time;												// засекаем время после проигрыша

				if(!(gameOverTimer > 1000))
                    scoreText.Position += new Vector2f(0, -0.6f * time);			// убираем счёт наверх экрана до того как прошла секунда после проигрыша
            }
		    else
		    {
			    scoreText.DisplayedString = ((int)score).ToString() + " m";			// иначе, если не было проигрыша, то выводим текст счёта как (для примера) "135 m"
		    }

		    if (gameOverTimer > 1000 && gameOver)									// елси был конец игры и прошла секунда после проигрыша
		    {
			    scoreText.DisplayedString = "Score: " + ((int) score).ToString();	// выводим текст счёта как "Score: 135"
                scoreText.Position = new Vector2f(100, 250);						// устанавливаем позицию текста счёта под надписью "Game over"

				player.MoveDown(0.2f * time);										// опускаем персонажа под экран
            }

        }							// обновляющет логику игры

        public void Render(RenderTarget target)
	    {
			target.Draw(background);		// рисуем задний фон
			target.Draw(player);            // рисуем игрока

		    foreach (var platform in platforms)
		    {
			    target.Draw(platform);      // рисуем платформы
            }

			target.Draw(scoreText);			// рисуем текст игрового счёта

		    if (gameOver)   // если конец игры
            {
				target.Draw(gameOverText);      // рисуем текст "конец игры"

				if(gameOverTimer > 1000)        // если прошла секунда после проигрыша
                    target.Draw(continueText);	// рисуем текст "нажмите любую клавишу"
            }

        }	   // отрисовывает сцену

	    public void Reset()
	    {
		    gameOver = false;
		    score = 0f;
		    gameOverTimer = 0f;

			player.Reset();								// сбрасываем игрока
            scoreText.Position = new Vector2f(10, 0);	// переносим текст в исходное состояние

		    foreach (var platform in platforms)
		    {
			    platform.Random();						// разбрасываем платформы
		    }
        }														// перезапускает игру
    }
}
