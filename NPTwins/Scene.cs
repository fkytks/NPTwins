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
    public class Scene
    {
        protected Game m_game;          // ゲーム管理クラスへの参照
        protected int m_nCount;         // ソーン開始からのカウント


        public Scene()
        {
        }

        public virtual void Init(Game game)
        {
            m_game = game;
            m_nCount = 0;
        }

        public virtual void Release()
        {
        }


        public virtual void Action()
        {
            m_nCount++;
        }

        public virtual void Draw(Surface surface)
        {

            DrawCore(surface);
//            surface.Blit(m_game.res.fontSmall.Render("COUNT:" + m_nCount, Color.Gray), new Point(20, 460));


        }

        public virtual void DrawCore(Surface surface)
        {
            surface.Fill(Color.FromArgb(0, 0, 0));
        }

        public virtual void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
        }

        public virtual void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        public virtual void Events_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
        public virtual void Events_MouseMotion(object sender, MouseMotionEventArgs e)
        {
        }




    }
}
