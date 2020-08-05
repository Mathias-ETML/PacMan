using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public partial class Food
    {
        public char Type { get; }

        public Food(char type)
        {
            Type = type;
        }

        public struct FoodMeaning
        {
            public const char NORMALFOOD = 'f';
            public const char BIGFOOD = 'b';
        }
    }
}
