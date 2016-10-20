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
        List<Product> products;
        public Home()
        {
            InitializeComponent();
            client = new HttpClient();
            var logoutButton = new ToolbarItem
            {
                
                Name = "Logout",
                Command = new Command(this.LogoutClicked)
            };
            ToolbarItems.Add(logoutButton);
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
                    products = JsonConvert.DeserializeObject<List<Product>>(content);


                    


                }
            }
            catch (Exception ex)
            {
                //ProductsPicker.Items.Add("Dog Food");
                //ProductsPicker.Items.Add("Ketchup");
                //ProductsPicker.Items.Add("Detergent");
            }
        }
        private async void OnRegisterScaleClicked(object sender, EventArgs e)
        {
            try
            {
                // var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/login?username={0}&password={1}", txtUsername.Text, txtPassword.Text));
                var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/shelf/{0}", txtScaleID.Text));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var shelf = JsonConvert.DeserializeObject<Shelf>(content);
                    

                    
                    
                    ShelffMessage.Text = "Shelf " + txtScaleID.Text + " has been registered to your mobile profile. There are " + shelf.scales.Count + " scales attached to this shelf.";
                    Picker picker;
                    picker = new Picker();
                    foreach (var p in products)
                    {
                        picker.Items.Add(p.name + " - weight: " + p.weight + " grams");
                    }
                    Picker picker2;
                    picker2 = new Picker();
                    foreach (var s in shelf.scales)
                    {
                        picker2.Items.Add(s.id.ToString());
                    }
                    //picker.SelectedIndex = picker.Items.IndexOf(s.productId.ToString());
                    int i = 1;

                    foreach (var s in shelf.scales)
                    {
                        prodLayout.Children.Add(new Label() { Text ="Scale " + i + ":  " + products.Where(p => p.id == s.productId).FirstOrDefault().name + " Scale weight: " + s.weight  + "Capacity: " + products.Where(p => p.id == s.productId).FirstOrDefault().weight });


                        i++;
                        
                    }
                    prodLayout.Children.Add(new Label() { Text = "Change Product" });
                    prodLayout.Children.Add(picker);
                    prodLayout.Children.Add(new Label() { Text = "Scale" });
                    prodLayout.Children.Add(picker2);
                    prodLayout.Children.Add(new Button() { Text = "Update Product" });
                }
                else
                {
                    ShelffMessage.Text = "Shelf " + txtScaleID.Text + " has been registered to your mobile profile.";
                   
                }

            }
            catch (Exception ex)
            {
                var exst = ex.Message;
               // LoginMessage.Text = exst;
            }
            // await _viewModel.SaveCarAsync();
        }
        private async void LogoutClicked()
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
                    var AppNavPage = new NavigationPage(new Login())
                    {
                        BarBackgroundColor = Color.Green,
                        BarTextColor = Color.White
                    };
                    AppNavPage.Title = "Smart Shelf Mobile App!";


                    await Navigation.PushModalAsync(AppNavPage);
                //}
                //else
                //{
                //ShelffMessage.Text = "Shelf" + txtScaleID.Text + " has been registered to your mobile profile.";
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
