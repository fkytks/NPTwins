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
    class SetStage01old : SetStage
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

            // 敵出現
            int nStart = 60 * 2;

            List<Vector> arrCourse2 = Uty.CreateSpline(new Vector[] { new Vector(380, -50, 0), new Vector(380, -30, 0), new Vector(380, 300, 0), new Vector(340, 300, 0), new Vector(340, 200, 0), new Vector(340, -100, 0) }, 400);
            List<Vector> arrCourse2R = Uty.MirrorVectors(arrCourse2);

            List<Vector> arrCourse5 = Uty.CreateSpline(new Vector[] { new Vector(500, 100, 0), new Vector(500, 100, 0), new Vector(300, 100, 0), new Vector(100, 100, 0), new Vector(100, 200, 0), new Vector(300, 200, 0), new Vector(500, 200, 0), new Vector(700, 200, 0) }, 300);
            List<Vector> arrCourse5R = Uty.MirrorVectors(arrCourse5);

            List<Vector> arrCourse6 = Uty.CreateSpline(new Vector[] { new Vector(320, -50, 0), new Vector(320, 0, 0), new Vector(320, 100, 0), new Vector(320 + 20, 100 + 20, 0), new Vector(320 - 20, 100 + 20, 0), new Vector(320 - 20, 100 - 20, 0), new Vector(320 + 20, 100 - 20, 0), new Vector(320, -100, 0) }, 1000);
            List<Vector> arrCourse6R = Uty.MirrorVectors(arrCourse6);

/*            SetGroup("First Wave L 1", 200);
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

            for (int i = 0; i < 10; i++)
            {
                ActorEnemy act = new ActorEnemy();
                // act.m_aps.Pos(320, 200).Vect(0, 1);

                act.Init(m_game, IMG.ENEMY01, 0, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 99, 60 * 3, 1.0f,
                            arrCourse2);
                AddEnemyStart(nStart + i * 15, act);
            }

            if (nRank > 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    ActorEnemy act = new ActorEnemy();
                    // act.m_aps.Pos(320, 200).Vect(0, 1);

                    act.Init(m_game, IMG.ENEMY01, 1, 2,
                                new Vector(0,0,0),
                                new Vector(0,0,0),
                                new Vector(0,0,0),
                                -1,
                                60, 60*2 / nRank, 1.0f * fRank,
                                arrCourse5R);
                    AddEnemyStart(nStart + i * 15, act);
                }

            }


            // ● First WAVE L
            nStart += 60 * 3;
            for (int i = 0; i < 10; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 99, 60 * 3, 1.0f,
                            arrCourse2R);
                AddEnemyStart(nStart + i * 15, act);

            }

            // SIDE IN
            if (nRank > 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    ActorEnemy act = new ActorEnemy();
                    // act.m_aps.Pos(320, 200).Vect(0, 1);

                    act.Init(m_game, IMG.ENEMY01, 0, 2,
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                -1,
                                60, 60*2 / nRank, 1.0f * fRank,
                                arrCourse5);
                    AddEnemyStart(nStart + i * 15, act);
                }

            }



            nStart += 60 * 4;

/*
            // ● First WAVE R
            for (int i = 0; i < 400; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, i/200, 2,
                    new Vector(m_game.GetRandom(200) + 40 + (i % 2) * 200, -50, 0),
                            new Vector(0, 0.3, 0) + (new Vector(i)) * 0.5f,
                            new Vector(0, 0.0, 0),
                            -1,
                            60 * 1, 60 * 4, 3.0f,
                            null);

                AddEnemyStart(nStart+i  , act);
                //                AddEnemyStart(600 + i * 10, act);
            }
            nStart += 60 * 8;
*/
            // ● RAND
            for (int i = 0; i < 40; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, i % 2, 2,
                    new Vector(m_game.GetRandom(200)+40 + (i%2)*200, -50, 0),
                            new Vector(0, 1, 0),
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 4 , 3.0f,
                            null);

                AddEnemyStart(nStart + i * 10, act);
//                AddEnemyStart(600 + i * 10, act);
            }

/*　改良後
            AddEnemyGorupStart(nStart, 40, 10, (ActorEnemy act, int i) =>
            {
                act.m_aps.Pat((int)CHR.TRACER).NP(i % 2).Life(2).Pos(m_game.GetRandom(200) + 40 + (i % 2) * 200, -50).Vect(0, 1).VectVect(0f, 0.01f).ShotTiming(60 * 1, 60 * 4).Shot(CHR.SHOT1, 3.0f);
            });
            */



            if (nRank > 1)
            {
                {
                    ActorEnemy act = new ActorEnemy();
                    act.Init(m_game, IMG.ENEMY02, 0, 40,
                                new Vector(0, -99, 0),
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                -1,
                                90, 30 / nRank, 2.0f * (float)nRank,
                                arrCourse6);
                    AddEnemyStart(nStart + 50, act);
                }
                {
                    ActorEnemy act = new ActorEnemy();
                    act.Init(m_game, IMG.ENEMY02, 1, 40,
                                new Vector(0, -99, 0),
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                -1,
                                90, 30 / nRank, 2.0f * (float)nRank,
                                arrCourse6R);
                    AddEnemyStart(nStart + 50, act);
                }
            }

            nStart += 60 * 8;


            // ● V

            List<Vector> arrCourse1 = Uty.CreateSpline(new Vector[] { new Vector(400, -50, 0), new Vector(400, -30, 0), new Vector(400, 100, 0), new Vector(300, 300, 0), new Vector(100, 300, 0), new Vector(100, 100, 0), new Vector(300, 300, 0), new Vector(500, 500, 0) }, 400);
            List<Vector> arrCourse1R = Uty.MirrorVectors(arrCourse1);

            for ( int i=0; i<20; i++ ) {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01,0, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 0.5, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            nRank == 1 ? 60 * 99 : 60 , 60 * 4 /nRank, 2.0f,
                            arrCourse1);
                AddEnemyStart(nStart + i * 10, act);
            }

            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 0.5, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            nRank == 1 ? 60 * 99 : 60, 60 * 4 / nRank, 2.0f,
                            arrCourse1R);

                AddEnemyStart(nStart + i * 10, act);
            }
            nStart += 60 * 7;



            
            // ● WALL 
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 21; i++)
                {
                    ActorEnemy act = new ActorEnemy();
                    act.Init(m_game, IMG.ENEMY01, j, 3,
                                new Vector( i*24, -40, 0),
                                new Vector(0, 0.5, 0) ,
                                new Vector(0, 0.015, 0),
                                -1,
                                60 * 1, 60 * 8/nRank, 1.5f * fRank,
                                null);

                    AddEnemyStart(nStart + j*60*3 + i , act);
                    //                AddEnemyStart(600 + i * 10, act);
                }
            }
            nStart += 60 * 3;



            // ● WALL 
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 0, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                                60 * 1, 60 * 8 / nRank, 1.5f * fRank,
                            arrCourse2);
                AddEnemyStart(nStart + i * 10, act);
            }
            nStart += 60 * 3;


            // ● V
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 6 / nRank, 4.0f,
                            arrCourse2R);
                AddEnemyStart(nStart + i * 10, act);

            }
            nStart += 60 * 6;
            
            
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 21; i++)
                {
                    ActorEnemy act = new ActorEnemy();
                    act.Init(m_game, IMG.ENEMY01, j, 10,
                                new Vector(i * 24, -50, 0),
                                new Vector(0, 0.5, 0),
                                new Vector(0, 0.0, 0),
                                -1,
                                60 * 2, 60 * 4 / nRank, 1.0f * fRank,
                                null);

                    AddEnemyStart(nStart + j * 60 * 3 - i*4, act);
                    //                AddEnemyStart(600 + i * 10, act);
                }
            }
            nStart += 60 * 10;






            // TWIN
            if (nRank > 1)
            {
                {
                    ActorEnemy act = new ActorEnemy();
                    act.Init(m_game, IMG.ENEMY02, 0, 40,
                                new Vector(0, -99, 0),
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                -1,
                                60, 10 / nRank, 2.0f * (float)nRank,
                                arrCourse6);
                    AddEnemyStart(nStart + 50, act);
                }
                {
                    ActorEnemy act = new ActorEnemy();
                    act.Init(m_game, IMG.ENEMY02, 1, 40,
                                new Vector(0, -99, 0),
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                -1,
                                60, 10 / nRank, 2.0f * (float)nRank,
                                arrCourse6R);
                    AddEnemyStart(nStart + 50, act);
                }
            }

            List<Vector> arrCourse3 = Uty.CreateSpline(new Vector[] { new Vector(-100, -50, 0), new Vector(0, -30, 0), new Vector(100, 100, 0), new Vector(480, 200, 0), new Vector(0, 300, 0), new Vector(480, 400, 0), new Vector(0, 500, 0), new Vector(480,600, 0) }, 60*8);
            List<Vector> arrCourse3R = Uty.MirrorVectors(arrCourse3);

            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 0, 2,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 4 / nRank, 2.0f,
                            arrCourse3);
                AddEnemyStart(nStart + i * 10, act);
            }
            nStart += 60 * 3;
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 2,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 4 / nRank, 2.0f,
                            arrCourse3R);
                AddEnemyStart(nStart + i * 10, act);

            }
            nStart += 60 * 8;




            
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 0, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 2, 60 * 1, 1.0f*fRank,
                            arrCourse3);
                AddEnemyStart(nStart + i * 10, act);
            }
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 2, 60 * 1, 1.0f * fRank,
                            arrCourse3R);
                AddEnemyStart(nStart + i * 10, act);

            }


            nStart += 60 * 6;


            // ● RAND + SLASH

            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 10,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 0.5, 0),
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 2, 60 * 2 / nRank, 1.0f,
                            null);
                AddEnemyStart(nStart + i * 10, act);
            }
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 0 , 4,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 2, 60 * 2 / nRank, 1.0f * fRank,
                            arrCourse3R);
                AddEnemyStart(nStart + i * 10, act);

            }





            // ● SIDE ATTRACK
            nStart += 60 * 6;
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY02, 0, 10,
                            new Vector(464, -50, 0),
                            new Vector(0, 1, 0),
                            new Vector(0, 0.0, 0),
                            90,
                            60 * 2, 60 * 2/nRank, 2.0f,
                            null);
                AddEnemyStart(nStart + i * 20, act);
            }
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY02, 1, 10,
                            new Vector(16, -50, 0),
                            new Vector(0, 1, 0),
                            new Vector(0, 0.0, 0),
                            270,
                            60 * 2, 60 * 2/nRank, 2.0f,
                            null);
                AddEnemyStart(nStart + i * 20, act);
            }

            nStart += 60 * 3;



            // ● WAVE2
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 0, 3,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 4/nRank, 2.0f,
                            arrCourse3R);
                AddEnemyStart(nStart + i * 20, act);

            }

            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 3,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 4/nRank, 2.0f,
                            arrCourse3);
                AddEnemyStart(nStart + i * 20, act);

            }


            nStart += 60 * 12;



            // ● LAST WAVE
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 1, 5,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 6/ nRank, 3.0f,
                            arrCourse1);
                AddEnemyStart(nStart + i * 10, act);
            }
            for (int i = 0; i < 20; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, 0, 5,
                            new Vector(m_game.GetRandom(480), -50, 0),
                            new Vector(0, 1, 0) + (new Vector(act.m_nAngle + 90)) * 0.5f,
                            new Vector(0, 0.01, 0),
                            -1,
                            60 * 1, 60 * 6 / nRank, 3.0f,
                            arrCourse1R);
                AddEnemyStart(nStart + i * 10, act);

            }


            // SIDE IN
            if (nRank > 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    ActorEnemy act = new ActorEnemy();
                    // act.m_aps.Pos(320, 200).Vect(0, 1);

                    act.Init(m_game, IMG.ENEMY01, 0, 4,
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                new Vector(0, 0, 0),
                                -1,
                                60, 60 * 2 / nRank, 1.0f * fRank,
                                arrCourse5R);
                    AddEnemyStart(nStart + i * 15, act);
                }

            }


            nStart += 60 * 12;



            // ● MASSIVE

            for (int i = 0; i < 100; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game, IMG.ENEMY01, (i / 50) % 2, 10,
                            new Vector(m_game.GetRandom(480), -20, 0),
                            new Vector(0, 0.3, 0),
                            new Vector(0, 0.01, 0),
                            -1,
                            60 *1 , 60 * 2 / nRank, 1.0f * fRank,
                            null);
                AddEnemyStart(nStart + i * 2, act);
            }





        }
    }
}
