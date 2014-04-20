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

using NPTwins.SplineManager;

namespace NPTwins
{

    public class EnemyGroup
    {
        public String m_strName;
        public int m_nCount;
        public int m_nBreak;
        public int m_nScore;
        public int m_nItem;

        public bool m_fAlreadyStart;
        public bool m_fHitSameNP;


        public void Init(String strName)
        {
            m_strName = strName;
            m_nCount = 0;
            m_nBreak = 0;
            m_nScore = 0;
            m_nItem = 0;

            m_fAlreadyStart = false;
            m_fHitSameNP = false;
        }
    }


    public delegate void Fire(ActorEnemy act);
    public delegate bool IsShot(ActorEnemy act);
    public delegate void ShotAlgo(ActorEnemy act);
    public delegate void EnemyAction(ActorEnemy act);


    public class ActorEnemy : Actor
    {
        public ActorEnemyParamSetter m_aps;


//        public int m_nPat;
        public int m_nAngle;
        public bool m_fAutoAngle;

/*        // ショットタイミング・ショット
        public int m_nShotStart;
        public int m_nShotEnd;
        public int m_nShotInterval;
        public CHR m_chrShot;
        public float m_fShotSpeed;
*/
        // ショットDelegate
        public IsShot m_delegateIsShot;
        public Fire m_delegateFire;
        public ShotAlgo m_delegateShotAlgo;

        // 挙動Delegate
        public EnemyAction m_delegateAction;
        public EnemyAction m_delegateEnter;
        public EnemyAction m_delegateDamaged;
        public EnemyAction m_delegateBreak;


        public int m_nDamageRatio;      // ダメージをうける割合（0～100 : 破壊不能壊れない , 0～99:親オブジェクトにダメージ伝播）

        public int m_nScore;
        public bool m_fHitSameNP; // 同じ属性の弾に当たったか？（減点）
        public int m_nBossNo;


        public MyCourse m_course;

        public List<Vector> m_arrVectCourse;

        public EnemyGroup m_group;


        public override void Init(Game game)
        {
            base.Init(game);
            m_aps = new ActorEnemyParamSetter(this);

            Size = 16;
            m_nLife = 1;

            m_nAngle = 0;
            m_fAutoAngle = true;
            m_nDamageRatio = 100;

            /*          
            m_nShotStart = 60 * 9999;    // don't shot
            m_nShotEnd   = 60 * 9999;    // never end
            m_nShotInterval = 60 * 5;
            m_fShotSpeed = 1.0f;
            */
            m_nScore = 10;
            m_fHitSameNP = false;
        }

        public void Init(Game game, IMG img, int nNP, int nLife, Vector vPos, Vector vVect, Vector vVectVect, int nAngle, int nShotStart, int nShotInterval, float fShotSpeed, List<Vector> arrVectCourse)
        {
            Init(game);

            m_img = img;
            Size = m_game.res[m_img][nNP].Width / 2;
            // m_nPat = nPat;
            m_nNP = nNP;
            m_nLife = nLife;
            Pos = vPos;
            Vect = vVect;
            VectVect = vVectVect;
            m_nAngle = nAngle; if (nAngle == -1) m_fAutoAngle = true;

            m_delegateIsShot = new IsShotFuncs(nShotStart, 60 * 9999, nShotInterval).IsShotInterval;
            m_delegateFire = new FireFuncs(fShotSpeed).FireAimPlayer;
            /*
            m_nShotStart = nShotStart;
            m_nShotInterval = nShotInterval;
            m_fShotSpeed = fShotSpeed;
            */

            m_nScore = m_nLife * 10;
            m_arrVectCourse = arrVectCourse;
        }


        public override void ActionCore()
        {


            if (m_delegateAction != null) m_delegateAction(this);

            if (m_course != null)
            {
                if (m_course.IsValid())
                {
                    Vector vPosBefore = Pos;
                    PointF pt = m_course.GetNext();
                    Vector vPosNext = new Vector(pt.X, pt.Y, 0);
                    Vect = vPosNext - vPosBefore;
                }
                else
                {
                    m_fKill = true;
                }
            }


            if (m_arrVectCourse != null)
            {
                if (m_arrVectCourse.Count > m_nCount)
                {

                    if (m_nCount > 1) Vect = m_arrVectCourse[m_nCount] - m_arrVectCourse[m_nCount-1];
                    Pos = m_arrVectCourse[m_nCount];

                }
            }


            PlayerHitCheck();

            if ( m_delegateIsShot!=null && m_delegateIsShot(this) )
            {
                if (m_delegateFire != null) m_delegateFire(this);
            }

            if (m_delegateShotAlgo != null) m_delegateShotAlgo(this);

            

            int nDispNP = m_nNP;
            if (m_nDamagedCounter > 0 && m_nCount % 4 <2 )
            {
                nDispNP = 1 - nDispNP;
            }
            if (m_fAutoAngle)
            {
                m_nAngle = ((int)Vect.DirectionDeg+90) % 360;
            }

            // 表示イメージ設定
            if ( m_img != IMG.NONE )  Chr = m_game.res[m_img][0,m_nAngle,nDispNP];

            base.ActionCore();

        }

        public virtual void PlayerHitCheck()
        {
            ActorPlayer actPlayer = m_game.world.m_CurrentPlayer;
            if (actPlayer.IsHit(this))
            {
                actPlayer.Damage(this);
            }
        }


        public override  bool IsInScreen()
        {
            bool fIn = base.IsInScreen();
            if (VectVect.Y > 0 && Y < 500 + Size && Y>-200 ) fIn = true;
            if (m_course != null && !m_course.IsEOP()) fIn = true;

            return fIn;
        }

        public override void Draw(Surface surface)
        {
            base.Draw(surface);
        }


        public void Damage(ActorShot actShot)
        {
            Damage(actShot.m_nNP, actShot.m_nPower, actShot.Pos);
        }

        public override void Damage(int nNP , int nPower , Vector vPos)
        {

            if (!m_fKill)
            {
                if (m_delegateDamaged != null) m_delegateDamaged(this);

                int nDamage = nPower;
                if (m_nNP != nNP)
                {
                    nDamage *= 2;
                }
                else
                {
                    m_fHitSameNP = true;
                    nDamage = 1;
                }
                nDamage = nDamage * m_nDamageRatio / 100;
                
                

                m_nLife -= nDamage;
                if (m_nLife <= 0)
                {
                    // 敵破壊
                    m_game.world.ScoreUp(m_nScore * (m_fHitSameNP ? 1 : 2),Pos);

                    if (m_delegateBreak != null) m_delegateBreak(this);


                    m_game.res.m_sounds["bomb"].Stop();
                    int n = m_game.res.m_sounds["bomb"].NumberOfChannels;
                    int n2 = Mixer.NumberOfChannelsPlaying();
                    if (n2 < 6)
                    {
                        m_game.res.m_sounds["bomb"].Volume = 40;
                        m_game.res.m_sounds["bomb"].Play();
                    }
                    m_fKill = true;


                    if (m_group != null)
                    {
                        if (m_fHitSameNP) m_group.m_fHitSameNP = m_fHitSameNP;
                        m_group.m_nBreak++;
                        if (m_group.m_nCount == m_group.m_nBreak)
                        {
                            if (!m_fHitSameNP)
                            {
                                // x2
                                m_game.ScoreUp(m_group.m_nScore*2);
                                m_game.world.PutMessageAsParticle_EnemyGroup("formation [" + m_group.m_strName + "] Perfect Destory!  +" + m_group.m_nScore + " x2", Color.Magenta);
                            }
                            else
                            {
                                m_game.ScoreUp(m_group.m_nScore);
                                m_game.world.PutMessageAsParticle_EnemyGroup("formation [" + m_group.m_strName + "] Complete Destory!  +" + m_group.m_nScore, Color.White);
                            }
                        }
                    }

                    m_game.world.m_mgrParticle.AddBurst(10, Pos, Vect, m_game.res[IMG.BOMBRING][0]);

                }
                else
                {
                    if ( nDamage>0 )  m_nDamagedCounter = 10;


                    m_game.world.m_mgrParticle.AddSpark(nDamage, vPos);


                    int n2 = Mixer.NumberOfChannelsPlaying();
                    if (n2 < 6)
                    {
                        m_game.res.m_sounds["edamaged"].Volume = 80;
                        m_game.res.m_sounds["edamaged"].Play();
                    }

                }
            }


        }

    }




}
