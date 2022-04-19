using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SCFollowerCompare
{
    //[System.Xml.Serialization.XmlInclude(typeof(Artist))]
    //[System.Xml.Serialization.XmlInclude(typeof(ObservableCollection<Artist>))]
    public class XmlManager
    {
        IFileReadWrite fileReadWrite;
        public XmlManager()
        {
            fileReadWrite = Xamarin.Forms.DependencyService.Get<IFileReadWrite>();
        }
        //public static void XmlDataWriter(ObservableCollection<Artist> obj, string filename)
        //{
        //    //XmlSerializer sr = new XmlSerializer(obj.GetType(), new Type[] { typeof(Artist) });
        //    XmlSerializer sr = new XmlSerializer(typeof(ObservableCollection<Artist>));
        //    TextWriter writer = new StreamWriter(filename);
        //    sr.Serialize(writer, obj);
        //    writer.Close();
        //}

        //public static async Task<bool> XmlDataWriter(ObservableCollection<Artist> obj)
        //{
        //    XmlSerializer sr = new XmlSerializer(typeof(ObservableCollection<Artist>));
        //    TextWriter writer = new StreamWriter(filename);
        //    sr.Serialize(writer, obj);
        //    writer.Close();

        //    await fileReadWrite.WriteToFile(sr.Serialize(writer, obj));
        //}

        public static object XmlDataReader(string filename)
        {
            ObservableCollection<Artist> obj;
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Artist>));
            FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            XmlReader xml = XmlReader.Create(reader);
            obj = (ObservableCollection<Artist>)xs.Deserialize(reader);
            //obj = (ObservableCollection<Artist>)xs.Deserialize(reader);
            reader.Close();
            return obj;
        }
    }
}
