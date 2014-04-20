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
    class SceneTitle : Scene
    {
        Manager m_manager;

        int m_nMenuPhase;
        int m_nMenu;

        public override void Init(Game game)
        {
            base.Init(game);

            m_manager = new Manager();
            m_manager.Init(m_game);

            m_game.res.m_musics["bgmTitle"].Play(true);
            m_game.res.m_fBgmPlay = false;

            m_nMenuPhase = 0;
            m_nMenu = 1;
        }

        override public void DrawCore(Surface surface)
        {
            base.DrawCore(surface);

            m_manager.Draw(surface);

//            surface.Blit(m_game.res.m_surBackBox, new Point(100-30, 300-30));
//            surface.Blit(m_game.res.font.Render("NPTwins", Color.Aquamarine), new Point(100, 300));
//            surface.Blit(m_game.res.fontSmall.Render("progremmed by FUKAYA Takashi (F/T)", Color.White), new Point(100, 330));



            surface.Blit(m_game.res.m_surTitle, new Point(0, 0));

            if (m_game.playinfo.m_nStartStage > 1)
            {
                surface.Blit(m_game.res.fontSmall.Render("STAGE:" + m_game.playinfo.m_nStartStage, Color.Yellow), new Point(20, 20));
            }


            if (m_nMenuPhase > 0)
            {
                surface.Blit(m_game.res.m_surTitleMenu, new Point(380, 38));

                String strMenuName="";
                String[] strMenuItems = new String[] {};
                int iSelect = 0;

                switch (m_nMenuPhase)
                {
                    case 1:
                        strMenuName = "GAME MENU";
                        strMenuItems = new String[] { "START", "REPLAY", "BENCH", "EXIT" };
                        iSelect = m_nMenu - 1;
                        break;
                    case 2:
                        strMenuName = "GAME RANK";
                        strMenuItems = new String[] { "EASY", "NORMAL" , "HARD" , "HELL" };
                        iSelect = m_game.playinfo.m_nRank - 1;
                        break;
                    case 3:
                        strMenuName = "CONTROLL MODE";
                        strMenuItems = new String[] { "TYPE-T (TRACE) *", "TYPE-W (WIRED)", "TYPE-S (Satellite)", "TYPE-F (FIX)" };
                        iSelect = m_game.playinfo.m_nControllMode - 1;
                        break;
                }
                surface.Blit(m_game.res.fontSmall.Render(strMenuName, Color.White), new Point(400, 50));
                for (int i = 0; i < strMenuItems.Length; i++ )
                {
                    surface.Blit(m_game.res.fontSmall.Render(strMenuItems[i], i== iSelect ? Color.Red : Color.White), new Point(420, 90 + i * 30));
                }
            }

        }

        public override void Action()
        {
            base.Action();

            m_manager.Action();

            if (m_nCount % 4 == 0)
            {
                Actor act = new Actor();
                act.Init(m_game);
                act.Pos  = new Vector(m_game.GetRandom(480), -m_game.GetRandom(10),0);
                act.Vect = new Vector(0,4,0);
                act.Chr = m_game.res.m_surSekihou;
 
                m_manager.Add(act);

            }

  //          if ((m_nCount % 600) > 540) m_game.world.map.ScrollUpDemo();
            uint uiJoy = m_game.GetJoystickValue();
            if ((uiJoy & (uint)Game.JOY.NEW_BTN0) != 0)
            {
                m_game.res.m_sounds["start"].Play();

                if (m_nMenuPhase == 1)
                {
                    if (m_nMenu == 1) m_nMenuPhase++;
                    if (m_nMenu == 2) GameStart(true);
                    if (m_nMenu == 3) m_game.EnterScene(Game.SCENE.BENCH);
                    if (m_nMenu == 4) Events.QuitApplication();

                }
                else
                {
                    m_nMenuPhase++;
                }
                if (m_nMenuPhase >= 4)
                {
                    GameStart(false);
                }
            }
            if ((uiJoy & (uint)Game.JOY.NEW_BTN1) != 0)
            {

                m_game.res.m_sounds["edamaged"].Play();
                if (m_nMenuPhase > 0) m_nMenuPhase--;
            }

            if ((uiJoy & (uint)Game.JOY.NEW_UP) != 0 && m_nMenuPhase> 0 )
            {
                m_game.res.m_sounds["change"].Play();
                if (m_nMenuPhase == 1 && m_nMenu > 1) m_nMenu--;
                if (m_nMenuPhase == 2 && m_game.playinfo.m_nRank > 1) m_game.playinfo.m_nRank--;
                if ( m_nMenuPhase == 3 && m_game.playinfo.m_nControllMode>1 ) m_game.playinfo.m_nControllMode--;
            }
            if ((uiJoy & (uint)Game.JOY.NEW_DOWN) != 0 && m_nMenuPhase > 0)
            {
                m_game.res.m_sounds["change"].Play();
                if (m_nMenuPhase == 1 && m_nMenu < 4) m_nMenu++;
                if (m_nMenuPhase == 2 && m_game.playinfo.m_nRank < 4) m_game.playinfo.m_nRank++;
                if ( m_nMenuPhase == 3 && m_game.playinfo.m_nControllMode<4 ) m_game.playinfo.m_nControllMode++;
            }

        }

        public override void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            base.Events_KeyboardDown(sender, e);
            switch (e.Key)
            {
                case Key.B:
                    m_game.EnterScene(Game.SCENE.BENCH);
                    break;

                case Key.R:
                    if ( m_game.world.m_joyrecord.IsValid() )  GameStart(true);
                    break;
                case Key.L:
                    m_game.world.m_joyrecord = JoyStickRecord.Load("replay" + System.IO.Path.DirectorySeparatorChar + "replay.xml");
                    break;

                case Key.LeftArrow:
                    if (m_game.playinfo.m_nStartStage > 1) m_game.playinfo.m_nStartStage--;
                    break;
                case Key.RightArrow:
                    if (m_game.playinfo.m_nStartStage < Stage.StageManager.GetMaxStage() ) m_game.playinfo.m_nStartStage++;
                    break;

//                case Key.Space:
//                    GameStart();
//                    break;


            }
        }

        public override void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.Events_MouseButtonDown(sender, e);
            // GameStart();
        }


        void GameStart(bool fDemo)
        {

            m_game.playinfo.Reset();

            m_game.playinfo.m_fDemo = fDemo;
            if (fDemo)
            {
                m_game.world.m_joyrecord.PlayStart();

                m_game.playinfo.m_nRank = m_game.world.m_joyrecord.m_nRank;
                m_game.playinfo.m_nStage = m_game.world.m_joyrecord.m_nStage;
                m_game.playinfo.m_nControllMode = m_game.world.m_joyrecord.m_nControllMode;

            }
            else
            {
                m_game.world.m_joyrecord.Reset();
                m_game.world.m_joyrecord.m_nRank = m_game.playinfo.m_nRank;
                m_game.world.m_joyrecord.m_nStage = m_game.playinfo.m_nStage;
                m_game.world.m_joyrecord.m_nControllMode = m_game.playinfo.m_nControllMode;
            }
            m_game.EnterScene(Game.SCENE.GAME);
        }
    }
}
