using System;
using System.Collections.Generic;
using System.Text;

namespace NPTwins.SplineManager
{
    public class CourseManager
    {
        List<Course> m_listCourse = new List<Course>();

        public List<Course> Courses
        {
            get { return m_listCourse; }
        }

        Dictionary<String, Course> m_dicCourse = new Dictionary<string, Course>();
        public Course this[String name] {
            get { return m_dicCourse[name]; }
        }


        public void Load(String strFilename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Course>));
            System.IO.FileStream fs = new System.IO.FileStream(strFilename, System.IO.FileMode.Open);
            m_listCourse = (List<Course>)serializer.Deserialize(fs);
            fs.Close();

            foreach (Course course in m_listCourse)
            {
                course.ExpandSpline();
                m_dicCourse.Add(course.name, course);
            }
        }


        public void Save(String strFilename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Course>));
            System.IO.FileStream fs = new System.IO.FileStream(strFilename,System.IO.FileMode.Create);
            serializer.Serialize(fs, m_listCourse);
            fs.Close();
        }


    }
}
