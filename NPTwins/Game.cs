using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Drawing;
using System.Diagnostics;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;


namespace NPTwins
{
    public class Game
    {
        // 設定
        Setting m_setting = new Setting();
        public Setting Setting { get { return m_setting; } }

        // ワールド
        World m_world;
        public World world
        {
            get {return m_world; }
        }

        // プレイ情報
        PlayInfo m_playinfo;
        public PlayInfo playinfo
        {
            get { return m_playinfo; }
        }

        // サーフェイス
        Surface m_surScreen;
        bool m_fFullScreen;
        bool m_fHardware;

        // リソース
        Res m_resource = new Res();
        public Res res
        {
            get { return m_resource; }
        }

        // シーン
        Scene m_scene;
        public enum SCENE {
            INIT,TITLE,GAME,MISS,CLEAR,MAPEDIT,BENCH
        };
        SCENE m_eScene;
        public SCENE scene
        {
            get { return m_eScene; }
        }

        bool m_fPause;      // ポーズ中フラグ


        // FPS
        int m_nFPS;
        int m_nFPSCount;
        Stopwatch m_swFPS = new Stopwatch();

        // Random
        Random m_rnd;
        public int GetRandom(int nMax) { return m_rnd.Next(nMax); }

        // JoyStick
        uint m_uiJoystickValue;
        Joystick m_joystick;


        public void Init()
        {
            // 設定読み込み
            m_setting = Setting.LoadSetting();
            Setting.RunCount++;
            Setting.SaveSetting(Setting);

            // ウィンドウ作成
            if (Setting.VideoDriver != "")
            {
                System.Environment.SetEnvironmentVariable("SDL_VIDEODRIVER", Setting.VideoDriver);
            }
            Video.WindowIcon();
            Video.WindowCaption = "NPTwins ver 0.19 by FUKAYA Takashi(F/T) 2009.11.15";
            m_fFullScreen = Setting.FullScreen;
            m_fHardware = Setting.UseSDLHardware;
            m_surScreen = Video.SetVideoMode(640, 480, true, false, m_fFullScreen, m_fHardware);


            // 
            Debug.Print(System.Environment.CurrentDirectory);
            m_rnd = new Random();


            // 起動メッセージ
            m_surScreen.Fill(Color.Crimson);
            m_surScreen.Update();

            SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font("font" + System.IO.Path.DirectorySeparatorChar +  "mplus-2p-black.ttf", 24);
            m_surScreen.Blit(font.Render("Start:NPTwins...", Color.White), new Point(50, 50));

            m_surScreen.Update();

            m_surScreen.Blit(font.Render("VideoDriver:" + Video.VideoDriver , Color.White), new Point(50, 100));
            m_surScreen.Update();


            //
            SdlDotNet.Graphics.Primitives.Circle circle = new SdlDotNet.Graphics.Primitives.Circle(400, 300, 50);
            circle.Draw(m_surScreen, Color.FromArgb(150, 255, 0, 0),true,true);
            circle.PositionX += 40;
            circle.Draw(m_surScreen, Color.FromArgb(150, 0, 255, 0), true, true);
            circle.PositionX -= 20;
            circle.PositionY += 40;
            circle.Draw(m_surScreen, Color.FromArgb(150, 0, 0, 255), true, true);

            m_surScreen.Update();


            // リソース読み込み
            m_resource.Init(m_surScreen);

            // ジョイスティック
            if (Joysticks.NumberOfJoysticks > 0)
            {
                m_joystick = new Joystick(0);
            }        

            // イベントハンドラ登録
            Events.TargetFps = 60;
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(Events_MouseButtonDown);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(Events_MouseButtonUp);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(Events_MouseMotion);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(Events_KeyboardDown);

            // ワールド/プレイ情報作成

            m_playinfo = new PlayInfo();
            m_playinfo.Init();
            m_fPause = false;

            m_world = new World();
            m_world.Init(this);

            // リプレイ
            world.m_joyrecord = JoyStickRecord.Load( "replay" + System.IO.Path.DirectorySeparatorChar + "replay.xml");
            // 

            // スタート
            EnterScene(SCENE.TITLE);

        }

        public void Run()
        {
            Events.Run();
        }


        private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.T:
                    EnterScene(SCENE.TITLE);
                    break;

                case Key.Pause:
                    m_fPause = !m_fPause;
                    break;

                case Key.Return:
                    {
                        KeyboardState ks = new KeyboardState();
                        if (ks.IsKeyPressed(Key.RightAlt) || ks.IsKeyPressed(Key.LeftAlt))
                        {
                            m_fFullScreen = !m_fFullScreen;
                            m_surScreen = Video.SetVideoMode(640, 480, true, false, m_fFullScreen, m_fHardware);
                        }
                    }
                    break;

                case Key.F4:
                    {
                        KeyboardState ks = new KeyboardState();
                        if (ks.IsKeyPressed(Key.RightAlt) || ks.IsKeyPressed(Key.LeftAlt))
                        {
                            Events.QuitApplication();
                        }
                    }
                    break;
                case Key.Escape:
                case Key.F12:
                    Events.QuitApplication();
                    break;
            }



            m_scene.Events_KeyboardDown(sender, e);
        }

        private void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_scene.Events_MouseButtonDown(sender, e);
        }
        private void Events_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_scene.Events_MouseButtonUp(sender, e);
        }
        private void Events_MouseMotion(object sender, MouseMotionEventArgs e)
        {
            m_scene.Events_MouseMotion(sender, e);
        }

        private void Tick(object sender, TickEventArgs args)
        {
            GetJoystick();
            if ( !m_fPause ) m_scene.Action();
            m_scene.Draw(m_surScreen);

            // FPS
            m_nFPSCount++;
            if (m_nFPSCount >= 10)
            {
                m_nFPS = m_nFPSCount * 1000 / (int)m_swFPS.ElapsedMilliseconds;
                m_nFPSCount = 0;
                m_swFPS.Reset();
                m_swFPS.Start();
            }
            m_surScreen.Blit(res.fontSmall.Render("FPS:" + m_nFPS, Color.Yellow), new Point(10, 450));

            // Screen Update
            m_surScreen.Update();

        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }



        public void EnterScene(SCENE eScene)
        {
            if (m_scene != null)
            {
                m_scene.Release();
                m_scene = null;
            }

            m_eScene = eScene;
            switch (eScene)
            {
                case SCENE.INIT:
                    m_scene = new SceneInit();
                    break;
                case SCENE.TITLE:
                    m_scene = new SceneTitle();
                    break;
                case SCENE.GAME:
                    m_scene = new SceneGame();
                    break;
                case SCENE.CLEAR:
                    m_scene = new SceneClear();
                    break;
                case SCENE.MISS:
                    m_scene = new SceneMiss();
                    break;
                case SCENE.MAPEDIT:
//                    m_scene = new SceneMapEdit();
                      break;
                case SCENE.BENCH:
                      m_scene = new SceneBench();
                      break;

            }

            m_scene.Init(this);


            m_nFPS = 0;
            m_nFPSCount = 0;
            m_swFPS.Start();
        }


        public enum JOY
        {
            UP = 0x01,
            DOWN = 0x02,
            LEFT = 0x04,
            RIGHT = 0x08,
            BTN0 = 0x10,
            BTN1 = 0x20,
            BTN2 = 0x40,
            BTN3 = 0x80,
            NEW_UP = 0x0100,
            NEW_DOWN = 0x0200,
            NEW_LEFT = 0x0400,
            NEW_RIGHT = 0x0800,
            NEW_BTN0 = 0x1000,
            NEW_BTN1 = 0x2000,
            NEW_BTN2 = 0x4000,
            NEW_BTN3 = 0x8000,
            REL_UP = 0x010000,
            REL_DOWN = 0x020000,
            REL_LEFT = 0x040000,
            REL_RIGHT = 0x080000,
            REL_BTN0 = 0x100000,
            REL_BTN1 = 0x200000,
            REL_BTN2 = 0x400000,
            REL_BTN3 = 0x800000
        }
        public uint GetJoystickRaw()
        {
            uint uiJoy = 0;

            KeyboardState ks = new KeyboardState();
            if (ks.IsKeyPressed(Key.LeftArrow) || ks.IsKeyPressed(Key.J))
            {
                uiJoy |= (uint)JOY.LEFT;
            }
            if (ks.IsKeyPressed(Key.RightArrow) || ks.IsKeyPressed(Key.L))
            {
                uiJoy |= (uint)JOY.RIGHT;
            }
            if (ks.IsKeyPressed(Key.UpArrow) || ks.IsKeyPressed(Key.I))
            {
                uiJoy |= (uint)JOY.UP;
            }
            if (ks.IsKeyPressed(Key.DownArrow) || ks.IsKeyPressed(Key.K))
            {
                uiJoy |= (uint)JOY.DOWN;
            }
            if (ks.IsKeyPressed(Key.Space) || ks.IsKeyPressed(Key.Z))
            {
                uiJoy |= (uint)JOY.BTN0;
            }
            if (ks.IsKeyPressed(Key.X))
            {
                uiJoy |= (uint)JOY.BTN1;
            }
            if (ks.IsKeyPressed(Key.C))
            {
                uiJoy |= (uint)JOY.BTN2;
            }
            if (ks.IsKeyPressed(Key.LeftShift))
            {
                uiJoy |= (uint)JOY.BTN3;
            }


            // ジョイスティック
            if (m_joystick != null)
            {
                float JoyH = m_joystick.GetAxisPosition(JoystickAxis.Horizontal);
                if (JoyH < 0.3) uiJoy |= (uint)JOY.LEFT;
                if (JoyH > 0.7) uiJoy |= (uint)JOY.RIGHT;
                float JoyV = m_joystick.GetAxisPosition(JoystickAxis.Vertical);
                if (JoyV < 0.3) uiJoy |= (uint)JOY.UP;
                if (JoyV > 0.7) uiJoy |= (uint)JOY.DOWN;
                int nBtn = m_joystick.NumberOfButtons;
                if (nBtn >= 1 && m_joystick.GetButtonState(Setting.JoyButtonA) != 0) uiJoy |= (uint)JOY.BTN0;
                if (nBtn >= 2 && m_joystick.GetButtonState(Setting.JoyButtonB) != 0) uiJoy |= (uint)JOY.BTN1;
                if (nBtn >= 3 && m_joystick.GetButtonState(Setting.JoyButtonC) != 0) uiJoy |= (uint)JOY.BTN2;
                if (nBtn >= 4 && m_joystick.GetButtonState(Setting.JoyButtonD) != 0) uiJoy |= (uint)JOY.BTN3;
            }

            return uiJoy;
        }

        public uint GetJoystick()
        {
            return GetJoystick(0xffffffff);
        }
        public uint GetJoystick(uint uiJoy)
        {
            if (uiJoy == 0xffffffff)
            {
                uiJoy = GetJoystickRaw();
            }
            else
            {
                uiJoy &= 0xff;
            }
            uiJoy |= ((uiJoy ^ (m_uiJoystickValue & 0xff)) & uiJoy) << 8;
            uiJoy |= (((uiJoy ^ (m_uiJoystickValue & 0xff))) & (uiJoy ^ 0xff)) << 16;

            m_uiJoystickValue = uiJoy;

            return uiJoy;
        }

        public uint GetJoystickValue()
        {
            return m_uiJoystickValue;
        }



        public void ScoreUp(int nScore)
        {
            m_playinfo.m_nScore += nScore;
        }

    }




}
