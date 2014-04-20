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
    class SceneGame : Scene
    {
        public int m_nNextScene;


        public override void Init(Game game)
        {
            base.Init(game);

            m_game.world.Init(m_game);

            if (m_game.res.m_fBgmPlay == false)
            {
                m_game.res.m_musics["bgm"].Play(true);
                m_game.res.m_fBgmPlay = true;
            }
            m_nNextScene = 0;

        }

        override public void DrawCore(Surface surface)
        {
            base.DrawCore(surface);

            m_game.world.Draw(surface);

        }

        public override void Action()
        {
            base.Action();
            m_game.world.Action();

            if (m_game.world.m_CurrentPlayer.m_nLife == 0 && m_game.world.m_CurrentPlayer.m_nDamagedCounter == 0)
            {
                m_game.EnterScene(Game.SCENE.MISS);
            }

            if (m_game.world.m_stagemanager.GetEnemyLeft() == 0 && m_game.world.m_mgrEnemy.GetCount() == 0 && m_nNextScene == 0) m_nNextScene = 120;
            if (m_nNextScene > 0)
            {
                m_nNextScene--;
                if (m_nNextScene == 0) m_game.EnterScene(Game.SCENE.CLEAR);
            }

       }



        public override void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            base.Events_KeyboardDown(sender, e);
            switch (e.Key)
            {
                case Key.M:
  //                  m_game.playinfo.m_nStartStage = m_game.playinfo.m_nStage;
  //                  m_game.EnterScene(Game.SCENE.MAPEDIT);
                    break;

                case Key.S:
                    m_game.res.m_sounds["click"].Play();
                    m_game.world.m_fLightMode = ( m_game.world.m_fLightMode  ? false :true );
                    break;

                case Key.U:
                    m_game.world.m_CurrentPlayer.m_nLife = 10;
                    m_game.world.m_SlavePlayer.m_nLife = 10;
                    break;

                case Key.T:
                    m_game.EnterScene(Game.SCENE.TITLE);
                    break;

                case Key.G:
                    m_game.res.m_sounds["click"].Play();

                    m_game.EnterScene(Game.SCENE.GAME);
                    break;


            }
        }



    }
}
