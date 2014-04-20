using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace NPTwins.SplineManager
{
    public class Course
    {
        static int count = 1;

        // public
        public List<PointF> pts;
        public int tick;
        public int start;
        public String name;


        //
        private List<PointF> m_expts;
        public PointF this[int i] {
            get { return i<m_expts.Count ? m_expts[i] : new PointF(); }
        }
        public int Count
        {
            get { return m_expts.Count; }
        }

        public Course()
        {
            pts = new List<PointF>();
            tick = 100;
            start = 0;
            name = "noname(" + count + ")";
            count++;
        }

        public override string ToString()
        {
            return name;
        }

        public void ExpandSpline() {
            m_expts = SplineUty.CreateSpline(pts, tick , tick*start/pts.Count);
        }
    }


    public class MyCourse
    {
        Course m_course;
        public Course Course
        {
            get { return m_course; }
        }
        public bool IsValid() { return m_course != null; }


        int m_iRead;

        PointF m_ptOrigine; // 開始座標（変換前）
        PointF m_ptOffset;
        float m_fMulX;
        float m_fMulY;
        int m_nAngle;
        float m_fMirror;

        public MyCourse(Course course)
        {
            m_course = course;
            if (m_course != null) m_ptOrigine = m_course[0];

            m_iRead = 0;
            m_fMulX = m_fMulY = 1;
            m_nAngle = 0;
            m_fMirror = 1;
        }

        public void  SetStartPos(PointF pt)
        {
            m_ptOffset.X = pt.X - m_ptOrigine.X;
            m_ptOffset.Y = pt.Y - m_ptOrigine.Y;
        }

        public void SetMulti(float mulx, float muly)
        {
            m_fMulX = mulx;
            m_fMulY = muly;
        }

        public void SetAngle(int nAngle)
        {
            m_nAngle = nAngle;
        }

        public void SetMirrorMode(float fMirror)
        {
            m_fMirror = fMirror;
        }

        public PointF GetNext()
        {
            PointF pt = m_course[m_iRead];
            m_iRead++;

            pt.X -= m_ptOrigine.X;
            pt.Y -= m_ptOrigine.Y;

            pt.X *= m_fMulX;
            pt.Y *= m_fMulY;

            if (m_nAngle != 0)
            {
                float rad = (float)m_nAngle / 180 * (float)Math.PI;
                float x = (float)(pt.X * Math.Cos(rad) - pt.Y * Math.Sin(rad));
                float y =  (float)(pt.X * Math.Sin(rad) + pt.Y * Math.Cos(rad));
                pt.X = x;
                pt.Y = y;
            }
            
            pt.X += m_ptOrigine.X;
            pt.Y += m_ptOrigine.Y;

            pt.X = 240 + (pt.X - 240) * m_fMirror;

            pt.X += m_ptOffset.X * m_fMirror;
            pt.Y += m_ptOffset.Y;

            return pt;
        }

        public bool IsEOP()
        {
            return m_iRead >= m_course.Count;
        }


    }

}
