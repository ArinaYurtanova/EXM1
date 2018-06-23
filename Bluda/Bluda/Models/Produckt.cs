using System;
using System.Runtime.Serialization;

namespace ClassLibrary.Models
{
    [DataContract]
    public class Produckt
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdBluda { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public DateTime DatePostavki { get; set; }
        [DataMember]
        public string PlaceProizvod { get; set; }

        public virtual Bluda Bluda { get; set; }
    }
}
