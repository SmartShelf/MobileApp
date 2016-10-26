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
    public partial class Login : ContentPage
    {
        HttpClient client;
        public Login()
        {
            InitializeComponent();
            client = new HttpClient();
            Title.Text = "Welcome to the Smart Shelf Mobile App!";
            
            
        }
        private void OnClearClicked(object sender, EventArgs e) {
            //_viewModel.Clear(); 
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {

            // bool success = await _viewModel.DeleteCarAsync(_viewModel.CarInstance);

            //if (success) { 
            await Navigation.PopAsync();
            LoginMessage.Text = "Registration unavailable at the moment... Work in progress";
            //}
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/login?username={0}&password={1}", txtUsername.Text, txtPassword.Text));
                //var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/products", string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(content);


                    LoginMessage.Text = "Welcome " +  loginInfo.firstName + " " + loginInfo.lastName;
                   // var AppNavPage = new NavigationPage(new Home())
                    var AppNavPage = new NavigationPage(new ShelfSelect())
                    {
                      //  BarBackgroundColor = Color.Green,
                      ///  BarTextColor = Color.White
                    };
                    
                    AppNavPage.Title = "Logout";

                  //  await Navigation.
                    await Navigation.PushModalAsync(AppNavPage);
                }
                else
                {
                    LoginMessage.Text = "Login unsuccessful. Please try again.";
                }
            }
            catch (Exception ex)
            {
                var exst = ex.Message;
                LoginMessage.Text = exst;
            }
           // await _viewModel.SaveCarAsync();
        }

    }
}
