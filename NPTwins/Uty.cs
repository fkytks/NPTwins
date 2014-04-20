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
    public class Uty
    {

        public static List<Vector> CreateSpline(Array arrSrc, int nPoint)
        {
            List<Vector> arrVect = new List<Vector>();
            foreach (Vector v in arrSrc)
            {
                arrVect.Add(v);
            }
            int nLen = arrVect.Count;

            List<Vector> arr = new List<Vector>();
            
//            arr.Add( arrVect[0] );

            double dStep = (double)(nLen-2) / (double)nPoint;
            for ( int j=0 ; j<nLen-2; j++ ) {
                for (double k = 0; k < 1; k += dStep)
                {
                    double n1 = 0.5 * (1 - k) *  (1 - k);
                    double n2 = (-k + 1) * k + 0.5;
                    double n3 = k * k * 0.5;
                    double x = n1 * arrVect[j].X + n2 * arrVect[j+1].X + n3 * arrVect[j + 2].X;
                    double y = n1 * arrVect[j].Y + n2 * arrVect[j+1].Y + n3 * arrVect[j + 2].Y;

                    arr.Add( new Vector(x,y,0) );
                }
            }
//            arr.Add( arrVect[nLen-1] );
            return arr;
        }

        public static List<Vector> MirrorVectors(List<Vector> arrSrc)
        {
            List<Vector> arrVect = new List<Vector>();
            foreach (Vector v in arrSrc)
            {
                arrVect.Add( new Vector(480-v.X,v.Y,v.Z) );
            }
            return arrVect;
        }


    }
}
