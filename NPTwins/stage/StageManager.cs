using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SdlDotNet.Core;


namespace NPTwins.Stage
{




    public class StageManager
    {
        public Game m_game;

        public SortedList<int, ActorEnemy> m_arrEnemyStart = new SortedList<int, ActorEnemy>();
        public int m_iEnemyStart;   // ステージの進行状況

        public List<EnemyGroup> m_arrEnemyGroup = new List<EnemyGroup>();


        SetStage m_setstage;

        // 初期化（起動時に1回のみ）
        public void Init(Game game)
        {
            m_game = game;
        }

        // ステージ構築
        public void BuildStage()
        {
            m_arrEnemyStart.Clear();
            m_arrEnemyGroup.Clear();


            switch (m_game.playinfo.m_nStage)
            {
                case 1:
                    m_setstage = new SetStage01();
                    break;
                case 2:
                    m_setstage = new SetStage02();
                    break;
                case 3:
                    m_setstage = new SetStage01();
                    m_setstage.Init(m_game, this);
                    m_setstage.BulidStage();
                    m_setstage = new SetStage02();
                    break;
                default:
                    m_setstage = new SetStage01();
                    break;
            }

            m_setstage.Init(m_game, this);
            m_setstage.BulidStage();

            m_iEnemyStart = 0;
        }

        static public int GetMaxStage()
        {
            return 3;
        }

        // 敵出現
        public void ArrivalEnemy()
        {
            // ボス出現中は次のボスを出さない
            if (m_game.world.m_actBoss != null && m_game.world.m_actBoss.m_fKill == false
                && m_arrEnemyStart.Count > 0 && m_arrEnemyStart[m_arrEnemyStart.Keys[0]].m_nBossNo > 0)
            {
                return;
            }

            // 敵出現
            while (m_arrEnemyStart.Count > 0)
            {
                int nKey = m_arrEnemyStart.Keys[0];
                if (nKey > m_iEnemyStart * 100) break;

                ActorEnemy act = m_arrEnemyStart[nKey];
                m_game.world.m_mgrEnemy.Add(act);
                if (act.m_delegateEnter != null) act.m_delegateEnter(act);
                if (act.m_nBossNo>0) m_game.world.m_actBoss = act;


                EnemyGroup eg = act.m_group;
                if (eg!=null && !eg.m_fAlreadyStart)
                {
                    eg.m_fAlreadyStart = true;
                    m_game.world.PutMessageAsParticle_EnemyGroup(eg.m_nCount + " enemys formation [" + eg.m_strName + "] ARRIVAL!", Color.Red);

                }

                m_arrEnemyStart.Remove(nKey);

            }
            m_iEnemyStart++;
        }

        // BG出現
        public void PutBG(int nCount)
        {
            m_setstage.PutBG(nCount);
        }

        public int GetEnemyLeft()
        {
            return m_arrEnemyStart.Count;
        }

    }
}
