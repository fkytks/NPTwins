using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using System.IO;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;

namespace NPTwins
{
    public class ActorPlayer : Actor
    {
        bool m_fDead;
        public bool IsDead() { return m_fDead; }

        public int m_nSWCharge;    // 合体兵器エネルギー
        public int m_nBomb;         // BOMB残数


        public override void Init(Game game)
        {
            base.Init(game);

            Size = 1;
            m_nLife = 3;
            m_nDamagedCounter = 0;

            m_nSWCharge = 50;
            m_nBomb = 100;

            m_fDead = false;
        }



        public override void ActionCore()
        {

            if ( m_fDead ) {

                return;
            }

            bool fMaster = (m_game.world.m_CurrentPlayer == this);


            ActorPlayer actOther = m_game.world.m_actPlayer[1 - m_nNP];

            uint uiJoy = m_game.world.JoystickValue;
            if (m_game.world.m_CurrentPlayer.m_nLife > 0)
            {
                if (fMaster)
                {

                    float xv = 0, yv = 0;


                    if ((uiJoy & (uint)Game.JOY.LEFT) != 0)
                    {
                        xv = -1;
                    }
                    if ((uiJoy & (uint)Game.JOY.RIGHT) != 0)
                    {
                        xv = 1;
                    }
                    if ((uiJoy & (uint)Game.JOY.UP) != 0)
                    {
                        yv = -1;
                    }
                    if ((uiJoy & (uint)Game.JOY.DOWN) != 0)
                    {
                        yv = 1;
                    }

                    float fSpeed = 4;
                    if ((uiJoy & (uint)Game.JOY.BTN3) != 0) fSpeed = 2;
                    Vect = new Vector(xv, yv, 0) * fSpeed;


                    if (Vect.Length > 0 || (uiJoy & (uint)Game.JOY.BTN0) == 0)
                    {
                        if (m_nNP == 0)
                        {
                            m_game.world.m_listPlayerLink.AddFirst(Pos);
                            m_game.world.m_listPlayerLink.RemoveLast();
                        }
                        else
                        {
                            m_game.world.m_listPlayerLink.AddLast(Pos);
                            m_game.world.m_listPlayerLink.RemoveFirst();
                        }
                    }


                    if ((uiJoy & (uint)Game.JOY.NEW_BTN1) != 0 && m_game.world.m_nFreeze==0 )
                    {
                        m_game.world.SetCurrentPlayer(1 - m_nNP);
                        m_game.world.m_nFreeze = 30;
                        // m_game.GetJoystick();
                        m_game.res.m_sounds["change"].Play();
                    }

                }
                else
                {
                    switch (m_game.playinfo.m_nControllMode)
                    {
                        case 1:
                            {
                                // TYPE-T
                                Vect *= 0;
                                if (m_nNP == 1)
                                {
                                    Pos = m_game.world.m_listPlayerLink.Last.Value;
                                }
                                else
                                {
                                    Pos = m_game.world.m_listPlayerLink.First.Value;
                                }
                            }
                            break;
                        case 2:
                            {
                                // TYPE-R
                                Freq = 0.99f;
                                // Vect += (m_game.world.m_CurrentPlayer.Pos - Pos).Normalized() * 0.05f;

                                float fDist = (m_game.world.m_CurrentPlayer.Pos - Pos).Length;
                                if (fDist > 125)
                                {
                                    Vector vPosBefore = Pos;
                                    Pos = m_game.world.m_CurrentPlayer.Pos + (Pos - m_game.world.m_CurrentPlayer.Pos).Normalized() * 125f;
                                    Vect += Pos - vPosBefore;
                                }

                                if ((uiJoy & (uint)Game.JOY.BTN0) != 0) Freq = 0.95f;

                            }
                            break;

                        case 3:
                            {
                                // TYPE-S
                                Freq = 0.99f;
                                if (m_game.world.m_CurrentPlayer.Pos != Pos)
                                {
                                    Vect += (m_game.world.m_CurrentPlayer.Pos - Pos).Normalized() * 0.05f;
                                }
                            }
                            break;
                        case 4:
                            {
                                // TYPE-F
                                Vect *= 0;
                            }
                            break;
                    }

                }
            }
            else
            {
                // player dead
                uiJoy = 0;
            }


            // ショット
            if ((uiJoy & (uint)Game.JOY.NEW_BTN0) != 0
                || ( (uiJoy & (uint)Game.JOY.BTN0) != 0 && (m_nCount % ( fMaster ? 3 : 3) == 0) ) )
            {
                int nNP = m_nNP;
                int nPow = 1;

                bool fLink = false;

                if (Math.Abs(actOther.X - X) < 32)
                {
                    nPow = 2;
                    if (!fMaster) nNP = 1 - nNP;
                    // if (actOther.Y < Y) nNP = 1 - nNP;
                    if (Math.Abs(actOther.Y - Y) < 32)
                    {
                        fLink = true;
                    }
                    else
                    {
                    }
                }
                if (!fLink || fMaster || true )
                {
                    ((ActorShot)m_game.world.m_mgrShot.Add(new ActorShot())).Init(m_game, Pos, new Vector(0, -15, 0), nNP, nPow);
                }
                if (fLink && m_nSWCharge>0)
                {
                //    ((ActorShot)m_game.world.m_mgrShot.Add(new ActorShot())).Init(m_game, Pos, new Vector(-5, -13, 0), nNP, nPow);
                //    ((ActorShot)m_game.world.m_mgrShot.Add(new ActorShot())).Init(m_game, Pos, new Vector(5, -13, 0), nNP, nPow);
                    ((ActorShot)m_game.world.m_mgrShot.Add(new ActorShot())).Init(m_game, Pos, new Vector(-3, -10, 0), nNP, 1);
                    ((ActorShot)m_game.world.m_mgrShot.Add(new ActorShot())).Init(m_game, Pos, new Vector(3, -10, 0), nNP, 1);

                    ((ActorShot)m_game.world.m_mgrShot.Add(new ActorShot())).Init(m_game, Pos, new Vector(0, 15, 0), nNP, nPow);


                    AddSWCharge(-2);
                }

                if (fMaster && (uiJoy & (uint)Game.JOY.NEW_BTN0) != 0)
                {
                    m_game.res.m_sounds["shot"].Play();

                }

            }



            // BOMB
            if (fMaster && (uiJoy & (uint)Game.JOY.NEW_BTN2) != 0 && m_nBomb >=100)
            {
                m_nBomb-=100;
                m_game.world.EraceEShot();

                m_nDamagedCounter = 60;
            }

            // チャージ
            if (fMaster && m_nCount % 12 == 0) AddSWCharge(1);


//            m_surChr = m_game.res.m_surPlayer[m_nNP];
            m_surChr = m_game.res[IMG.PLAYER][fMaster ? 0: 1, 0, m_nNP];

/*            if ( !fMaster)
            {
                if (m_nCount % 4 != 0) m_surChr = null;
            }
            */

            if (m_nDamagedCounter > 0 )
            {
                if (m_nCount % 2 == 0) m_surChr = null;
            }

            if (m_game.world.m_CurrentPlayer.m_nLife == 0)
            {
                Vect += new Vector(0, 0.8, 0);
            }


            base.ActionCore();

            if (this.m_fKill)
            {
                Debug.Write("Player Killed");
                Debug.WriteLine("A pos=" + X + "," + Y + "   V=" + Vect);
            }

            if (m_game.world.m_CurrentPlayer.m_nLife > 0)
            {
                if (X > 480-16) X = 480-16;
                if (X < 16) { X = 17; Vect *= new Vector(0f,1f,0f); }
                if (Y > 480 - 16)
                {
                    Y = 480 - 16;
                }
                if (Y < 20) { Y = 21; Vect *= new Vector(1f, 0f, 0f); }
            }
        }


        public override void Draw(Surface surface)
        {
            bool fMaster = (m_game.world.m_CurrentPlayer == this);
            if (fMaster)
            {
//                m_game.res.m_surRing.AlphaBlending = true;
//                m_game.res.m_surRing.Alpha = (byte)(Math.Sin((double)m_nCount / 100) * 128 + 128);
//              if ((Math.Sin((double)m_nCount / 60) * 2 + 2) < (m_nCount % 5))

                if (m_game.world.m_nFreeze == 0)
                {
                    int nBright = m_nSWCharge / 20;
                    if (m_game.world.m_nPlayChangeCounter > 40) nBright = 5;
                    if (nBright >= (m_nCount % 7))
                    {
                        surface.Blit(m_game.res.m_surRing, new Point((int)X - m_game.res.m_surRing.Width / 2, (int)Y - m_game.res.m_surRing.Height / 2));
                    }
                }
                else
                {
                    if (m_game.world.m_nFreeze % 4 != 0)
                    {
                        surface.Blit(m_game.res.m_surRing, new Point((int)X - m_game.res.m_surRing.Width / 2, (int)Y - m_game.res.m_surRing.Height / 2));
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        Vector vMkPos = Pos + new Vector(-90 + m_game.world.m_nFreeze * 5 + i * 120) * m_game.world.m_nFreeze*2;
                        surface.Blit(m_game.res.m_surMarker, new Point((int)vMkPos.X - m_game.res.m_surMarker.Width / 2, (int)vMkPos.Y - m_game.res.m_surMarker.Height / 2));
                    }

                }
            }
            base.Draw(surface);
        }


        public override void Damage()
        {
            Damage(0, 1, Pos);
        }
        public void Damage(Actor actShot)
        {
            Damage(actShot.m_nNP, 1, actShot.Pos);
        }
        public override void Damage(int nNP, int nPower, Vector vPos)
        {
            if (m_nDamagedCounter == 0)
            {
                if (m_nLife > 0)
                {
                    m_game.res.m_sounds["damaged"].Play();
                    m_nLife--;
                    m_nDamagedCounter = 60 * 2;

                    m_game.world.m_mgrParticle.AddBurst(20, Pos, new Vector(0, 0.1 * 10, 0), m_game.res.m_surRing);
                }
            }

        }



        public void AddSWCharge(int n)
        {
            m_nSWCharge += n;
            if (m_nSWCharge > 100) m_nSWCharge = 100;
            if (m_nSWCharge < 0) m_nSWCharge = 0;

            if (n > 4)
            {
                if ( m_nBomb<200 ) m_nBomb += 1;
            }
        }

    }
}
