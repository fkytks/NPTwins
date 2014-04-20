using System;
using System.Collections.Generic;
using System.Text;

namespace NPTwins
{
    public class Setting
    {
        public string VideoDriver;
        public bool UseSDLHardware;
        public bool FullScreen;

        public int JoyButtonA;
        public int JoyButtonB;
        public int JoyButtonC;
        public int JoyButtonD;

        public int RunCount;

//        public static Setting v = new Setting();


        public Setting()
        {
            VideoDriver = "";
            UseSDLHardware = false;
            FullScreen = false;

            JoyButtonA = 0;
            JoyButtonB = 1;
            JoyButtonC = 2;
            JoyButtonD = 3;

            RunCount = 0;
        }

        static public string GetSettingPath()
        {
            //            return System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return "./setting.xml";
        }

        static public bool SaveSetting(Setting setting)
        {
            bool fResult = false;
            string strFilename = GetSettingPath();

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
                System.IO.FileStream fs = new System.IO.FileStream(strFilename, System.IO.FileMode.Create);
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(fs, setting);
                //閉じる
                fs.Close();
                fResult = true;
            }
            catch (Exception)
            {

            }
            return fResult;
        }

        static public Setting LoadSetting()
        {
            string strFilename = GetSettingPath();
            Setting setting = new Setting();

            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
                System.IO.FileStream fs = new System.IO.FileStream(strFilename, System.IO.FileMode.Open);
                //XMLファイルから読み込み、逆シリアル化する
                setting = (Setting)serializer.Deserialize(fs);

                //閉じる
                fs.Close();
            }
            catch (Exception)
            {

            }

            return setting;
        }

    }

}
