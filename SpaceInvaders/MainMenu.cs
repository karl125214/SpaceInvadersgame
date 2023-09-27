using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;
using Raylib_CsLo.InternalHelpers;
using System.ComponentModel.Design;

namespace SpaceInvaders
{
   

    internal class MainMenu
    {
        int width = 40;
        int height = 20;

        public event EventHandler StartButtonPressedEvent;
        public event EventHandler OptionButtonPressedEvent;
        public event EventHandler ExitButtonPressedEvent;

        public void ShowMenu()
        {
            // BUTTON FOCUS STYLE
            
            

            

            RayGui.GuiLabel(new Rectangle(250, 25, 0 , 0), "SPACE INVADERS");


           if( RayGui.GuiButton(new Rectangle(50, 75, 70, 30), "Play game"))
            {
                StartButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }

            if (RayGui.GuiButton(new Rectangle(50, 550, 70, 30), "Close"))
            {
                ExitButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }

           
            if (RayGui.GuiButton(new Rectangle(50, 125, 70, 30), "Settings"))
            {
                OptionButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            
        }


        
          
        

    }
}
