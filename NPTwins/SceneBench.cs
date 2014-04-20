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
    class SceneBench : Scene
    {
        Manager m_manager;

        public override void Init(Game game)
        {
            base.Init(game);

            m_manager = new Manager();
            m_manager.Init(m_game);

            m_game.res.m_musics["bgmTitle"].Play(true);
            m_game.res.m_fBgmPlay = false;
        }

        override public void DrawCore(Surface surface)
        {
            base.DrawCore(surface);

            m_manager.Draw(surface);

//            surface.Blit(m_game.res.m_surBackBox, new Point(100-30, 300-30));
//            surface.Blit(m_game.res.font.Render("NPTwins", Color.Aquamarine), new Point(100, 300));
//            surface.Blit(m_game.res.fontSmall.Render("progremmed by FUKAYA Takashi (F/T)", Color.White), new Point(100, 330));

//            surface.Blit(m_game.res.font.Render("COUNT:" + m_game.world.manager.GetCount(), Color.Yellow), new Point(200, 440));


            // surface.Blit(m_game.res.m_surTitle, new Point(0, 0));
            surface.Blit(m_game.res.fontSmall.Render("Objects:" + m_manager.GetCount(), Color.Yellow), new Point(10, 30));

        }

        public override void Action()
        {
            base.Action();

            m_manager.Action();

            uint uiJoy = m_game.GetJoystick();
            if (m_nCount <=1000 || (uiJoy & (int)Game.JOY.UP)!=0 )
            {
                Actor act = new Actor();
                act.Init(m_game);
                act.Pos = new Vector(m_game.GetRandom(640), m_game.GetRandom(480), 0);
                act.Vect = new Vector(0, 0, 0);
                // act.Chr = m_game.res.m_surEnemy[1, (m_nCount / 100) % 2, m_game.GetRandom(8)];
                act.Chr = m_game.res[IMG.ENEMY01][0, m_nCount & 360, m_nCount % 2];
                //                act.Chr = m_game.res.m_surWall;
 
                m_manager.Add(act);

            }


            if ( (uiJoy & (int)Game.JOY.DOWN) != 0 && m_manager.GetCount()> 0 )
            {
                m_manager.FindHit(new Vector(320, 240,0), 1000).m_fKill = true;
            }

  //          if ((m_nCount % 600) > 540) m_game.world.map.ScrollUpDemo();



            if ((m_game.GetJoystickValue() & (uint)Game.JOY.NEW_BTN0) != 0) GameStart();
        }

        public override void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            base.Events_KeyboardDown(sender, e);
            switch (e.Key)
            {
                case Key.M:
                    m_game.EnterScene(Game.SCENE.MAPEDIT);
                    break;

                case Key.S:
                    m_game.world.manager.AddByParam(Manager.ACTOR.MAN, m_nCount % 160, m_nCount / 99 % 120);
                    break;

                case Key.LeftArrow:
                    if (m_game.playinfo.m_nStartStage > 1) m_game.playinfo.m_nStartStage--;
                    break;
                case Key.RightArrow:
                    if (m_game.playinfo.m_nStartStage < 99) m_game.playinfo.m_nStartStage++;
                    break;

                case Key.Space:
                    GameStart();
                    break;


            }
        }

        public override void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.Events_MouseButtonDown(sender, e);
            GameStart();
        }


        void GameStart()
        {
            m_game.res.m_sounds["start"].Play();

            m_game.playinfo.Reset();
            m_game.EnterScene(Game.SCENE.GAME);
        }
    }
}
