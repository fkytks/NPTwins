using System;
using System.Collections.Generic;
using System.Text;

namespace NPTwins
{
    public class PlayInfo
    {
        public int m_nStage;
        public int m_nScore;
        public int m_nLeft;
        public int m_nStartStage;
        public int m_nRank;             // ゲームランク (1:Easy 2:Normal 3;Hard 4:Hell)
        public int m_nControllMode;     // 操作モード   (1:Type-T 2:Type-R)

        public bool m_fDemo;

        public void Init()
        {
            Reset();
            m_nStartStage = 1;
            m_nRank = 2;
            m_nControllMode = 1;
            m_fDemo = false;

        }

        public void Reset()
        {
            m_nStage = m_nStartStage;
            m_nScore = 0;
            m_nLeft = 3;
        }

        public String GetRankName()
        {
            String[] strRankName = { "None", "Easy", "Normal", "Hard", "Hell", "Harven", "Kaos" };

            return strRankName[m_nRank];


        }

    }
}
