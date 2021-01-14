using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump
{
    class Program
    {
        private static RenderWindow window;
	    private static Game game;
	    public static Random random;

        static void Main(string[] args)
        {
			random = new Random();

            window = new RenderWindow(new VideoMode(400, 533), "Doodle Jump", Styles.Close);    // создаём окно размером 400x533 с названием Doodle Jump
            window.SetMouseCursorVisible(false);                                                // делаем невидиомой курсор мыши

            Image icon = new Image("images/DoodleJump.png");        // загружаем
            window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);  // и устанавливаем иконку на окно

            window.Closed += OnWindowClosed;                        // подписываемся на событие закрития окна
            window.KeyPressed += OnKeyPressed;						// подписываемся на событие нажатия клавиши

																				// подгружаем игровые ресурсы
            Texture background	= new Texture("images/background1.png");        // текстуры фона
            Texture platform	= new Texture("images/platform.png");			// платформы
			Texture doodle		= new Texture("images/doodle.png");				// персонажа
			Font font			= new Font("fonts/doodleJump.ttf");				// а также основной шрифт

			game = new Game(background, platform, doodle, font);			// передаём в управление игры

            while (window.IsOpen)					// бесеконечный цикл пока открыто окно
            {
                window.DispatchEvents();			// обрабатываем события окна

				game.Update();						// обновляем игровую логику

                window.Clear();						// очищаем экран
				game.Render(window);				// рендерим игру
                window.Display();					// и отображаем на экран
            }
        }

        private static void OnKeyPressed(object sender, KeyEventArgs e)
        {
	        if (game.IsGameOver)
	        {
				game.Reset();
	        }
        }

        private static void OnWindowClosed(object sender, EventArgs e)
        {
            window.Close();			// если было возбуждено событие закрытия окна, то закрываем окно
        }
    }
}
