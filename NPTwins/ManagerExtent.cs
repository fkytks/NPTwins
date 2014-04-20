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

    public class EShotManager : Manager
    {
        public ActorEShot StartShot(IMG img, Vector vPos, int nAngle, float fShotSpeed)
        {
            ActorEShot act = new ActorEShot();
            Vector vVect = new Vector(nAngle - 90) * fShotSpeed;

            int nSize = (int)(m_game.res[img][0].Width * 0.4);

            act.Init(m_game, vPos, vVect);
            act.m_aps.Img(img).Size(nSize).Angle(nAngle);
            Add(act);
            return act;
        }

        public ActorEShot StartShot(IMG img, Vector vPos, Vector vAimPos, float fShotSpeed)
        {
            return StartShot(img, vPos, ((int)(vAimPos - vPos).DirectionDeg + 90), fShotSpeed);
        }

        public ActorEShot[] StartShotNway(IMG img, Vector vPos, int nCenterAngle, int nNum, int nAngleDelta, float fShotSpeed)
        {
            ActorEShot[] acts = new ActorEShot[nNum];
            for (int i = 0; i < nNum; i++)
            {
                acts[i] = StartShot(img, vPos, nCenterAngle - nAngleDelta * (nNum-1) / 2 + nAngleDelta * i, fShotSpeed);
            }
            return acts;
        }

    }










    public class ParticleManager : Manager
    {
        public void AddParticle( Surface sur, Vector vPos , Vector vVect , Vector vVectVect , int nKillCount)
        {
                Actor act = new Actor();
                act.Init(m_game);
                act.Pos = vPos;
                act.Vect = vVect;
                act.VectVect = vVectVect;
                act.Chr = sur;
                act.m_nKillCount = nKillCount;
                Add(act);
        }

        public void AddSpark( int nNum , Vector vPos )  
        {
            for (int i = 0; i < nNum; i++)
            {
                Actor act = new Actor();
                act.Init(m_game);
                act.Pos = vPos;
                act.Vect = new Vector((double)(m_game.GetRandom(100) - 50) / 10, (double)(m_game.GetRandom(100) - 50) / 10, 0);
                act.VectVect = new Vector(0, 0.1, 0);
                act.Chr = m_game.res.m_surSpark;
                act.m_nKillCount = 20;
                Add(act);
            }
        }


        public void AddBurst( int nNum , Vector vPos , Vector vVect , Surface sur )  
        {
            for (int i = 0; i < nNum; i++)
            {
                Actor act = new Actor();
                act.Init(m_game);
                act.Pos = vPos;
                act.Vect = new Vector((double)(m_game.GetRandom(100) - 50) / 15, (double)(m_game.GetRandom(100) - 50) / 15, 0);
                act.VectVect = vVect * 0.1f;
                act.Chr = sur;
                act.m_nKillCount = 20;
                Add(act);
            }
        }

    }
}
