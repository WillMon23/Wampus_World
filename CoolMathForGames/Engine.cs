using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MathLibrary;


namespace CoolMathForGames
{
    class Engine
    {

        private static bool _applicationShouldClose = false;
        private static int _currentSceneIndex;
        private Scene[] _scenes = new Scene[0];
        private static Icon[,] _buffer;

        private Actor[,] _walls; 



        /// <summary>
        /// Called to begin the application 
        /// </summary>
        public void Run()
        {
            // Call start for the entire application 
            Start();

            // Loop until the application is told to close
            while (!_applicationShouldClose)
            {

                Draw();
                Update();

                Thread.Sleep(150);
            }
            // Called end for the entire application
            End();
        }

        /// <summary>
        /// Called when application starts 
        /// </summary>
        private void Start()
        {
            //Initulises the characters 
            Scene scene = new Scene();

            //Creats thr actors starting position
            Actor actor = new Actor('K', new MathLibrary.Vector2 { X = 0, Y = 0 }, "Actor1", ConsoleColor.Magenta);
            Actor actor2 = new Actor('R', new MathLibrary.Vector2 { X = 10, Y = 10 }, "Actor2", ConsoleColor.Green);
            scene.AddActor(actor);
            scene.AddActor(actor2);  

            Player player = new Player('P', 2, 2, 5, "Player", ConsoleColor.DarkCyan);
            scene.AddActor(player);


            Enemy wampus = new Enemy('W', 9, 9, 1, "Wampus", player, ConsoleColor.DarkCyan);
            scene.AddActor(wampus);


            UIText healthText = new UIText(30, 3, "Health", ConsoleColor.Blue, 20, 1);
            UIText livesText = new UIText(20, 1, "Lives", ConsoleColor.Red, 10,10 );
            PlayerHud playerHud = new PlayerHud(player,healthText,livesText);

            scene.AddUIElement(playerHud);

            BuildingWalls(20);
            
            for (int x = 0; x < _walls.GetLength(0); x++)
                for (int y = 0; y < _walls.GetLength(1); y++)
                {
                    if (_walls[x,y] != null)
                        scene.AddActor(_walls[x, y]);
                } 
             _currentSceneIndex = AddScene(scene);

        }

        /// <summary>
        /// Called to draw to the scene 
        /// </summary>
        private void Draw()
        {
            Console.CursorVisible = false;
            //Clear the stuff that was on the screen in the last frame 
            _buffer = new Icon[Console.WindowWidth, Console.WindowHeight - 1];

            // Resets the cursor position to the top
            Console.SetCursorPosition(0, 0);
            //Adds all actor icon to buffer
            _scenes[_currentSceneIndex].Draw();
            _scenes[_currentSceneIndex].DrawUI();

            //Iterate through buffer
            for (int y = 0; y < _buffer.GetLength(1); y++)
            {
                for (int x = 0; x < _buffer.GetLength(0); x++)
                {
                    //checks to see if there is a null char. . .
                    if (_buffer[x, y].Symbol == '\0')
                        //. . . If found it's replaced with a char space 
                        _buffer[x, y].Symbol = ' ';

                    //Set console text color ro be color of item at the buffer
                    Console.ForegroundColor = _buffer[x, y].Color;

                    //Print the symbol of the item in the buffer
                    Console.Write(_buffer[x, y].Symbol);
                }
                //Skip a line once the end of a row has been reached 
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Updates the application and notifies the console of any changes 
        /// </summary>
        private void Update()
        {
            _scenes[_currentSceneIndex].Update();
            _scenes[_currentSceneIndex].UpdateUI();

            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        /// <summary>
        /// Called once the game has been set to game over 
        /// </summary>
        private void End()
        {
            _scenes[_currentSceneIndex].End();
        }

        /// <summary>
        /// Creats walls around the grid
        /// </summary>
        /// <param name="size">Size of the walls</param>
        private void BuildingWalls(int size)
        {
            Actor[,] wallBilder = new Actor[size, size ];

            for (int x = 0; x < wallBilder.GetLength(0); x++)
                for (int y = 0; y < wallBilder.GetLength(1); y++)
                {
                    if (x == 0 || x == (wallBilder.GetLength(1) - 1))
                        wallBilder[x, y] = new Actor('▌', x, y, "Wall", ConsoleColor.White);

                    else if (y == 0 || y == (wallBilder.GetLength(0) - 1))
                        wallBilder[x, y] = new Actor('▄', x, y, "Wall", ConsoleColor.White);
                }
                _walls = wallBilder;
        }

        /// <summary>
        /// Created to append new scnene to the current listing of scene 
        /// </summary>
        /// <param name="scene">Scene being added to the current list of scens</param>
        /// <returns>returns the new ammount of scenes</returns>
        public int AddScene(Scene scene)
        {
            // Creats a Temporary array 
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            //Copys all the values from old array info to the temp array
            for (int i = 0; i < _scenes.Length; i++)
                tempArray[i] = _scenes[i];

            //Sets adds the new scene to the new size
            tempArray[_scenes.Length] = scene;

            // Set the old array to the new array
            _scenes = tempArray;

            // returns the new allocated size
            return _scenes.Length - 1;
        }

        /// <summary>
        /// Get the nexy key in the input stream
        /// </summary>
        /// <returns>The key thst waspressed </returns>
        public static ConsoleKey GetNextKey()
        {
            //if there is no key being pressed. . . 
            if (!Console.KeyAvailable)

                //. . . return
                return 0;

            //Return the current key being pressed 
            return Console.ReadKey(true).Key;
        }

        /// <summary>
        /// Adds the icon to the buffer to print to the scene in the next draw call
        /// Prints the icon at the given position in the buffer
        /// </summary>
        /// <param name="icon">The Icon to draw</param>
        /// <param name="position">The podition of the icon in the buffer</param>
        /// <returns>False if the positionis out side the bounds of the buffer</returns>
        public static bool Render(Icon icon, Vector2 position)
        {
            //If the position is out. . .
            if (position.X < 0 || position.X > _buffer.GetLength(0) ||
                position.Y < 0 || position.Y >= _buffer.GetLength(1))
                return false;

            _buffer[(int)position.X, (int)position.Y] = icon;

            return true;
        }

        /// <summary>
        /// when called will end the game
        /// </summary>
        public static void CloseApplication()
        {
            _applicationShouldClose = true;
        }



    }

}
