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
    public class ActorShot : Actor
    {


        public int m_nPower;

        public override void Init(Game game)
        {
            base.Init(game);

            m_surChr = m_game.res.m_surEnemy[0, 0, 0];

            m_nPower = 1;
            m_nNP = 0;
        }

        public void Init(Game game , Vector vPos, Vector vVect , int nNP , int nPower)
        {
            Init(game);

            Pos = vPos;
            Vect = vVect;

            m_nNP = nNP;
            m_nPower = nPower;
            Size = 3;

            m_surChr =  m_game.res[IMG.SHOT][nPower ==1 ? 1:2,0,nNP];
        }


        public override void ActionCore()
        {

            base.ActionCore();

            Actor act = m_game.world.m_mgrEnemy.FindHit(this);
            if (act != null)
            {
                ((ActorEnemy)act).Damage(this);
                m_fKill = true;


                //// TEST
                //if (false ) {
                //    ActorEnemy actEnemy = new ActorEnemy();
                //    actEnemy.Init(m_game, IMG.ENEMY01, m_nNP, 2,
                //        new Vector(Pos),
                //                new Vector((double)(m_game.GetRandom(100) - 50) / 20, (double)(m_game.GetRandom(100) - 50) / 20 - 2, 0),
                //                new Vector(0, 0.0, 0),
                //                -1,
                //                1000, 1, 0,
                //                null);
                //    m_game.world.m_mgrEnemy.Add(actEnemy);
                //}

            }
        }

  

    }
}
