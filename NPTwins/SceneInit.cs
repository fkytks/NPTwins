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
    public class SceneInit : Scene
    {
        public override void Init(Game game)
        {
            base.Init(game);
        }

        override public void DrawCore(Surface surface)
        {
            base.DrawCore(surface);
            surface.Blit(m_game.res.font.Render("MIZUGAME", Color.Aquamarine), new Point(200, 200));
            surface.Blit(m_game.res.font.Render("Now Initialize...", Color.White), new Point(200, 250));
        }

        public override void Action()
        {
            base.Action();

            if (m_nCount > 30) m_game.EnterScene(Game.SCENE.TITLE);
        }
    }
}
