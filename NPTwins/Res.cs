using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;


namespace NPTwins
{

    // カスタムアトリビュート
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ImgInfoAttribute : Attribute
    {
        public String m_strFilename;
        public int m_nPattern;
        public int m_nAngle;
        public int m_nNP;
        public ImgInfoAttribute(String strFilename,int nP,int nA,int nN) {
            m_strFilename = strFilename; 
            m_nPattern = nP;
            m_nAngle = nA;
            m_nNP = nN;
        }
        public ImgInfoAttribute(String strFilename,int nP,int nA) : this(strFilename,nP,nA,1) {}
        public ImgInfoAttribute(String strFilename,int nP) : this(strFilename,nP,1,1) {}
        public ImgInfoAttribute(String strFilename) : this(strFilename,1,1,1) {}

    }

    public enum IMG
    {
        NONE = 0,
        [ImgInfo("ActorPlayer%pat%%np%.png", 2, 1, 2)]
        PLAYER,
        [ImgInfo("20080122_045040_fromRAW_450.jpg")]
        MOON,

        [ImgInfo("spark.png")]
        SPARK,
        [ImgInfo("eshot01.png", 1, 32, 1)]
        ESHOT01,
        [ImgInfo("eshot02.png", 1, 32, 1)]
        ESHOT02,
        [ImgInfo("eshot03.png", 1, 32, 1)]
        ESHOT03,
        [ImgInfo("eshot04.png", 1, 32, 1)]
        ESHOT04,
        [ImgInfo("ring.png")]
        RING,
        [ImgInfo("marker.png")]
        MARKER,
        [ImgInfo("bombring.png")]
        BOMBRING,
        [ImgInfo("sekihou.png")]
        SEKIHOU,
        [ImgInfo("shot%pat%%np%.png", 3, 8, 2)]
        SHOT,
        [ImgInfo("title.png")]
        TITLE,
        [ImgInfo("titleMenu.png")]
        TITLE_MENU,
        [ImgInfo("sidePict%np%.png", 1, 1, 2)]
        SIDEPICT,
        [ImgInfo("sidePictBG.bmp")]
        SIDEPICTBG,

        [ImgInfo("EN01%np%.png", 1, 32, 2)]
        ENEMY01,
        [ImgInfo("EN02%np%.png", 1, 8, 2)]
        ENEMY02,
        [ImgInfo("EN51%np%.png", 1, 32, 2)]
        ENEMY51,


        MAX
    };



    public class FTSurface
    {
        static String s_strBasePath = "img";

        int m_nPattern;
        int m_nAngle;
        int m_nNP;

        Surface[] m_surfaces;

        public FTSurface(String strFilename) : this(1, 1, 1) { LoadImage(strFilename); }
        public FTSurface(String strFilename, int nPattern) : this(nPattern, 1, 1) { LoadImage(strFilename); }
        public FTSurface(String strFilename, int nPattern, int nAngle) : this(nPattern, nAngle, 1) { LoadImage(strFilename); }
        public FTSurface(String strFilename, int nPattern, int nAngle, int nNP) : this(nPattern, nAngle, nNP) { LoadImage(strFilename); } 

        public FTSurface() : this(1, 1, 1) { }
        public FTSurface(int nPattern) : this(nPattern, 1, 1) { }
        public FTSurface(int nPattern, int nAngle) : this(nPattern, nAngle, 1) {}
        public FTSurface(int nPattern, int nAngle, int nNP)
        {
            m_nPattern = nPattern;
            m_nAngle = nAngle;
            m_nNP = nNP;

            m_surfaces = new Surface[m_nPattern * m_nAngle * m_nNP];
        }

        public int CalcIndex(int nPattern, int nAngle, int nNP)
        {
            return nNP * m_nPattern * m_nAngle + nPattern * m_nAngle + nAngle;
        }

        public void LoadImage(String strFilename)
        {
            String strModFilename;
            for (int iN = 0; iN < m_nNP; iN++)
            {
                for (int iP = 0; iP < m_nPattern; iP++)
                {
                    strModFilename = strFilename;
                    strModFilename = strModFilename.Replace("%pat%", iP.ToString("00"));
                    strModFilename = strModFilename.Replace("%np%", (iN == 0 ? "N" : "P"));
                    m_surfaces[CalcIndex(iP,0,iN )] = new Surface(System.IO.Path.Combine(s_strBasePath, strModFilename));
                    for (int iA = 1; iA < m_nAngle; iA++)
                    {
                        m_surfaces[CalcIndex(iP, iA, iN)] = m_surfaces[iN * m_nPattern * m_nAngle + iP * m_nAngle + 0].CreateRotatedSurface(-360 * iA / m_nAngle);
                    }
                }
            }
        }

        public void ForEach( Action<Surface> action)
        {
            for (int i = 0; i < m_surfaces.Length; i++) action(m_surfaces[i]);
        }


        public Surface this[int iP]
        {
            get { return m_surfaces[CalcIndex(iP, 0, 0)]; }
        }
        public Surface this[int iP, int iA]
        {
            get { return m_surfaces[CalcIndex(iP, ((iA + 3600) * m_nAngle / 360) % m_nAngle, 0)]; }
        }
        public Surface this[int iP, int iA , int iN]
        {
            get { return m_surfaces[CalcIndex(iP, ((iA + 3600) * m_nAngle / 360) % m_nAngle, iN)]; }
        }
    }





    public class Res
    {
        // フォント
        public SdlDotNet.Graphics.Font m_font;
        public SdlDotNet.Graphics.Font font
        {
            get { return m_font; }
        }
        public SdlDotNet.Graphics.Font m_fontSmall;
        public SdlDotNet.Graphics.Font fontSmall
        {
            get { return m_fontSmall; }
        }


        // サーフェイス
        public FTSurface[] m_img = new FTSurface[(int)IMG.MAX];
        public FTSurface this[ IMG img ]
        {
            get { return m_img[(int)img]; }
        }
        
        

        public Surface[] m_surPlayer = new Surface[2];
//        public Surface[] m_surShot = new Surface[2];
        public Surface m_surRing;
        public Surface m_surMarker;
        public Surface m_surBombRing;

        public Surface m_surSekihou;
        public Surface m_surMoon;

        public Surface[] m_surSidePict = new Surface[2];
        public Surface m_surSidePictBG;
        public Surface m_surSidePictBGBuf;

        public Surface m_surTitle;
        public Surface m_surTitleMenu;

        public Surface m_surGaugeLife;
        public Surface m_surGaugeLifeS;

        public Surface m_surSpark;
        public Surface m_surEShot;

        public Surface[,,] m_surEnemy = new Surface[10,2,8];





        // サウンド
        public MusicDictionary m_musics = new MusicDictionary();
        public SoundDictionary m_sounds = new SoundDictionary();
        public bool m_fBgmPlay=false;


        public void LoadImage(IMG img, String strFilename, int nPattern, int nAngle, int nNP)
        {
            m_img[(int)img] = new FTSurface(strFilename, nPattern, nAngle, nNP);
        }




        public void Init(Surface surface)
        {
            // フォント読み込み
            m_font = new SdlDotNet.Graphics.Font("font" + System.IO.Path.DirectorySeparatorChar + "mplus-2p-black.ttf", 24);
            m_fontSmall = new SdlDotNet.Graphics.Font("font" + System.IO.Path.DirectorySeparatorChar + "mplus-2p-black.ttf", 16);




            // イメージロード
            foreach (IMG enumImage in Enum.GetValues(typeof(IMG)))
            {
                Console.WriteLine((int)enumImage + " = " + enumImage.ToString());
                ImgInfoAttribute[] infos = (ImgInfoAttribute[])enumImage.GetType().GetField(enumImage.ToString()).GetCustomAttributes(typeof(ImgInfoAttribute), false);
                if (infos.Length > 0)
                {
                    LoadImage(enumImage,infos[0].m_strFilename,infos[0].m_nPattern,infos[0].m_nAngle,infos[0].m_nNP );
                }
            }








//            LoadImage(IMG.PLAYER,"ActorPlayer%np%.png", 1, 1, 2);
            m_surMoon = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "20080122_045040_fromRAW_450.jpg");
            m_surMoon.TransparentColor = Color.Black;
            m_surMoon.Transparent = true;

            

            m_surSpark = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "spark.png");
            m_surEShot = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "eshot01.png");
            m_surRing = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "ring.png");
            m_surMarker = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "marker.png");
            m_surBombRing = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "bombring.png");
            m_surSekihou = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "sekihou.png");

//            m_surShot[0] = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "shot01N.png");
//            m_surShot[1] = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "shot01P.png");

            m_surTitle = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "title.png");
            m_surTitleMenu = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "titleMenu.png");
            m_surSidePict[0] = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "sidePictN.png");
            m_surSidePict[1] = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "sidePictP.png");
            m_surSidePictBG = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "sidePictBG.bmp");
            m_surSidePictBGBuf = new Surface(160, 480);


            m_surGaugeLife = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "gaugeLife.png");
            m_surGaugeLifeS = new Surface("img" + System.IO.Path.DirectorySeparatorChar + "gaugeLifeS.png");

            for (int i = 1; i <= 2; i++)
            {
                  for (int j = 0; j < 2; j++)
                {
                    String strFilename = "EN" + i.ToString("00") + (j == 0 ? "N" : "P") + ".png";
                    m_surEnemy[i, j, 0] = new Surface("img" + System.IO.Path.DirectorySeparatorChar + strFilename);
                    m_surEnemy[i, j, 0].Convert();
                    for (int k = 1; k < 8; k++)
                    {
                        m_surEnemy[i, j, k] = m_surEnemy[i, j, 0].CreateRotatedSurface(-k * 360 / 8);
                        m_surEnemy[i, j, k].Convert();
                    }
                }
            }

            




            // サウンド
            Mixer.ReserveChannels(200);
            

            m_musics["bgmTitle"] = new Music("sound" + System.IO.Path.DirectorySeparatorChar + "tamspiral.ogg");
            m_musics["bgm"] = new Music("sound" + System.IO.Path.DirectorySeparatorChar + "tamco07.ogg");
            m_fBgmPlay = false;

            m_sounds["shot"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "tm2_shoot000.ogg");
            m_sounds["start"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "cursor02.ogg");
            m_sounds["change"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "mecha22_r.ogg");
            m_sounds["bomb"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "tm2_bom004mod.ogg");
            m_sounds["damaged"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "tm2_bom005.ogg");
            m_sounds["edamaged"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "metal38mod.ogg");


            



            m_musics["allclear"] = new Music("sound" + System.IO.Path.DirectorySeparatorChar + "se_allclear.ogg");
            m_musics["miss"] = new Music("sound" + System.IO.Path.DirectorySeparatorChar + "se_miss.ogg");

            m_sounds["clear"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "se_clear.ogg");
            m_sounds["click"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "se_click.ogg");
            m_sounds["get"] = new Sound("sound" + System.IO.Path.DirectorySeparatorChar + "se_get2.ogg");

        }
    }





    

}
