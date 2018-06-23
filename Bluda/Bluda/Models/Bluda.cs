using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ClassLibrary.Models
{
    [DataContract]
    public class Bluda
    {
        [DataMember]
        public int Id { get; set; }
        [ForeignKey("IdBluda")]
        public virtual List<Produckt> Products { get; set; }
        [DataMember]
        public string NameBluda { get; set; }
        [DataMember]
        public string TypeBluda { get; set; }
        [DataMember]
        public DateTime DateCreate { get; set; }
    }
}
