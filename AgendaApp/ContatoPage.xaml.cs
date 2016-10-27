using AgendaApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AgendaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContatoPage : Page
    {

        public Contato contato = null;

        public ContatoPage()
        {
            this.InitializeComponent();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            contato = new Models.Contato
            {
                Nome = txtNome.Text,
                Fone = txtFone.Text
            };
            this.Frame.GoBack();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
         {
            (e.Content as MainPage).contato = contato;
         }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            contato = null;
            this.Frame.GoBack();
        }
    }
}
