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



namespace NPTwins.Stage
{
    class SetStage02 : SetStage
    {
        public override void BulidStage()
        {
            // UTY変数
            int nRank = m_game.playinfo.m_nRank;
            float fRank = (float)nRank;

            jp.takel.PseudoRandom.MersenneTwister rand = new jp.takel.PseudoRandom.MersenneTwister(123);

            // コース
            CourseManager m_coursemanager = new CourseManager();
            m_coursemanager.Load("stagedata" + System.IO.Path.DirectorySeparatorChar +  "Stage02.xml");

            

            // 敵出現
            int nStart = 60 * 2;





            // ● First WAVE
            SetGroup("First Wave R", 1000);
            AddEnemyGorupStart(nStart, 10, 20, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse( m_coursemanager["U-Right"] );
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(2).MyCourse(course);
                act.m_aps.ShotTiming(100, 400 / nRank).Shot(new FireFuncs(fRank * 1.5f,(nRank/3)*2+1,8).FireAimPlayer);
            });
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["TurnDown"]);
                course.SetMirrorMode(-1);
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(nRank).MyCourse(course);
            });

            nStart += 3 * 60;

            SetGroup("First Wave L", 1000);
            AddEnemyGorupStart(nStart, 10, 20, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["U-Right"]);
                course.SetMirrorMode(-1);
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).MyCourse(course);
                act.m_aps.ShotTiming(100, 400 / nRank).Shot(new FireFuncs(fRank * 1.5f, (nRank / 3) * 2 + 1, 8).FireAimPlayer);
            });
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["TurnDown"]);
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(nRank).MyCourse(course);
            });

            nStart += 6 * 60;

            // ● Second WAVE
            SetGroup("Second Wave", 2000);
            AddEnemyGorupStart(nStart, 40, 15, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["U-Center"]);
                course.SetMirrorMode( (float)(20-i)/10  );
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(nRank).MyCourse(course);
            });
            AddEnemyGorupStart(nStart, 40, 15, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["U-Center"]);
                course.SetMirrorMode(-(float)(20 - i) / 10);
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(nRank).MyCourse(course);
            });


            SetGroup("Across", 2000);
            AddEnemyGorupStart(nStart + 2*60 , 10, 45, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["Across"]);
                act.m_aps.Img(IMG.ENEMY02).NP(1).Life(6).MyCourse(course).Angle(180);
                act.m_aps.ShotTiming(30, 200 / nRank).Shot(new FireFuncs( 2,nRank,20).FireFront);
            });
            AddEnemyGorupStart(nStart + 2 * 60, 10, 45, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["Across"]);
                course.SetMirrorMode(-1);
                act.m_aps.Img(IMG.ENEMY02).NP(0).Life(6).MyCourse(course).Angle(180);
                act.m_aps.ShotTiming(30, 200 / nRank).Shot(new FireFuncs( 2, nRank , 15).FireFront);
            });

            nStart += 10 * 60;



            // ● Formation
            SetGroup("Formation", 4000);
            for (int j = 0; j < 4; j++)
            {
                AddEnemyGorupStart(nStart + j * 20 , 20, 5, (ActorEnemy act, int i) =>
                {
                    MyCourse course = new MyCourse(m_coursemanager["FormationIn"]);
                    course.SetMirrorMode((float)(10 - i) / 10);
                    course.SetMulti(1, 0.3f + (float)j * 0.15f);
                    act.m_aps.Img(IMG.ENEMY01).NP(1).Life(10-j*2).MyCourse(course);
                    act.m_aps.ShotTiming(200 + i*10, (100+i*5) / nRank).Shot(new FireFuncs(fRank * 1.5f).FireFront);
                });
            }
            nStart += 10 * 60;




            // ● Gardian 
            SetGroup("Gardian", 2000);
            AddEnemyGorupStart(nStart, nRank*2, 1, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["CenterInStay"]);
                course.SetStartPos(new PointF(240 + (i-nRank) * 40, -32*4));
                act.m_aps.Img(IMG.ENEMY02).NP(0).Life(50).MyCourse(course).Angle(180);
                act.m_aps.ShotAlgo((ActorEnemy acte) => {
                    if (acte.m_nCount > 100 && acte.m_nCount % 50 < (5*nRank-3)) new FireFuncs(5 + acte.m_nCount % 50).FireAimPlayer(act);
                });

            });
            nStart += 3 * 60;


            // ● V 
            SetGroup("V-Assult", 1000);
            AddEnemyGorupStart(nStart, 30, 15, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["V-Assult"]);
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(2).MyCourse(course);

            });
            AddEnemyGorupStart(nStart, 30, 15, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["V-Assult"]);
                course.SetMirrorMode(-1);
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).MyCourse(course);

            });

            nStart += 9 * 60;


            // ● BackAttack
            SetGroup("BackAttack", 1000);
            AddEnemyGorupStart(nStart, 30, 8, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["BackAttack"]);
                act.m_aps.Img(IMG.ENEMY02).NP(0).Life(2).MyCourse(course).Angle(180);
                act.m_aps.ShotTiming(80-  i - nRank*10 , (100 + i * 5) / nRank).Shot(new FireFuncs(fRank * 1.5f,nRank,10).FireFront);

            });
            AddEnemyGorupStart(nStart, 30, 8, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["BackAttack"]);
                course.SetMirrorMode(-1);
                act.m_aps.Img(IMG.ENEMY02).NP(1).Life(2).MyCourse(course).Angle(180);
                act.m_aps.ShotTiming(80 - i - nRank * 10, (100 + i * 5) / nRank).Shot(new FireFuncs(fRank * 1.5f, nRank, 10).FireFront);

            });

            nStart += 7 * 60;


            // ● Uzu
            SetGroup("Uzu", 1000);
            for (int z = 0; z < 2; z++)
            {
                AddEnemyGorupStart(nStart, 10+10 * nRank, 5, (ActorEnemy act, int i) =>
                {
                    MyCourse course = new MyCourse(m_coursemanager["Uzu"]);
                    if ( z==1 ) course.SetMirrorMode(-1);
                    act.m_aps.Img(IMG.ENEMY01).NP(1-z).Life(4).MyCourse(course);
                    act.m_aps.ShotTiming(new IsShotFuncs(60 - i, (30 + i * 2) / nRank).IsShotInterval);
                    act.m_aps.Shot((ActorEnemy acte) =>{
                        m_game.world.m_mgrEShot.StartShot(IMG.ESHOT01, acte.Pos, new Vector(240, 285,0), -fRank * 1.5f);
                    } );
//                        new FireFuncs(fRank * 1.5f).FireAimPlayer);

                });

            }

            nStart += 5 * 60;


            // ● V-Rotate
            SetGroup("V-Rotate", 1000);
            AddEnemyGorupStart(nStart, 10 * nRank, 1, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["V"]);
                course.SetAngle(-20+i*2);
                course.SetMulti(1, 0.8f);
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(2).MyCourse(course);
                act.m_aps.ShotTiming(120, (200 + i * 9) / nRank).Shot(new FireFuncs(3 + i%5).FireUpper);

            });
            nStart += 2* 60;

            AddEnemyGorupStart(nStart, 10 * nRank, 1, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["V"]);
                course.SetMirrorMode(-1);
                course.SetAngle(-20+i*2);
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).MyCourse(course);
                act.m_aps.ShotTiming(120, (200 + i * 9) / nRank).Shot(new FireFuncs(3 + i % 5).FireUpper);

            });
            nStart += 5 * 60;


            SetGroup("V-Rotate2", 1000);
            AddEnemyGorupStart(nStart, 10 * nRank, 1, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["V"]);
                course.SetAngle( i * 2);
                course.SetMulti(1, 0.8f);
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(2).MyCourse(course);
                act.m_aps.ShotTiming(120, (100 + i * 9) / nRank).Shot(new FireFuncs(  i % 7).FireUpper);

            });

            AddEnemyGorupStart(nStart, 10 * nRank, 1, (ActorEnemy act, int i) =>
            {
                MyCourse course = new MyCourse(m_coursemanager["V"]);
                course.SetMirrorMode(-1);
                course.SetAngle( i * 2);
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).MyCourse(course);
                act.m_aps.ShotTiming(120, (100 + i * 9) / nRank).Shot(new FireFuncs(  i% 7).FireUpper);

            });
            nStart += 7 * 60;

            NoGroup();
            AddEnemyGorupStart(nStart, 10, 5, (ActorEnemy act, int i) =>
            {
                act.m_aps.Kill(5).EnterActionAlgo((ActorEnemy acte) =>
                {
                    m_game.world.PutMessageAsParticle("WARNING !", Color.Red, new Vector(240, 80 + 30 * i, 0), new Vector(0, 0, 0), 60,true);
                });
            });

            nStart += 1 * 60;


            // ● Boss
            ActorEnemy actBossCore = null;

            // ● Boss Phase 1
            SetGroup("Boss Phase 1", 2000);
            AddEnemyGorupStart(nStart, 5, 0, (ActorEnemy act, int i) =>
            {
                switch (i)
                {
                    case 0: // Core
                        {
                            MyCourse course = new MyCourse(m_coursemanager["CenterInStay"]);
                            act.m_aps.Img(IMG.ENEMY51).NP(0).Life(150).MyCourse(course).Angle(180).BossNo(1);
                            act.m_aps.ShotAlgo((ActorEnemy acte) =>
                            {
                                if (acte.m_nCount > 100 && acte.m_nCount % 50 < (3 * nRank - 2))
                                {
                                    m_game.world.m_mgrEShot.StartShotNway(IMG.ESHOT03, act.Pos, acte.m_nCount, 7, 30, 3 + acte.m_nCount % 50);
                                }
                            });
                            actBossCore = act;
                        }
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        {
                            act.m_aps.Img(IMG.ENEMY01).NP(0).Life(50).Pearent(actBossCore).Angle(0).AutoKillAtPearentDead(true);
                            act.m_aps.ShotTiming(new IsShotFuncs(100, 160-nRank*20, nRank*5, 4).IsShotInterval);
                            act.m_aps.Shot(new FireFuncs(4, 1, 0).FireFront);
                            act.m_aps.ActionAlgo((ActorEnemy acte) =>
                            {
                                acte.PearentOffsetPos = new Vector((float)((acte.m_nCount + i * 90) * Math.PI / 180)) * 64;
                                acte.m_nAngle = m_game.world.m_CurrentPlayer.CalcAngleFrom(acte);
                            });
                        }
                        break;
                }

            });
            nStart += 10;

            // ● Boss Phase 2
            SetGroup("Boss Phase 2", 2000);
            AddEnemyGorupStart(nStart, 7, 0, (ActorEnemy act, int i) =>
            {
                switch (i)
                {
                    case 0: // Core
                        {
                            MyCourse course = new MyCourse(m_coursemanager["CenterInStay2"]);
                            act.m_aps.Img(IMG.ENEMY51).NP(0).Life(200).MyCourse(course).Angle(180).BossNo(1);
                            act.m_aps.ShotAlgo((ActorEnemy acte) =>
                            {
                                if (acte.m_nCount > 100 && acte.m_nCount %  (120/ nRank ) == 0 )
                                {
                                    m_game.world.m_mgrEShot.StartShotNway(IMG.ESHOT02, act.Pos, acte.m_nCount, 360/15, 15, 3 );
                                }
                                if (acte.m_nCount > 60*10 && acte.m_nCount % 120 == 0)
                                {
                                    m_game.world.m_mgrEShot.StartShotNway(IMG.ESHOT01, act.Pos, m_game.world.m_CurrentPlayer.CalcAngleFrom(acte),  nRank*2,4, 4);
                                }
                            });
                            actBossCore = act;
                        }
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        {
                            act.m_aps.Img(IMG.ENEMY01).NP(0).Life(100).Pearent(actBossCore).Angle(0).AutoKillAtPearentDead(true);
                            act.m_aps.ShotTiming(new IsShotFuncs(100, 160 - nRank * 20, nRank * 10, 4).IsShotInterval);
                            act.m_aps.Shot(new FireFuncs(4, 1, 0).FireFront);
                            act.m_aps.ActionAlgo((ActorEnemy acte) =>
                            {
                                acte.PearentOffsetPos = new Vector((float)((acte.m_nCount + i * 60) * Math.PI / 180)) * 64;
                                acte.m_nAngle = acte.Pearent.CalcAngleFrom(acte) + 180;
                            });
                        }
                        break;
                }

            });


            nStart += 30 * 60;








            nStart += 60 * 10;
            NoGroup();




        }


        public override  void PutBG(int nCount)
        {
            // BGキャラ
            if (nCount % 4 == 0)
            {
                Actor act = new Actor();
                act.Init(m_game);
                act.Pos = new Vector(m_game.GetRandom(480), -m_game.GetRandom(10), 0);
                act.Vect = new Vector(0, 10, 0);
                act.Chr = m_game.res.m_surSekihou;

                m_game.world.m_mgrBG.Add(act);

            }

            if (nCount == 60 * 20)
            {
                Actor act = new Actor();
                act.Init(m_game);
                act.Pos = new Vector(m_game.GetRandom(480), -200, 0);
                act.Vect = new Vector(0, 0.1, 0);
                act.Chr = m_game.res.m_surMoon;
                act.Size = 200;
                m_game.world.m_mgrBG.Add(act);

            }

        }
    }
}
