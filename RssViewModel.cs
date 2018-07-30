using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RssReader
{
    class RssViewModel : INotifyPropertyChanged
    {
        String url = "http://api.heute.at/rss/view/1";
        XDocument doc = XDocument.Load("http://api.heute.at/rss/view/1");




        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                // Button wird erst aktiviert, wenn der Benutzer aus dem 
                // Textfeld rausklickt (ungut....)
                // Alternative: CanExecute liefert immer true, in der Execute
                // Methode wird abgefragt, ob die Url leer ist.
                url = value;

                try
                {
                    doc = XDocument.Load(url);
                    
                }


                catch(ArgumentException e)
                {
                    MessageBox.Show("No URL was found");
                }
                catch (WebException e)
                {
                    MessageBox.Show("File Not Found");
                }

                catch (XmlException e)
                {
                    MessageBox.Show("Something went wrong");
                }

                // Bei ORF Feed geht wegen rdf:RDF Tag nix

                
                PropertyChanged(this,
                    new PropertyChangedEventArgs(nameof(LoadCommand)));
            }
        }

        public List<string> Titles
        {
            
            get
            {
                
                    var titles = (from t in doc.XPathSelectElements("//item/title") select t.Value).ToList();
                    

                    if(!titles.Any())
                    {
                    MessageBox.Show("The RSS feed is empty or the xml has inappropriate format");
                    }

                return titles;                        
            }

            set
            {
                

            }

        }

        private string selectedTitle = "Bitte wählen";
        public string SelectedTitle
        {
            get
            {
                return selectedTitle;
            }
            set
            {
                // Die Variable inhalt auf den Inhalt
                // des gewählten Artikels setzen.
                selectedTitle = value;
                PropertyChanged(this, 
                    new PropertyChangedEventArgs(nameof(Inhalt)));
            }
        }

        private string inhalt;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Inhalt
        {
            get
            {

                if (url != "")
                {
                    

                    var descr = from d in doc.XPathSelectElements("//item")
                                where d.Element("title").Value == selectedTitle
                                select d.Element("description").Value;


                    //select d.Element("/description").Value;




                    // return inhalt
                    return descr.SingleOrDefault();
                }

                else
                {
                    return null;
                }

            }

        }


         

        public ICommand LoadCommand
        {
            get
            {
                return new WpfButton(
                    loadCommandExecute,    // Execute Methode
                    () => Url != ""        // CanExecute, entspricht
                                           // loadCommandCanExecute
                );
            }
        }

        private bool loadCommandCanExecute()
        {
            return Url != "";
        }

        private void loadCommandExecute()
        {

            url = Url;

            PropertyChanged(this,
                new PropertyChangedEventArgs(nameof(Titles)));
        }

    }
}
