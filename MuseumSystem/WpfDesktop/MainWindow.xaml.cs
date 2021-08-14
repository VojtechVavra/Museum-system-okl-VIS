using BusinessLayer.BO;
using BusinessLayer.Controllers;
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


namespace WpfDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoginCanvas.Visibility = Visibility.Visible;
            RecepceCanvas.Visibility = Visibility.Hidden;
            ArcheologCanvas.Visibility = Visibility.Hidden;
        }

        private void prihlasitSe_button_Click(object sender, RoutedEventArgs e)
        {
            //var a = comboBox_userLogin;
            string selectedUser = comboBox_userLogin.SelectionBoxItem.ToString();// SelectionBoxItem.ToString();
            if (selectedUser == "Recepce")
            {
                RecepceCanvas.Visibility = Visibility.Visible;
                ArcheologCanvas.Visibility = Visibility.Hidden;
                LoginCanvas.Visibility = Visibility.Hidden;

                Canvas_zaevidovat_navstevu.Visibility = Visibility.Hidden;

                prihlasenaRole_text.Text = "Recepce";
                Console.WriteLine("Recepce");
            }
            else if(selectedUser == "Archeolog")
            {
                RecepceCanvas.Visibility = Visibility.Hidden;
                ArcheologCanvas.Visibility = Visibility.Visible;
                LoginCanvas.Visibility = Visibility.Hidden;
                vytvoreni_vystavy_canvas.Visibility = Visibility.Hidden;
                seznamVystav_canvas.Visibility = Visibility.Hidden;

                prihlasenaRole_text.Text = "Archeolog";
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void odhlasitSe_button_Click(object sender, RoutedEventArgs e)
        {
            setVisibilityOnLogout();
        }

        private void odhlasitSe_button2_Click(object sender, RoutedEventArgs e)
        {
            setVisibilityOnLogout();
        }

        private void zaevidovat_button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Canvas_zaevidovat_navstevu.Visibility = Canvas_zaevidovat_navstevu.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            bezRezervace_canvas.Visibility = Visibility.Hidden;
            sRezervaci_canvas.Visibility = Visibility.Hidden;

            chybove_hlaseni_evicence_s_rez_text.Text = "";
        }

        private void setVisibilityOnLogout()
        {
            LoginCanvas.Visibility = Visibility.Visible;

            RecepceCanvas.Visibility = Visibility.Hidden;
            ArcheologCanvas.Visibility = Visibility.Hidden;

            vytvoreni_vystavy_canvas.Visibility = Visibility.Hidden;
            seznamVystav_canvas.Visibility = Visibility.Hidden;

            prihlasenaRole_text.Text = "Role";
        }

        private void bezRezervace_button_Click(object sender, RoutedEventArgs e)
        {
            bezRezervace_canvas.Visibility = Visibility.Visible;
            info_o_zaevidovani_text.Text = "";
            pocetOsob_textbox.Text = "";
        }
        
        // tlacitko na zaevidovani poctu navstev bez rezervace
        private void sRezervaci_zaevidovat_button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = rezervace_dataGrid.SelectedItem;
            string pocetOsob = pocetOsob_s_rezervaci_textbox.Text;
            string email = email_k_otestovani_textbox.Text;

            SpravaEvidenci se = SpravaEvidenci.Instance;

            object vybranaRezervace = selectedItem;
            if(vybranaRezervace is null)
            {
                sRezervaci_canvas.Visibility = Visibility.Hidden;
                chybove_hlaseni_evicence_s_rez_text.Text = "Nebyla vybrána rezervace!";
                return;
            }
            bool povedloSe = se.EvidenceNavstevySRezervaci((Rezervace)vybranaRezervace, email, pocetOsob);

            if(povedloSe)
            {
                email_k_otestovani_textbox.Text = "";
                pocetOsob_s_rezervaci_textbox.Text = "";
                rezervace_dataGrid.UnselectAll();

                sRezervaci_canvas.Visibility = Visibility.Hidden;
                chybove_hlaseni_evicence_s_rez_text.Text = "Zaevidovaní proběhlo úspěšně";
            }
            else
            {
                sRezervaci_canvas.Visibility = Visibility.Hidden;
                chybove_hlaseni_evicence_s_rez_text.Text = "Nastala chyba při vkládání, nebo email byl nesprávný!";
            }
        }

        private void sRezervaci_button_Click(object sender, RoutedEventArgs e)
        {
            sRezervaci_canvas.Visibility = Visibility.Visible;

            chybove_hlaseni_evicence_s_rez_text.Text = "";
            info_o_zaevidovani_text.Text = "";
            pocetOsob_s_rezervaci_textbox.Text = "";
            email_k_otestovani_textbox.Text = "";


            SpravaRezervaci sr = SpravaRezervaci.Instance;
            List<Rezervace> rezervace = new List<Rezervace>();

            bool uspech = sr.NactiRezervace(rezervace);

            //List<Rezervace> rezervace = App.NactiRezervace();

            rezervace_dataGrid.UnselectAll();
            rezervace_dataGrid.ItemsSource = rezervace;
        }

        private void bezRezervace_zaevidovat_button_Click(object sender, RoutedEventArgs e)
        {
            string pocetOsob = pocetOsob_textbox.Text;

            SpravaEvidenci se = SpravaEvidenci.Instance;

            bool uspech = se.EvidenceNavstevyBezRezervaci(pocetOsob);
            //bool uspech = App.ulozNavstevu(pocetOsob, null);

            if (uspech)
            {
                info_o_zaevidovani_text.Text = "Pocet osob uspesne zaznamenan do databaze";
                Console.WriteLine("Pocet osob uspesne zaznamenan do databaze");
            }
            else
            {
                info_o_zaevidovani_text.Text = "Neplatny vstup, nebo chyba! ";
                Console.WriteLine("Neplatny vstup!");
            }

            pocetOsob_textbox.Text = "";
        }

        private void vystava_button_Click(object sender, RoutedEventArgs e)
        {
            vytvoreni_vystavy_canvas.Visibility = Visibility.Visible;
            seznamVystav_canvas.Visibility = Visibility.Hidden;

            List<Artefakt> artefakty = new List<Artefakt>();

            SpravaArtefaktu sa = SpravaArtefaktu.Instance;
            bool uspech = sa.NactiArtefakty(artefakty);

            artefakty_k_pridani_dataGrid.UnselectAll();
            artefakty_k_pridani_dataGrid.ItemsSource = artefakty;


            SpravaVystavy sv = SpravaVystavy.Instance;

            pridane_artefakty_dataGrid.ItemsSource = sv.VratRoztvorenouVystavu();
            pridane_artefakty_dataGrid.UnselectAll();

            artefakt_nemohl_byt_pridan_textBlock.Text = "";
        }

        private void pridat_artefakt_button_Click(object sender, RoutedEventArgs e)
        {
            artefakt_nemohl_byt_pridan_textBlock.Text = "";

            if (zacatek_textBox.Text.Length == 0 || konec_textBox.Text.Length == 0)
            {
                artefakt_nemohl_byt_pridan_textBlock.Text = "Vyplnte datumy!";
                return;
            }
            else if(artefakty_k_pridani_dataGrid.SelectedItem == null)
            {
                artefakt_nemohl_byt_pridan_textBlock.Text = "Nevybrali jste artefakt";
                return;
            }
            else {
                // example time: "2009-05-08 14:40:52,531"
                DateTime odData;
                DateTime doData;

                try
                {
                    odData = DateTime.ParseExact(zacatek_textBox.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    doData = DateTime.ParseExact(konec_textBox.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return;
                }
                

                if(odData > doData)
                {
                    return;
                }

                // TODO: Zavolat metodu na pridani artefaktu - a ta bude volat kontrolu, zda muze byt pridana
                // podle vyplneneho datumu

                Artefakt vybranyArtefakt = (Artefakt)artefakty_k_pridani_dataGrid.SelectedItem;
                SpravaVystavy sv = SpravaVystavy.Instance;
                bool uspech = sv.pridatArtefaktDoVystavy(odData, doData, vybranyArtefakt);

                if(uspech)
                {
                    // priradit artefakt
                    pridane_artefakty_dataGrid.ItemsSource = null;
                    pridane_artefakty_dataGrid.ItemsSource = sv.VratRoztvorenouVystavu();
                }
                else
                {
                    // vypsat, ze artefakt nemuze byt prirazen k vystave
                    artefakt_nemohl_byt_pridan_textBlock.Text = "Artefakt nemohl byt přiřazen!";
                }
            }
        }

        private void ulozit_vystavu_button_Click(object sender, RoutedEventArgs e)
        {
            if(nazevVystavy_textBox.Text == null)
            {
                artefakt_nemohl_byt_pridan_textBlock.Text = "Nevybrali jste název výstavy!";
                return;
            }

            // uloz vystavu do Json souboru na disk
            SpravaVystavy sv = SpravaVystavy.Instance;
            sv.getVystava().nastavJmenoVystavy(nazevVystavy_textBox.Text);

            DateTime odData = DateTime.ParseExact(zacatek_textBox.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime doData = DateTime.ParseExact(konec_textBox.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            sv.getVystava().nastavDatumZacatkuKonce(odData, doData);

            bool uspech = sv.ulozVystavu();

            if(uspech)
            {
                artefakt_nemohl_byt_pridan_textBlock.Text = "Výstava byla uložena do souboru";
                zacatek_textBox.Text = "";
                konec_textBox.Text = "";

                sv.VymazRoztvorenouVystavu();
                pridane_artefakty_dataGrid.ItemsSource = null;
            }
            else
            {
                artefakt_nemohl_byt_pridan_textBlock.Text = "Výstavu se nepodařilo uložit do souboru";
            }
        }

        private void zacatek_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SpravaVystavy sv = SpravaVystavy.Instance;
            sv.VymazRoztvorenouVystavu();
            pridane_artefakty_dataGrid.ItemsSource = null;
        }

        private void konec_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SpravaVystavy sv = SpravaVystavy.Instance;
            sv.VymazRoztvorenouVystavu();
            pridane_artefakty_dataGrid.ItemsSource = null;
        }

        private void seznamVystav_button_Click(object sender, RoutedEventArgs e)
        {
            seznamVystav_canvas.Visibility = Visibility.Visible;
            vytvoreni_vystavy_canvas.Visibility = Visibility.Hidden;

            SpravaVystavy sv = SpravaVystavy.Instance;

            List<Vystava> vystavy = sv.nactiVystavy();
            vystavy_dataGrid.ItemsSource = vystavy;
        }

        private void smazat_vystavu_button_Click(object sender, RoutedEventArgs e)
        {
            Vystava vybranaVystava = (Vystava)vystavy_dataGrid.SelectedItem;

            SpravaVystavy sv = SpravaVystavy.Instance;
            sv.SmazVystavu(vybranaVystava);

            vystavy_dataGrid.ItemsSource = sv.nactiVystavy();
        }
    }
}
