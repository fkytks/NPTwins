using System;
using System.Collections.Generic;
using System.Text;



using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;



namespace NPTwins.Stage
{
    class SetStage01 : SetStage
    {
        public override void BulidStage()
        {
            /*
            AddEnemyStart( 30,nStart,10 , delegate(ActorEnemy act,int i) {
                act._Pos(320,0)._Vect(0,1)._Shot(60*3,10,1)._Shot( delegate(ActorEnemy act) { act.Shot(act.Pos,i); } )._Move(arrCourse2)._Pat(1);
            };
            */

            int nRank = m_game.playinfo.m_nRank;
            float fRank = (float)nRank;

            jp.takel.PseudoRandom.MersenneTwister rand = new jp.takel.PseudoRandom.MersenneTwister(123);

            // 敵出現
            int nStart = 60 * 2;

            List<Vector> arrCourse2 = Uty.CreateSpline(new Vector[] { new Vector(380, -50, 0), new Vector(380, -30, 0), new Vector(380, 300, 0), new Vector(340, 300, 0), new Vector(340, 200, 0), new Vector(340, -100, 0) }, 400);
            List<Vector> arrCourse2R = Uty.MirrorVectors(arrCourse2);

            List<Vector> arrCourse5 = Uty.CreateSpline(new Vector[] { new Vector(500, 100, 0), new Vector(500, 100, 0), new Vector(300, 100, 0), new Vector(100, 100, 0), new Vector(100, 200, 0), new Vector(300, 200, 0), new Vector(500, 200, 0), new Vector(700, 200, 0) }, 300);
            List<Vector> arrCourse5R = Uty.MirrorVectors(arrCourse5);

            List<Vector> arrCourse6 = Uty.CreateSpline(new Vector[] { new Vector(320, -50, 0), new Vector(320, 0, 0), new Vector(320, 100, 0), new Vector(320 + 20, 100 + 20, 0), new Vector(320 - 20, 100 + 20, 0), new Vector(320 - 20, 100 - 20, 0), new Vector(320 + 20, 100 - 20, 0), new Vector(320, -100, 0) }, 1000);
            List<Vector> arrCourse6R = Uty.MirrorVectors(arrCourse6);

/*            
            AddEnemyGorupStart(nStart, 10, 15, (ActorEnemy act, int i) =>
            {
                act.m_aps.Pat((int)CHR.TRACER).NP(1).Course(arrCourse5R)
                    .ShotAlgo((ActorEnemy acte) => { if (new IsShotFuncs(30, 90, i * 5 + 5).IsShotInterval(act)) new FireFuncs(i + 1).FireAimPlayer(act); })
                    .ShotAlgo((ActorEnemy acte) => { if (new IsShotFuncs(100, 130, 5).IsShotInterval(act)) new FireFuncs(-i-1).FireAimPlayer(act); });
            });
            nStart += 60 * 4;

            SetGroup("First Wave R", 200);
            AddEnemyGorupStart(nStart, 10, 15, (ActorEnemy act, int i) =>
            {
                act.m_aps.Pat((int)CHR.TRACER).Pos(10 + i * 40, 10).Angle(90).Vect(0, 1).NP(i % 2).Course(arrCourse5).ShotTiming(30,90,i*5+5).Shot( new FireFuncs(i+1).FireAimPlayer );
            });
            nStart += 60*4;
*/


            // ● First WAVE R
            SetGroup("First Wave R", 200);
            AddEnemyGorupStart(nStart, 10, 15, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).CourseList(arrCourse2);
            });

            if (nRank > 1)
            {
                SetGroup("First Wave R2", 200);
                AddEnemyGorupStart(nStart, 10, 15, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).CourseList(arrCourse5R).ShotTiming(60, 9999, 60 * 2 / nRank).Shot(new FireFuncs(1.0f * fRank).FireAimPlayer);
                });
            }


            nStart += 60 * 3;
            // ● First WAVE L
            SetGroup("First Wave L", 200);
            AddEnemyGorupStart(nStart, 10, 15, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).CourseList(arrCourse2R);
            });
            // SIDE IN
            if (nRank > 1)
            {
                SetGroup("First Wave L2", 200);
                AddEnemyGorupStart(nStart, 10, 15, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY01).NP(0).Life(2).CourseList(arrCourse5).ShotTiming(60, 9999, 60 * 2 / nRank).Shot(new FireFuncs(1.0f * fRank).FireAimPlayer);
                });
            }

            nStart += 60 * 4;
            // ● RAND
            SetGroup("RAND", 2000);
            AddEnemyGorupStart(nStart, 40, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(i % 2).Life(2).Pos(rand.Next(200) + 40 + (i % 2) * 200, -50).Vect(0, 1).VectVect(0f, 0.01f).ShotTiming(60 * 1, 60 * 4).Shot(CHR.SHOT1, 3.0f);
            });


            if (nRank > 1)
            {
                NoGroup();
                AddEnemyGorupStart(nStart, 1, 1, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY51).NP(0).Life(40).CourseList(arrCourse6).ShotTiming(90, 999, 30 / nRank).Shot(new FireFuncs(2 * fRank).FireFront);
                });
                AddEnemyGorupStart(nStart, 1, 1, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY51).NP(1).Life(40).CourseList(arrCourse6R).ShotTiming(90, 999, 30 / nRank).Shot(new FireFuncs(2 * fRank).FireFront);
                });
            }

            nStart += 60 * 8;
            // ● V
            SetGroup("V", 500);

            List<Vector> arrCourse1 = Uty.CreateSpline(new Vector[] { new Vector(400, -50, 0), new Vector(400, -30, 0), new Vector(400, 100, 0), new Vector(300, 300, 0), new Vector(100, 300, 0), new Vector(100, 100, 0), new Vector(300, 300, 0), new Vector(500, 500, 0) }, 400);
            List<Vector> arrCourse1R = Uty.MirrorVectors(arrCourse1);

            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(4).CourseList(arrCourse1).ShotTiming(nRank == 1 ? 60 * 99 : 60, 180, 60 * 4 / nRank).Shot(new FireFuncs(2.0f * fRank).FireAimPlayer);
            });
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(4).CourseList(arrCourse1R).ShotTiming(nRank == 1 ? 60 * 99 : 60, 180, 60 * 4 / nRank).Shot(new FireFuncs(2.0f * fRank).FireAimPlayer);
            });
            nStart += 60 * 7;

            // ● WALL 
            SetGroup("WALL", 2000);
            for (int j = 0; j < 2; j++) {
                AddEnemyGorupStart(nStart + j*60*3, 20, 1, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY01).NP(j).Life(2).Pos(i * 24, -40).Vect(0f, 0.5f).VectVect(0f, 0.015f).ShotTiming(60 * 1, 999, 60 * 8 / nRank).Shot(new FireFuncs(1.5f * fRank).FireAimPlayer);
                });
            }
            nStart += 60 * 6;



            // ● WAVE
            SetGroup("WAVE", 1000);
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(4).CourseList(arrCourse2).ShotTiming(60 * 1,9999, 60 * 8 / nRank).Shot(new FireFuncs(1.5f * fRank).FireAimPlayer);
            });
            nStart += 60 * 3;


            // ● V
            SetGroup("V2", 1000);
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(4).CourseList(arrCourse2R).ShotTiming(60 * 1, 9999, 60 * 6 / nRank).Shot(new FireFuncs(4.0f).FireAimPlayer);
            });
            nStart += 60 * 6;


            // ● WALL2 
            SetGroup("WALL 2", 2000);
            for (int j = 0; j < 2; j++)
            {
                AddEnemyGorupStart(nStart + j * 60*3, 20, 1, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY01).NP(j).Life(10).Pos(i * 24, -50).Vect(0f, 0.5f).ShotTiming(60 * 2, 999, 60 * 4 / nRank).Shot(new FireFuncs(1.1f * fRank).FireAimPlayer);
                });
            }
            nStart += 60 * 10;

            //● TWIN
            if (nRank > 1)
            {
                NoGroup();
                AddEnemyGorupStart(nStart, 1, 1, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY51).NP(0).Life(40).CourseList(arrCourse6).ShotTiming(60, 999, 10 / nRank).Shot(new FireFuncs(2 * fRank).FireFront);
                });
                AddEnemyGorupStart(nStart, 1, 1, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY51).NP(1).Life(40).CourseList(arrCourse6R).ShotTiming(60, 999, 10 / nRank).Shot(new FireFuncs(2 * fRank).FireFront);
                });
            }


            // ● WAVE
            List<Vector> arrCourse3 = Uty.CreateSpline(new Vector[] { new Vector(-100, -50, 0), new Vector(0, -30, 0), new Vector(100, 100, 0), new Vector(480, 200, 0), new Vector(0, 300, 0), new Vector(480, 400, 0), new Vector(0, 500, 0), new Vector(480,600, 0) }, 60*8);
            List<Vector> arrCourse3R = Uty.MirrorVectors(arrCourse3);

            SetGroup("WAVE", 1000);
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(2).CourseList(arrCourse3).ShotTiming(60 * 1, 9999, 60 * 4 / nRank).Shot(new FireFuncs(2.0f).FireAimPlayer);
            });
            nStart += 60 * 3;

            SetGroup("WAVE", 1000);
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).CourseList(arrCourse3R).ShotTiming(60 * 1, 9999, 60 * 4 / nRank).Shot(new FireFuncs(2.0f).FireAimPlayer);
            });
            nStart += 60 * 8;




            // ● WAVE

            SetGroup("WAVE", 1000);
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(2).CourseList(arrCourse3).ShotTiming(60 * 2, 9999, 60 * 1).Shot(new FireFuncs(1.0f * fRank).FireAimPlayer);
            });
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).CourseList(arrCourse3R).ShotTiming(60 * 2, 9999, 60 * 1).Shot(new FireFuncs(1.0f * fRank).FireAimPlayer);
            });
            nStart += 60 * 6;




            // ● RAND + SLASH
            SetGroup("RAND + SLASH", 1000);
            AddEnemyGorupStart(nStart, 40, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(2).Pos(rand.Next(480), -50).Vect(0, 0.5f).VectVect(0f, 0.01f).ShotTiming(60 * 2, 9999, 60 * 2 / nRank).Shot(CHR.SHOT1, 1.0f);
            });
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(4).CourseList(arrCourse3R).ShotTiming(60 * 2, 9999, 60 * 2 / nRank).Shot(new FireFuncs(1.0f * fRank).FireAimPlayer);
            });
            nStart += 60 * 6;




            // ● SIDE ATTRACK
            SetGroup("SIDE R", 2000);
            AddEnemyGorupStart(nStart, 20, 20, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY02).NP(0).Life(10).Pos(464,-50).Vect(0,1).Angle(270).ShotTiming(60*2, 999, 60*2 / nRank).Shot(new FireFuncs(2 * fRank).FireFront);
            });
            SetGroup("SIDE L", 2000);
            AddEnemyGorupStart(nStart, 20, 20, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY02).NP(1).Life(10).Pos(16, -50).Vect(0, 1).Angle(90).ShotTiming(60 * 2, 999, 60 * 2 / nRank).Shot(new FireFuncs(2 * fRank).FireFront);
            });



            // ● WAVE2

            SetGroup("WAVE2", 1000);
            AddEnemyGorupStart(nStart, 20, 30, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(3).CourseList(arrCourse3R).ShotTiming(60 * 1, 9999, 60 * 5 / nRank).Shot(new FireFuncs(2.0f).FireAimPlayer);
            });

            SetGroup("WAVE", 1000);
            AddEnemyGorupStart(nStart, 20, 30, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(3).CourseList(arrCourse3).ShotTiming(60 * 1, 9999, 60 * 5 / nRank).Shot(new FireFuncs(2.0f).FireAimPlayer);
            });
            nStart += 60 * 12;



            // ● LAST WAVE
            SetGroup("LAST WAVE 1", 1000);
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(1).Life(5).CourseList(arrCourse1).ShotTiming(60 * 1, 9999, 60 * 6 / nRank).Shot(new FireFuncs(3.0f).FireAimPlayer);
            });

            SetGroup("LAST WAVE 2", 1000);
            AddEnemyGorupStart(nStart, 20, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP(0).Life(5).CourseList(arrCourse3).ShotTiming(60 * 1, 9999, 60 * 6 / nRank).Shot(new FireFuncs(3.0f).FireAimPlayer);
            });


            // SIDE IN
            if (nRank > 1)
            {
                SetGroup("LAST WAVE 2", 1000);
                AddEnemyGorupStart(nStart, 10, 15, (ActorEnemy act, int i) =>
                {
                    act.m_aps.Img(IMG.ENEMY01).NP(0).Life(5).CourseList(arrCourse5R).ShotTiming(60 * 1, 9999, 60 * 2 / nRank).Shot(new FireFuncs(1.0f * fRank).FireAimPlayer);
                });
            }


            nStart += 60 * 12;



            // ● MASSIVE
            SetGroup("MASSIVE", 10000);
            AddEnemyGorupStart(nStart, 25 * nRank, 2, (ActorEnemy act, int i) =>
            {
                act.m_aps.Img(IMG.ENEMY01).NP((i / 50) % 2).Life(10).Pos(rand.Next(480), -50).Vect(0, 0.5f).VectVect(0f, 0.01f).ShotTiming(60 * 2, 9999, 60 * 2 / nRank).Shot(CHR.SHOT1, 1.0f * fRank);
            });


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
