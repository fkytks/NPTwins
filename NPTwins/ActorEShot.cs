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
    public class ActorEShot : ActorEnemy
    {

        bool m_fBuzzed;

        public override void Init(Game game)
        {
            base.Init(game);

            //m_surChr = m_game.res.m_surEnemy[0,0,0];

            m_nNP = 0;
            Size = 4;
            m_fBuzzed = false;
        }

        public void Init(Game game , Vector vPos, Vector vVect )
        {
            Init(game);

            Pos = vPos;
            Vect = vVect;

        }


        public override void ActionCore()
        {

            base.ActionCore();
        }


        public override void PlayerHitCheck()
        {
            ActorPlayer actPlayer = m_game.world.m_CurrentPlayer;
            if (actPlayer.CalcDist(this) < 30)
            {
                // buzz
                if (!m_fBuzzed)
                {
                    m_fBuzzed = true;
                    // m_game.res.m_sounds["shot"].Play();
                    actPlayer.AddSWCharge(5);
                    // m_surChr = m_game.res.m_surBombRing;

                    m_game.ScoreUp(1);

                    m_game.world.m_mgrParticle.AddSpark(2, Pos);

                }

                if (actPlayer.IsHit(this))
                {
                    actPlayer.Damage(this);
                    m_fKill = true;
                }
            }
        }

    }
}
