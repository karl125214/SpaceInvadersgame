using System.Numerics;
using System.Security.Principal;
using Invaders_Demo;
using Raylib_CsLo;


namespace SpaceInvaders
{

    internal class Program
    {


        static void Main(string[] args)
        {
            Invaders game = new Invaders();
            game.Run();
        }
    }

}