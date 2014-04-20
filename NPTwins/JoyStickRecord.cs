using System;
using System.Collections.Generic;
using System.Text;

namespace NPTwins
{
    public class JoyStickRecord
    {
        public int m_nStage;
        public int m_nRank;
        public int m_nControllMode;

        public List<uint> m_uiJoys = new List<uint>();
        int m_iPlay;

        public void Init()
        {
            Reset();
        }

        public void Reset()
        {
            m_uiJoys.Clear();
            m_iPlay = 0;

        }

        public bool IsValid()
        {
            return m_nStage != 0;
        }

        public void Record(uint byJoy)
        {
            m_uiJoys.Add(byJoy);
        }

        public void PlayStart()
        {
            m_iPlay = 0;
        }

        public uint Play()
        {
            if (m_iPlay < m_uiJoys.Count)
            {
                return m_uiJoys[m_iPlay++];
            }
            return 0;
        }


        static public void Save(JoyStickRecord jsr , String strFilename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(JoyStickRecord));
            System.IO.FileStream fs = new System.IO.FileStream(strFilename,System.IO.FileMode.Create);
            serializer.Serialize(fs, jsr);
            fs.Close();
        }

        static public JoyStickRecord Load(String strFilename)
        {
            JoyStickRecord jsr = null;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(JoyStickRecord));
                System.IO.FileStream fs = new System.IO.FileStream(strFilename, System.IO.FileMode.Open);
                jsr = (JoyStickRecord)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception)
            {
                jsr = new JoyStickRecord();
            }
            return jsr;
        }


    }
}
