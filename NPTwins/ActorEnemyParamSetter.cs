using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;

using NPTwins.SplineManager;

namespace NPTwins
{
    // 出現時のパラメーター設定楽チンクラス
    public class ActorEnemyParamSetter
    {
        protected ActorEnemy m_act;      // 対象となるActor

        // コンストラクタ
        public ActorEnemyParamSetter(ActorEnemy act)
        {
            m_act = act;
        }

        // パラメータセット
        public ActorEnemyParamSetter Img(IMG img )
        {
            m_act.m_img = img;
            return this;
        }

        public ActorEnemyParamSetter Size(int nSize)
        {
            m_act.Size = nSize;
            return this;
        }

        public ActorEnemyParamSetter NP(int nNP)
        {
            m_act.m_nNP = nNP;
            return this;
        }

        public ActorEnemyParamSetter Life(int nLife)
        {
            m_act.m_nLife = nLife;
            m_act.m_nScore = nLife * 10;
            return this;
        }
        public ActorEnemyParamSetter Kill(int nKill)
        {
            m_act.m_nKillCount = nKill;
            return this;
        }


        public ActorEnemyParamSetter Score(int nScore)
        {
            m_act.m_nScore = nScore;
            return this;
        }


        public ActorEnemyParamSetter MyCourse(MyCourse course)
        {
            m_act.m_course = course;
            PointF pt = course.GetNext();
            m_act.Pos = new Vector(pt.X, pt.Y, 0); 
            return this;
        }



        public ActorEnemyParamSetter CourseList(List<Vector> arrCourse)
        {
            m_act.m_arrVectCourse = arrCourse;
            m_act.Pos = arrCourse[0];
            return this;
        }


        public ActorEnemyParamSetter Pos(float x, float y)
        {
            m_act.Pos = new Vector(x, y, 0);
            return this;
        }
        public ActorEnemyParamSetter Vect(float x, float y)
        {
            m_act.Vect = new Vector(x, y, 0);
            return this;
        }
        public ActorEnemyParamSetter VectVect(float x, float y)
        {
            m_act.VectVect = new Vector(x, y, 0);
            return this;
        }
        public ActorEnemyParamSetter Freq(float freq)
        {
            m_act.Freq = freq;
            return this;
        }

        public ActorEnemyParamSetter Angle(int nAngle)
        {
            if (nAngle >= 0)
            {
                m_act.m_nAngle = nAngle;
                m_act.m_fAutoAngle = false;
            }
            else
            {
                m_act.m_fAutoAngle = true;
            }

            return this;
        }

        public ActorEnemyParamSetter BossNo(int nBossNo)
        {
            m_act.m_nBossNo = nBossNo;
            return this;
        }


        public ActorEnemyParamSetter Pearent(ActorEnemy act)
        {
            m_act.Pearent = act;
            return this;
        }
        public ActorEnemyParamSetter PearentOffsetPos(float dx, float dy)
        {
            m_act.PearentOffsetPos = new Vector(dx, dy, 0);
            return this;
        }

        public ActorEnemyParamSetter AutoKillAtPearentDead( bool fDead ) {
            m_act.m_fAutoKillAtPearentDead = fDead;
            return this;
        }


        public ActorEnemyParamSetter ShotTiming(int nStart, int nInterval)
        {
            return ShotTiming(nStart,  60 * 9999 , nInterval);
        }
        public ActorEnemyParamSetter ShotTiming(int nStart, int nEnd , int nInterval )
        {
            m_act.m_delegateIsShot = new IsShotFuncs(nStart, nEnd, nInterval).IsShotInterval;

            return this;
        }

        public ActorEnemyParamSetter ShotTiming(IsShot isshot)
        {
            m_act.m_delegateIsShot = isshot;
            return this;
        }

        public ActorEnemyParamSetter Shot(CHR chrPad, float fSpeed)
        {
            /*
            m_act.m_chrShot = chrPad;
            m_act.m_fShotSpeed = fSpeed;
            */

            m_act.m_delegateFire = new FireFuncs(fSpeed).FireAimPlayer;
            return this;
        }
        public ActorEnemyParamSetter Shot(Fire fire)
        {
            m_act.m_delegateFire = fire;
            return this;
        }

        public ActorEnemyParamSetter ShotAlgo(ShotAlgo shotalgo)
        {
            m_act.m_delegateShotAlgo += shotalgo;
            return this;
        }

        public ActorEnemyParamSetter ActionAlgo(EnemyAction action)
        {
            m_act.m_delegateAction += action;
            return this;
        }
        public ActorEnemyParamSetter EnterActionAlgo(EnemyAction action)
        {
            m_act.m_delegateEnter += action;
            return this;
        }
        public ActorEnemyParamSetter DamagedActionAlgo(EnemyAction action)
        {
            m_act.m_delegateDamaged += action;
            return this;
        }
        public ActorEnemyParamSetter BreakActionAlgo(EnemyAction action)
        {
            m_act.m_delegateBreak += action;
            return this;
        }

    }


    public class IsShotFuncs
    {
        int nStart, nEnd, nInterval;
        int nBurstTime ,  nBurstInterval;

        public IsShotFuncs(int nStart, int nInterval) : this(nStart, 9999, nInterval, 1, 1) {}
        public IsShotFuncs(int nStart, int nEnd, int nInterval) : this(nStart, nEnd, nInterval, 1, 1) {}
        public IsShotFuncs(int nStart, int nInterval, int nBurstTime, int nBurstInterval) : this(nStart, 9999, nInterval, nBurstTime, nBurstInterval) { }

        public IsShotFuncs(int nStart, int nEnd, int nInterval , int nBurstTime , int nBurstInterval)
        {
            this.nStart = nStart;
            this.nEnd  = nEnd;
            this.nInterval = nInterval;
            this.nBurstTime = nBurstTime;
            this.nBurstInterval = nBurstInterval;
        }

        public bool IsShotInterval(ActorEnemy act)
        {
            return (act.m_nCount >= nStart && act.m_nCount <= nEnd 
                    && (act.m_nCount - nStart) % nInterval <nBurstTime
                    && ((act.m_nCount - nStart) % nInterval) % nBurstInterval == 0 );
        }
    }

    public class FireFuncs
    {
        float m_fShotSpeed;
        int m_nWay;
        int m_nAngleDelta;

        public FireFuncs(float fShotSpeed) : this(fShotSpeed, 1, 0) { }
        public FireFuncs(float fShotSpeed,int nWay,int nAngleDelta)
        {
            m_fShotSpeed = fShotSpeed;
            m_nWay = nWay;
            m_nAngleDelta = nAngleDelta;
        }

        public void FireAimPlayer(ActorEnemy act ) {
            ActorEShot[] acts = act.m_game.world.m_mgrEShot.StartShotNway(IMG.ESHOT01, act.Pos, act.m_game.world.m_CurrentPlayer.CalcAngleFrom(act),m_nWay,m_nAngleDelta, m_fShotSpeed);
            foreach ( ActorEShot acte in acts ){ 
                acte.m_aps.ActionAlgo((ActorEnemy actx) => { actx.m_nAngle += 5; });
            }
        }

        public void FireFront(ActorEnemy act)
        {
            act.m_game.world.m_mgrEShot.StartShotNway(IMG.ESHOT02, act.Pos, act.m_nAngle, m_nWay,m_nAngleDelta,m_fShotSpeed);
        }

        public void FireUpper(ActorEnemy act)
        {
            act.m_game.world.m_mgrEShot.StartShot(IMG.ESHOT03, act.Pos, (act.m_nCount % 10) - 5, m_fShotSpeed).m_aps.VectVect(0, 0.1f).Freq(0.995f);
        }


    }

}
