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
     public class SceneClear : Scene
    {
        public override void Init(Game game)
        {
            base.Init(game);
            m_game.res.m_sounds["clear"].Play();
        }

        override public void DrawCore(Surface surface)
        {
            base.DrawCore(surface);
            m_game.world.Draw(surface);

            if (m_nCount % 15 < 8 || m_nCount > 60)
            {
                surface.Blit(m_game.res.font.Render("STAGE CLEAR !!", Color.White), new Point(160, 200));
            }
        }

        public override void Action()
        {
            base.Action();

            m_game.world.Action();


            if (m_nCount > 240)
            {
                m_game.playinfo.m_nStage++;

                if (m_game.playinfo.m_nStage <= Stage.StageManager.GetMaxStage())
                {
                    m_game.EnterScene(Game.SCENE.GAME);
                }
                else
                {
                    m_game.EnterScene(Game.SCENE.MISS);
                }
            }
        }

        public override void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            base.Events_KeyboardDown(sender, e);
            switch (e.Key)
            {
                case Key.Space:
                case Key.Z:
//                    m_game.EnterScene(Game.SCENE.GAME);
                    break;


            }
        }


    }
}
