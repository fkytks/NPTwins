using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
//using System.Linq;

namespace NPTwins.SplineManager
{
    class SplineUty
    {
        public static List<PointF> CreateSpline(List<PointF> arrSrc, int nPoint)
        {
            return CreateSpline(arrSrc, nPoint, 0);
        }

        public static List<PointF> CreateSpline(List<PointF> arrSrc, int nPoint ,int nStart)
        {
            List<PointF> arrVect = new List<PointF>();
            if (arrSrc.Count > 0)
            {
                arrVect.Add(arrSrc[0]);
                foreach (PointF v in arrSrc)
                {
                    arrVect.Add(v);
                }
                arrVect.Add(arrSrc[ arrSrc.Count-1 ]);
            }
            int nLen = arrVect.Count;

            List<PointF> arr = new List<PointF>();

            //            arr.Add( arrVect[0] );

            double dStep = (double)(nLen - 2) / (double)nPoint;
            for (int j = 0; j < nLen - 2; j++)
            {
                for (double k = 0; k < 1; k += dStep)
                {
                    double n1 = 0.5 * (1 - k) * (1 - k);
                    double n2 = (-k + 1) * k + 0.5;
                    double n3 = k * k * 0.5;
                    double x = n1 * arrVect[j].X + n2 * arrVect[j + 1].X + n3 * arrVect[j + 2].X;
                    double y = n1 * arrVect[j].Y + n2 * arrVect[j + 1].Y + n3 * arrVect[j + 2].Y;

                    arr.Add(new PointF((float)x, (float)y));
                }
            }
            //            arr.Add( arrVect[nLen-1] );
            return arr;
        }


    }
}
