using System;
using System.Runtime.Serialization;

namespace SCFollowerCompare
{
    [Serializable()]
    public class Artist : ISerializable
    {
        public Artist() { }
        public Artist(string name, string url)
        {
            Name = name;
            Url = url;
        }


        public string Name { get; set; }
        public string Url { get; set; }


        public int Followers { get; set; }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Url", Url);
        }

        public Artist(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Url = (string)info.GetValue("Url", typeof(string));
        }

    }
}
