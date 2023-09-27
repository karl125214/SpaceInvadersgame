using Raylib_CsLo;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace SpaceInvaders
{
   
    public class GameSettings
    {
        public event EventHandler BackButtonPressedEvent;
        public event EventHandler MusicStartedEvent;
        public event EventHandler MusicStoppedEvent;
        
       

        public GameSettings()
        {

        }
        
        
            
        public void ShowSettings()
        {

            RayGui.GuiLabel(new Rectangle(250, 25, 0, 0), "GAME SETTINGS");


            Raylib.DrawText("Press ESC for settings when in game", 400 / 8f, 400 / 13 + 240, 20, Raylib.WHITE);

            if (RayGui.GuiButton(new Rectangle(50, 550, 70, 30), "Back"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }

            if (RayGui.GuiButton(new Rectangle(50, 200, 70, 30), "Play Music"))
            {
                Console.WriteLine("music");
                
                MusicStartedEvent.Invoke(this, EventArgs.Empty);
            }

            if (RayGui.GuiButton(new Rectangle(150, 200, 70, 30), "Stop Music"))
            {
                
                MusicStoppedEvent.Invoke(this, EventArgs.Empty);
            }
        }

        

   
    }
}
