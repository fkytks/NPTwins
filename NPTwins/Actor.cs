using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

using System.IO;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;

namespace NPTwins
{
    public class Actor
    {
        public Game m_game;


        Manager.ACTOR m_eActorKind;
        public Manager.ACTOR kind
        {
            get { return m_eActorKind; }
            set { m_eActorKind = value; }
        }
        public int m_nCount;


        public bool m_fKill;
        public int m_nKillCount;

        public bool m_fInScreen;    // １度画面内にはいった

        // ゲーム独自要素
        public int m_nNP;           // 属性 0:NEGA 1:POSI
        public int m_nLife;
        public int m_nDamagedCounter;


        // イメージ
        public IMG m_img;           // 表示イメージID


        // 親子関係
        Actor m_actPearent;
        Vector m_vPearentOffsetPos;
        public Actor Pearent { get { return m_actPearent; } set { m_actPearent = value; } }
        public Vector PearentOffsetPos { get { return m_vPearentOffsetPos; } set { m_vPearentOffsetPos = value; } }
        public bool m_fAutoKillAtPearentDead;

        // 座標
        Vector m_vPos ;
        Vector m_vVect;
        Vector m_vVectVect;
        float m_fFreq;  // 減速率
        float m_fSize;  // オブジェクト半径（当たり判定用）

        public Vector Pos
        {
            get { return m_vPos; }
            set { m_vPos = value; }
        }
        public Vector Vect
        {
            get { return m_vVect; }
            set { m_vVect = value; }
        }
        public Vector VectVect
        {
            get { return m_vVectVect; }
            set { m_vVectVect = value; }
        }
        public float X
        {
            get { return m_vPos.X; }
            set { m_vPos.X = value; }
        }
        public float Y
        {
            get { return m_vPos.Y; }
            set { m_vPos.Y = value; }
        }
        public float Freq { get { return m_fFreq; } set { m_fFreq = value; } }
        public float Size { get { return m_fSize; } set { m_fSize = value; } }


        protected Surface m_surChr ;
        public Surface Chr { set { m_surChr = value; } }



        public virtual void Init(Game game)
        {
            m_game = game;

            m_fKill = false;
            m_nKillCount = 0;
            m_fInScreen = false;
            m_img = IMG.NONE;
            m_fFreq = 1.0f;
            m_fSize = 16;
            m_surChr = null;
            m_nNP = 0;
            m_nCount = 0;
            m_fAutoKillAtPearentDead = false;
        }


        public virtual void Action()
        {
            m_nCount++;
            ActionCore();
        }


        public virtual void ActionCore()
        {
            if (m_actPearent != null ) { 
                if (!m_actPearent.m_fKill)
                {
                    // 親オブジェクトに従属
                    Vector vNewPos = m_actPearent.Pos + m_vPearentOffsetPos;
                    Vect = vNewPos - Pos;
                }
                else{
                    if (m_fAutoKillAtPearentDead) m_fKill = true;
                }
            }
            else
            {
                m_vVect += m_vVectVect;
            }

            m_vPos += m_vVect;
            m_vVect *= m_fFreq;

            if (m_nDamagedCounter > 0) m_nDamagedCounter--;
            if (m_nKillCount > 0)
            {
                m_nKillCount--;
                if ( m_nKillCount==0 ) m_fKill = true;
            }

            if (IsInScreen())
            {
                m_fInScreen = true;
            }else
            {
                if ( m_fInScreen ) m_fKill = true;  // 画面外で消滅
            }
        }

        public virtual void Draw(Surface surface)
        {
            if (m_surChr != null)
            {
                surface.Blit(m_surChr, new Point((int)X - m_surChr.Width / 2, (int)Y - m_surChr.Height / 2));

            }
        }




        public virtual bool IsInScreen()
        {
            return (X > -30 - m_fSize && X < 500 + m_fSize && Y > -20 - m_fSize && Y < 500 + m_fSize);
        }

        public virtual bool IsMovable(int xv,int yv)
        {
            bool fOK = true;

            return fOK;
        }

        public virtual void Move(int xv, int yv)
        {
            X += xv;
            Y += yv;
        }


        public virtual float CalcDist(Vector vPos)
        {
            return (m_vPos - vPos).Length;
        }
        public virtual float CalcDist(Actor act)
        {
            return CalcDist(act.Pos);
        }


        public virtual bool IsHit( Vector vPos , float fSize )
        {
            return CalcDist(vPos) < m_fSize + fSize;
        }
        public virtual bool IsHit(Actor act)
        {
            return IsHit( act.Pos, act.m_fSize);
        }

        public int CalcAngleFrom(Vector vPos)
        {
            return  ((int)(Pos-vPos).DirectionDeg + 90) % 360;
        }
        public int CalcAngleFrom(Actor act)
        {
            return CalcAngleFrom(act.Pos);
        }

        public virtual void Damage()
        {
            if (m_nLife > 0)
            {
                m_game.res.m_sounds["damaged"].Play();
                m_nLife--;
            }
        }
        public virtual void Damage(int nNP, int nPower , Vector vPos)
        {
            Damage();
        }


    }

}
