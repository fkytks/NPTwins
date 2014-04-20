using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;

namespace NPTwins
{
    class Program
    {

        Game m_game = new Game();


        static void Main(string[] args)
        {
            // SDL_VIDEODRIVER=directx
            // System.Environment.SetEnvironmentVariable("SDL_VIDEODRIVER", "directx");

            Program program = new Program();
            program.Go();
        }

        public void Go()
        {
            m_game.Init();
            m_game.Run();

        }
    }
}
