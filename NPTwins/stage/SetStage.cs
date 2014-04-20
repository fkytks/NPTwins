using System;
using System.Collections.Generic;
using System.Text;

namespace NPTwins.Stage
{
    public abstract class SetStage
    {
        public Game m_game;
        public StageManager m_stagemanager;

        


        public EnemyGroup m_enemygroupCurrent;      // 現在の敵グループ

        public void Init(Game game , StageManager stagemanager)
        {
            m_game = game;
            m_stagemanager = stagemanager;
        }


        public abstract void BulidStage();
        public virtual  void PutBG(int nCount) {} 


        public void AddEnemyStart(int nCount, ActorEnemy act)
        {
            nCount *= 100;
            while (m_stagemanager.m_arrEnemyStart.ContainsKey(nCount))
            {
                nCount++;
            }
            m_stagemanager.m_arrEnemyStart.Add(nCount, act);
        }

        public delegate void SetParam(ActorEnemy act, int i);
        public int AddEnemyGorupStart( int nStart, int nNum, int nInterval, SetParam setparam)
        {
            for (int i = 0; i < nNum; i++)
            {
                ActorEnemy act = new ActorEnemy();
                act.Init(m_game);
                act.m_group = m_enemygroupCurrent;
                setparam(act, i);
                AddEnemyStart(nStart + i * nInterval, act);

                if ( m_enemygroupCurrent != null ) m_enemygroupCurrent.m_nCount++;
            }
            return nStart + nNum * nInterval;
        }

        public void SetGroup(String strName, int nScore)
        {
            EnemyGroup group = new EnemyGroup();
            group.Init(strName);
            group.m_nScore = nScore;

            m_stagemanager.m_arrEnemyGroup.Add(group);
            m_enemygroupCurrent = group;
        }
        public void NoGroup()
        {
            m_enemygroupCurrent = null;
        }


    }
}
