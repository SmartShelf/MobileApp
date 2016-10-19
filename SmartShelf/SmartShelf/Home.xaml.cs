using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SmartShelf.Entities;

using Xamarin.Forms;

namespace SmartShelf
{
    public partial class Home : ContentPage
    {

        HttpClient client;
        public Home()
        {
            InitializeComponent();
            client = new HttpClient();
            LoadProducts();
             
        }
        private async void LoadProducts()
        {
            try
            {
                var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/products", string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<Product>>(content);


                    foreach (var p in products)
                    {
                        ProductsPicker.Items.Add(p.name);
                    }


                }
            }
            catch (Exception ex)
            {
                ProductsPicker.Items.Add("Dog Food");
                ProductsPicker.Items.Add("Ketchup");
                ProductsPicker.Items.Add("Detergent");
            }
        }
        private async void OnRegisterScaleClicked(object sender, EventArgs e)
        {
            try
            {
                // var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/login?username={0}&password={1}", txtUsername.Text, txtPassword.Text));
                //var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/products", string.Empty));
                //var response = await client.GetAsync(uri);
                //if (response.IsSuccessStatusCode)
                //{
                //    var content = await response.Content.ReadAsStringAsync();
                //    var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(content);


                //    LoginMessage.Text = "Welcome " + loginInfo.firstName + " " + loginInfo.lastName;
                //    var AppNavPage = new NavigationPage(new Home())
                //    {
                //        BarBackgroundColor = Color.Green,
                //        BarTextColor = Color.White
                //    };
                //    AppNavPage.Title = "Smart Shelf! Hi";


                //    await Navigation.PushAsync(AppNavPage);
                //}
                //else
                //{
                   ShelffMessage.Text = "Shelf" + txtScaleID.Text + " has been registered to your mobile profile.";
                //}

            }
            catch (Exception ex)
            {
                var exst = ex.Message;
               // LoginMessage.Text = exst;
            }
            // await _viewModel.SaveCarAsync();
        }
    }
}
