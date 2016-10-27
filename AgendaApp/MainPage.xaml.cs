using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AgendaApp.Models;
using Windows.ApplicationModel.Calls;
using Windows.UI.Popups;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AgendaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Contato contato = null;
        public Contatos agenda = new Contatos();
        private string arquivo = "Agenda.xml";

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            listView.ItemsSource = agenda;
            abrirArquivo();
        }
        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            /*contato = new Contato { Nome = "Gilson",
                Fone = "1234-5678" 
            };
            agenda.Add(contato);*/

            this.Frame.Navigate(typeof(ContatoPage));
        }

        private async void listView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                contato = listView.SelectedItem as Contato;
                PhoneCallStore phoneCallStore = 
                    await PhoneCallManager.RequestStoreAsync();
                Guid lineGuid = await phoneCallStore.GetDefaultLineAsync();
                MessageDialog msg = new MessageDialog(lineGuid.ToString());
                await msg.ShowAsync();
                PhoneLine phoneLine = await PhoneLine.FromIdAsync(lineGuid);
                phoneLine.Dial(contato.Fone, contato.Nome);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (contato != null) agenda.Add(contato);
        }

        private void abrirArquivo()
        {
            IsolatedStorageFile file; IsolatedStorageFileStream stream;
            XmlSerializer xml;
            using (file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists(arquivo))
                    using (stream = file.OpenFile(arquivo, FileMode.Open))
                    {
                        xml = new XmlSerializer(typeof(Contatos));
                        foreach (Contato c in (Contatos)xml.Deserialize(stream))
                            agenda.Add(c);
                    }
            }
        }

        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            salvarArquivo();
            MessageDialog msg = new MessageDialog("Arquivo salvo com sucesso!");
            await msg.ShowAsync();
        }

        private void salvarArquivo()
        {
            IsolatedStorageFile file;
            IsolatedStorageFileStream stream;
            XmlSerializer xml;
            using (file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (stream = file.OpenFile(arquivo, FileMode.Create))
                {
                    xml = new XmlSerializer(typeof(Contatos));
                    xml.Serialize(stream, agenda);
                }
            }
        }
    }
}
