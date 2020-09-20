using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_CSharp_Test
{
    class Henkilot
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Etunimi")]
        public string Etunimi { get; set; }

        [BsonElement("Sukunimi")]
        public string Sukunimi { get; set; }

        [BsonElement("Osoite")]
        public string Osoite { get; set; }

        [BsonElement("Postinumero")]
        public string Postinumero { get; set; }

        [BsonElement("Postitoimipaikka")]
        public string Postitoimipaikka { get; set; }

        [BsonElement("Puhelin")]
        public string Puhelin { get; set; }

        [BsonElement("Sähköposti")]
        public string Sähköposti { get; set; }

        [BsonElement("JäsenyysAlkanut")]
        public DateTime JäsenyysAlkanut { get; set; }

        public Henkilot(string etunimi, string sukunimi, string osoite, string postinumero, string postitoimipaikka, string puhelin, string sähköposti, DateTime jäsenyysalkanut)
        {
            Etunimi = etunimi;
            Sukunimi = sukunimi;
            Osoite = osoite;
            Postinumero = postinumero;
            Postitoimipaikka = postitoimipaikka;
            Puhelin = puhelin;
            Sähköposti = sähköposti;
            JäsenyysAlkanut = jäsenyysalkanut;
        }
    }
}