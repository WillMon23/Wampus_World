using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;

namespace CoolMathForGames
{
    struct Icon
    {
        public char Symbol;
        public ConsoleColor Color;

        /// <summary>
        /// Overrides the equals equals oporator in order to
        /// checks to see if the left hand side vector X and Y 
        /// matches the X and Y on the right hand side
        /// </summary>
        /// <param name="lhs">left hand side vector </param>
        /// <param name="rhs">right hand side vector</param>
        /// <returns>if there equal to each other</returns>
        public static bool operator ==(Icon lhs, char rhs)
        {
            return (lhs.Symbol == rhs && lhs.Symbol == rhs);
        }

        /// <summary>
        /// Overrides the not equals oporator in order to
        /// checks to see if the left hand side vector X and Y 
        /// does not match the X and Y on the right hand side
        /// </summary>
        /// <param name="lhs">left hand side vector </param>
        /// <param name="rhs">right hand side vector</param>
        /// <returns>if there not equal to each other</returns>
        public static bool operator !=(Icon lhs, char rhs)
        {
            return (lhs.Symbol != rhs || lhs.Symbol != rhs);
        }
    }
    class Actor
    {
        private Icon _icon;
        private string _name;
        private Vector2 _position;
        private bool _started;

        /// <summary>
        /// True if the start function has been called for this actor
        /// </summary>
        public bool Started { get { return _started; } }

        public Vector2 Posistion { get { return _position; } set { _position = value; } }
        
        public Icon Icon { get { return _icon; } }

        public string Name { get { return _name; } }

        /// <summary>
        /// Defult Contructor
        /// </summary>
        public Actor()
        {

        }

        public Actor(char icon, Vector2 position, string name = "Actor", ConsoleColor color = ConsoleColor.DarkRed)
        {
            _icon = new Icon { Symbol = icon, Color = color }; 
            _name = name;
            _position = position;
        }

        public Actor(char icon, float x, float y, string name = "Actor", ConsoleColor color = ConsoleColor.DarkRed) :
            this(icon, new Vector2 { X = x, Y = y }, name, color){ }

      
        public virtual void Start()
        {
            _started = true;
        }

        public virtual void Update()
        {
           
        }

        public virtual void Draw()
        {
            Engine.Render(_icon, Posistion);
        }

        public virtual void End()
        {

        }

        public virtual void OnCollision( Actor actor)
        {
            
        }


    }
}
