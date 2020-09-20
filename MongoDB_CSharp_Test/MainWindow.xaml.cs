using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB_CSharp_Test
{
    public partial class MainWindow : Window
    {
        //Globaalit muuttujat
        static MongoClient client = new MongoClient();
        static IMongoDatabase db = client.GetDatabase("rekisteri");
        //static IMongoCollection<Shares> collection = db.GetCollection<Shares>("henkilot");
        static IMongoCollection<Henkilot> collection = db.GetCollection<Henkilot>("henkilot");
        
        //Luetaan kaikki dokumentit pääikkunaan
        public void ReadCollection()
        {
            //Lista collectionin lukua varten
            List<Henkilot> list = collection.AsQueryable().ToList();
            HenkiloLista.ItemsSource = list;

            //Tekstikentät oletusarvoisesi tyhjiä
            tbxEtunimi.Text = "";
            tbxSukunimi.Text = "";
            tbxKatuosoite.Text = "";
            tbxPostinumero.Text = "";
            tbxPostitoimipaikka.Text = "";
            tbxPuhelin.Text = "";
            tbxSahkoposti.Text = "";
            tbxJasenyysAlkanut.Text = "";
            tbxId.Text = "";
        }
        
        //Pääikkunan initialisointi ja dokumenttien luku sinne
        public MainWindow()
        {
            InitializeComponent();
            ReadCollection();
        }

        //Luetaan kaikki tiedot HenkiloLista Listeview-kohtaan
        private void HenkiloLista_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Henkilot h = (Henkilot)HenkiloLista.SelectedItem;

            DateTime alkuPvm = h.JäsenyysAlkanut;
            tbxEtunimi.Text = h.Etunimi;
            tbxSukunimi.Text = h.Sukunimi;
            tbxKatuosoite.Text = h.Osoite;
            tbxPostinumero.Text = h.Postinumero;
            tbxPostitoimipaikka.Text = h.Postitoimipaikka;
            tbxPuhelin.Text = h.Puhelin;
            tbxSahkoposti.Text = h.Sähköposti;
            tbxJasenyysAlkanut.Text = alkuPvm.ToString("dd.MM.yyyy");            
            dpJasenyysAlku.SelectedDate = alkuPvm;
            tbxId.Text = h.Id.ToString();
        }

        //Tyhjentää kaikki kentät
        public void EmptyFields()
        {
            tbxEtunimi.Text = "";
            tbxSukunimi.Text = "";
            tbxKatuosoite.Text = "";
            tbxPostinumero.Text = "";
            tbxPostitoimipaikka.Text = "";
            tbxPuhelin.Text = "";
            tbxSahkoposti.Text = "";
            tbxJasenyysAlkanut.Text = "";
            dpJasenyysAlku.SelectedDate = null;
            tbxId.Text = "";
        }

        //Uuden henkilön lisääminen 
        private void btnLisaa_Click(object sender, RoutedEventArgs e)
        {
            //Uuden Henkilo-objektin luonti tekstikentissä olevilla arvoilla
            //Jostain syystä päivämäärään pitää lisätä 1 päivä - muuten tietokantaan tallentuu aina WPF:ssä asetettua päivää edeltävä pvm
            Henkilot h = new Henkilot(tbxEtunimi.Text, tbxSukunimi.Text, tbxKatuosoite.Text, tbxPostinumero.Text, tbxPostitoimipaikka.Text, tbxPuhelin.Text, tbxSahkoposti.Text, dpJasenyysAlku.SelectedDate.Value.AddDays(1));
            //Arvojen lisääminen collectioniin
            collection.InsertOne(h);
            //Päivitetään Listview
            ReadCollection();
            //Tyhjennetään tekstikentät
            EmptyFields();
        }

        //Henkilän tietojen muokkaaminen
        private void btnMuokkaa_Click(object sender, RoutedEventArgs e)
        {
            //Uudet arvot tekstikentistä
            var muokkaa = Builders<Henkilot>.Update.Set("Etunimi", tbxEtunimi.Text).Set("Sukunimi", tbxSukunimi.Text).Set("Osoite", tbxKatuosoite.Text).Set("Postinumero",tbxPostinumero.Text).Set("Postitoimipaikka",tbxPostitoimipaikka.Text).Set("Puhelin",tbxPuhelin.Text).Set("Sähköposti",tbxSahkoposti.Text).Set("JäsenyysAlkanut",dpJasenyysAlku.SelectedDate.Value.AddDays(1));
            //Collectionin päivitys uusilla arvoilla
            collection.UpdateOne(h => h.Id == ObjectId.Parse(tbxId.Text), muokkaa);
            //Päivitetään Listview
            ReadCollection();
            //Tyhjennetään tekstikentät
            EmptyFields();
        }

        //Henkilön poistaminen tietokannasta
        private void btnPoista_Click(object sender, RoutedEventArgs e)
        {
            //Poista henkilö tietokannasta Id:n perusteella. Id haetaan tbxId kentästä (joka voi olla myös hidden tilassa)
            collection.DeleteOne(h => h.Id == ObjectId.Parse(tbxId.Text));
            //Päivitetään Listview
            ReadCollection();
            //Tyhjennetään tekstikentät
            EmptyFields();
        }

        //Tietokannan luoman Id:n näyttäminen
        private void btnId_Click(object sender, RoutedEventArgs e)
        {
            lblId.Visibility = Visibility.Visible;
            tbxId.Visibility = Visibility.Visible;
            btnId2.Visibility = Visibility.Visible;

            btnId.Visibility = Visibility.Hidden;
        }

        //Tietokannan luoman Id:n piilottaminen
        private void btnId2_Click(object sender, RoutedEventArgs e)
        {
            lblId.Visibility = Visibility.Hidden;
            tbxId.Visibility = Visibility.Hidden;
            btnId2.Visibility = Visibility.Hidden;
            btnId.Visibility = Visibility.Visible;
        }

        //Tyhjennä tekstikentät
        private void btnTyhjenna_Click(object sender, RoutedEventArgs e)
        {
            EmptyFields();
        }
    }
}
