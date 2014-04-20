using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using System.IO;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;

using NPTwins.Stage;


namespace NPTwins
{
    public class World
    {
        int m_nCount;
        Game m_game;

        Manager m_manager = new Manager();
        public Manager manager
        {
            get { return m_manager; }
        }

        public ActorPlayer[] m_actPlayer = new ActorPlayer[2];
        public int m_iCurrentPlayer;
        public ActorPlayer m_CurrentPlayer;
        public ActorPlayer m_SlavePlayer;
        public int m_nPlayChangeCounter;
        public int m_nFreeze;

        public LinkedList<Vector> m_listPlayerLink = new LinkedList<Vector>();

        public Manager m_mgrShot = new Manager();
        public Manager m_mgrEnemy = new Manager();
        public EShotManager     m_mgrEShot      = new EShotManager();
        public ParticleManager  m_mgrParticle   = new ParticleManager();
        public Manager m_mgrBG = new Manager();

        public ActorEnemy m_actBoss;

        uint m_uiJoy;
        public uint JoystickValue { get { return m_uiJoy; } }

        public JoyStickRecord m_joyrecord = new JoyStickRecord();

        // ステージ管理
        public StageManager m_stagemanager = new StageManager();

        Actor m_actBeforeEnemyGroupMessage;



        // ゲーム軽量化フラグ
        public bool m_fLightMode;


        public void Init(Game game)
        {
            m_game = game;

            m_manager.Init(m_game);

            m_actPlayer[0] = new ActorPlayer();
            m_actPlayer[0].Init(m_game);
            m_actPlayer[0].Pos = new Vector(240+40, 400,0);
            m_actPlayer[0].m_nNP = 0;

            m_actPlayer[1] = new ActorPlayer();
            m_actPlayer[1].Init(m_game);
            m_actPlayer[1].Pos = new Vector(240-40, 400, 0);
            m_actPlayer[1].m_nNP = 1;

            m_listPlayerLink.Clear();
            for ( int i=0; i<40; i ++ ) {
                m_listPlayerLink.AddLast(new Vector(m_actPlayer[0].Pos.X + (m_actPlayer[1].Pos.X - m_actPlayer[0].Pos.X) * i / 40,
                                                        m_actPlayer[0].Pos.Y + (m_actPlayer[1].Pos.Y - m_actPlayer[0].Pos.Y) * i / 40,
                                                        0 ) );
            }

            m_nFreeze = 0;

            SetCurrentPlayer(1);

            m_mgrShot.Init(m_game);
            m_mgrEnemy.Init(m_game);
            m_mgrEShot.Init(m_game);
            m_mgrParticle.Init(m_game);
            m_mgrBG.Init(m_game);


            m_stagemanager.Init(m_game);
            m_stagemanager.BuildStage();


            m_actBoss = null;

            m_game.GetJoystick(0);
            m_game.GetJoystick(0);
            m_game.GetJoystick(0);



            m_nCount = 0;
        }


        public void SetCurrentPlayer(int iNP)
        {
            m_iCurrentPlayer = iNP;
            m_CurrentPlayer = m_actPlayer[m_iCurrentPlayer];
            m_SlavePlayer = m_actPlayer[1 - m_iCurrentPlayer];
            m_nPlayChangeCounter = 50;

        }


        public void Action()
        {
            // ジョイスティック入力
            if (m_game.playinfo.m_fDemo)
            {
                m_uiJoy = m_joyrecord.Play() ;
            }
            else
            {
                m_uiJoy = m_game.GetJoystickValue();
                m_joyrecord.Record( m_uiJoy);
            }


            // チェンジ後の一時停止

            if (m_nFreeze > 0 )
            {
                m_nFreeze--;
                if (m_nFreeze < 29)
                {
                    if ((JoystickValue & (uint)Game.JOY.BTN1) == 0) m_nFreeze = 0;
                    return;
                }
            }

            // 通常進行
            m_nCount++;

            m_manager.Action();

            m_actPlayer[0].Action();
            m_actPlayer[1].Action();

            m_mgrShot.Action();
            m_mgrEnemy.Action();
            m_mgrEShot.Action();
            m_mgrParticle.Action();
            m_mgrBG.Action();




            // メッセージ出現
            if (m_nCount == 30)
            {
                PutMessageAsParticle("STAGE " + m_game.playinfo.m_nStage + " START", Color.White, new Vector(240, 200, 0), new Vector(0, -0.5, 0), 90);
            }
            if (m_nCount == 60)
            {
                PutMessageAsParticle("RANK: " + m_game.playinfo.GetRankName(), Color.White, new Vector(240, 220, 0), new Vector(0, -0.5, 0), 60);
            }


            // 敵出現
            m_stagemanager.ArrivalEnemy();
            m_stagemanager.PutBG(m_nCount);

            //

        }


        public Actor PutMessageAsParticle(String strMessage, Color color, Vector vPos, Vector vVect, int nKillCount)
        {
            return PutMessageAsParticle(strMessage, color, vPos, vVect, nKillCount, false);
        }

        public Actor PutMessageAsParticle(String strMessage, Color color, Vector vPos, Vector vVect, int nKillCount,bool fBig)
        {
            Actor act = new Actor();
            act.Init(m_game);
            act.Pos = vPos;
            act.Vect = vVect;
            act.VectVect = new Vector(0, 0, 0);
            act.Chr = (fBig ? m_game.res.font :  m_game.res.fontSmall).Render(strMessage, color);
            act.m_nKillCount = nKillCount;
            m_game.world.m_mgrParticle.Add(act);
            return act;
        }

        public Actor PutMessageAsParticle_EnemyGroup(String strMessage, Color color)
        {
            Vector vPos =  new Vector(240, 60, 0);
            if (m_actBeforeEnemyGroupMessage != null)
            {
                if (!m_actBeforeEnemyGroupMessage.m_fKill && m_actBeforeEnemyGroupMessage.Pos.Y > 16)
                {
                    vPos.Y = m_actBeforeEnemyGroupMessage.Pos.Y + 24;
                }
            }
            m_actBeforeEnemyGroupMessage =  PutMessageAsParticle(strMessage, color, vPos, new Vector(0, -0.5, 0), 90);
            return m_actBeforeEnemyGroupMessage;
        }


        public void Draw(Surface surface)
        {
            DrawStage(surface);
            if (m_CurrentPlayer.m_nDamagedCounter > 0 && m_game.GetRandom(100)>50 )
            {
                surface.Fill(Color.FromArgb(m_CurrentPlayer.m_nDamagedCounter *2, 0, 0));
            }

            m_mgrBG.Draw(surface);

            m_manager.Draw(surface);


            // プレイヤーリンクを描画
            if (m_CurrentPlayer.m_nLife > 0)
            {
                switch (m_game.playinfo.m_nControllMode)
                {
                    case 1:
                        {
                            Vector vBefore = m_listPlayerLink.First.Value;
                            foreach (Vector v in m_listPlayerLink)
                            {
                                if (vBefore != null)
                                {
                                    SdlDotNet.Graphics.Primitives.Line line = new SdlDotNet.Graphics.Primitives.Line((short)vBefore.X, (short)vBefore.Y, (short)v.X, (short)v.Y);
                                    line.Draw(surface, Color.Yellow);

                                }
                                vBefore = v;
                            }
                        }
                        break;
                    case 2:
                        {
                            SdlDotNet.Graphics.Primitives.Line line = new SdlDotNet.Graphics.Primitives.Line((short)m_CurrentPlayer.Pos.X, (short)m_CurrentPlayer.Pos.Y, (short)m_SlavePlayer.Pos.X, (short)m_SlavePlayer.Y);
                            line.Draw(surface, Color.Yellow);
                        }
                        break;
                }
            }

            m_SlavePlayer.Draw(surface);
            m_CurrentPlayer.Draw(surface);



            m_mgrShot.Draw(surface);
            m_mgrEnemy.Draw(surface);
            m_mgrEShot.Draw(surface);
            m_mgrParticle.Draw(surface);


            // サイド情報エリア

            if (m_nPlayChangeCounter > 0)
            {
                m_game.res.m_surSidePictBG.AlphaBlending = false;
                m_game.res.m_surSidePictBGBuf.Blit(m_game.res.m_surSidePictBG, new Point(0, 0));

                m_nPlayChangeCounter--;
                m_game.res.m_surSidePictBGBuf.Blit(m_game.res.m_surSidePict[1 - m_iCurrentPlayer], new Point(0 + (50 - m_nPlayChangeCounter) * 3, 0));
                m_game.res.m_surSidePictBGBuf.Blit(m_game.res.m_surSidePict[m_iCurrentPlayer], new Point(0 + m_nPlayChangeCounter * m_nPlayChangeCounter * 160 / 2500, 0));
                
                m_game.res.m_surSidePictBG.Alpha = 200;
                m_game.res.m_surSidePictBG.AlphaBlending = true;
                if (m_fLightMode)
                {
                    m_game.res.m_surSidePictBGBuf.Blit(m_game.res.m_surSidePictBG, new Point(0, 480 - (m_nPlayChangeCounter) * 20));
                }
                else
                {
                    m_game.res.m_surSidePictBGBuf.Blit(m_game.res.m_surSidePictBG, new Point(0, -(m_nPlayChangeCounter) * 10));
                }

            }
            m_game.res.m_surSidePictBGBuf.AlphaBlending = false;
            surface.Blit(m_game.res.m_surSidePictBGBuf, new Point(480, 0));




            surface.Blit(m_game.res.fontSmall.Render("SHIELD:", Color.Orange), new Point(490, 440));
            if (m_CurrentPlayer.m_nLife > 1 || m_nCount % 20 > 10)
            {
                for (int i = 0; i < m_CurrentPlayer.m_nLife; i++)
                {
                    surface.Blit(m_game.res.m_surGaugeLife, new Point(554 + i * 16, 440));
                }
            }
            for (int i = 0; i < m_SlavePlayer.m_nLife; i++)
            {
                surface.Blit(m_game.res.m_surGaugeLifeS, new Point(550 + i * 16, 460));
            }

            SdlDotNet.Graphics.Primitives.Box box = new SdlDotNet.Graphics.Primitives.Box(new Point(490, 418), new Size(m_CurrentPlayer.m_nSWCharge, 10));
            box.Draw(surface, Color.Orange,false,true);

            surface.Blit(m_game.res.fontSmall.Render("BOMB:" + m_CurrentPlayer.m_nBomb.ToString("0").PadLeft(3) +" %", Color.Orange), new Point(490, 370));

            surface.Blit(m_game.res.fontSmall.Render("CHARGE:" + m_CurrentPlayer.m_nSWCharge.ToString("0").PadLeft(3), Color.Orange), new Point(490, 400));

            surface.Blit(m_game.res.fontSmall.Render("SCORE:" + m_game.playinfo.m_nScore.ToString("00000000") , Color.Orange), new Point(490, 50));


            if (m_fLightMode)
            {
                surface.Blit(m_game.res.fontSmall.Render("INFO:" + m_mgrEnemy.GetCount().ToString("000") + "," + m_mgrEShot.GetCount().ToString("000") + "/" + m_stagemanager.GetEnemyLeft().ToString("0000") + " @" + m_nCount + "(" + m_nCount/60 + ")", Color.White), new Point(10, 10));
            }

            if (m_actBoss != null && m_actBoss.m_fKill == false )
            {
                surface.Blit(m_game.res.font.Render("BOSS LIFE:" + m_actBoss.m_nLife.ToString("00000000"), Color.RoyalBlue), new Point(20, 100));

            }


        }



        public void DrawStage(Surface surface)
        {
        }


        public void ScoreUp(int nScore , Vector vPos)
        {
            m_game.ScoreUp(nScore);
            Actor act = new Actor();
            act.Init(m_game);
            act.Pos = vPos;
            act.Vect = new Vector(0, -2, 0);
            act.VectVect = new Vector(0, 0.1, 0);
            act.Chr = m_game.res.fontSmall.Render("+" + nScore, Color.White);
            act.m_nKillCount = 20;
            m_game.world.m_mgrParticle.Add(act);


        }


        public void EraceEShot()
        {
            m_game.res.m_sounds["bomb"].Volume = 60;
            m_game.res.m_sounds["bomb"].Play();


            m_game.world.m_mgrEShot.ForEach((Actor actShot) => { 
                actShot.m_fKill = true;



                for (int i = 0; i < 2; i++)
                {
                    Actor act = new Actor();
                    act.Init(m_game);
                    act.Pos = actShot.Pos;
                    act.Vect = new Vector((double)(m_game.GetRandom(100) - 50) / 10, (double)(m_game.GetRandom(100) - 50) / 10, 0);
                    act.VectVect = new Vector(0, 0.1, 0);
                    act.Chr = m_game.res.m_surBombRing;
                    act.m_nKillCount = 20;
                    m_game.world.m_mgrParticle.Add(act);
                }

            });

        }





    }

}
