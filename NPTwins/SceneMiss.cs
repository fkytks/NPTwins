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
    public class SceneMiss : Scene
    {
        public override void Init(Game game)
        {
            base.Init(game);

            //m_game.res.m_musics["miss"].Play(false);
            //Mixer.Stop();
            MusicPlayer.Stop();
            m_game.res.m_fBgmPlay = false;


        }

        override public void DrawCore(Surface surface)
        {
            base.DrawCore(surface);
            m_game.world.Draw(surface);

            m_game.res.m_surSidePictBG.Alpha = 200;
            m_game.res.m_surSidePictBG.AlphaBlending = true;
            for (int i = 0; i < 3; i++)
            {
                int y = 500 +i * 400 - m_nCount * m_nCount/10;
                if (y<0 ) y=0;
                surface.Blit(m_game.res.m_surSidePictBG, new Point(i*160, y ));
            }

            if (m_nCount % 15 < 8 || m_nCount>60)
            {
                surface.Blit(m_game.res.font.Render("GAME OVER", Color.Yellow), new Point(167, 200));
            }
        }

        public override void Action()
        {
            base.Action();
            m_game.world.Action();


            if ((m_game.GetJoystickValue() & (uint)Game.JOY.NEW_BTN0) != 0 && m_nCount > 60) m_game.EnterScene(Game.SCENE.TITLE);
        }

        public override void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            base.Events_KeyboardDown(sender, e);
            switch (e.Key)
            {
                case Key.S:
                    // SAVE REPLAY DATA
                    JoyStickRecord.Save(m_game.world.m_joyrecord, "replay" + System.IO.Path.DirectorySeparatorChar + "replay.xml");

                    break;


            }
        }


    }
}
