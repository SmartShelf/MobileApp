using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SmartShelf.Entities;

using Xamarin.Forms;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace SmartShelf
{
    public partial class ShelfSelect : ContentPage
    {
        HttpClient client;
        List<Shelf> shelves;
        Button bReg;
        public ShelfSelect()
        {
            try
            {
                InitializeComponent();
                registerLayout.IsVisible = false;
                bReg = new Button() { Text = "Register a New Shelf" };
                client = new HttpClient();
                var logoutButton = new ToolbarItem
                {

                    Name = "Logout",
                    Command = new Command(this.LogoutClicked)
                };
                ToolbarItems.Add(logoutButton);
                LoadShelves();

            }
            catch (Exception ex)
            {
                string m = ex.Message;
            }

        }
        private async void OnRegisterShelfClicked(Object sender, EventArgs e)
        {
            try
            {
                // var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/login?username={0}&password={1}", txtUsername.Text, txtPassword.Text));
                var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/shelf", string.Empty));
                string url = "http://smartshelf.mybluemix.net/main/shelf";
                //client.ContentType = "application/json";
                string postBody = JsonConvert.SerializeObject(new { Id = "8888888", name = "testing 123..." });
                string postData = "{ \"id\": \"" + txtScaleID.Text + "\", \"name\": \"" + txtDescription.Text + "\" }";
                await DoAsyncPut(url, postData);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage response = await client.PutAsync(uri, new StringContent(postBody, Encoding.UTF8, "application/json"));
                ////var response = await client.GetAsync(uri);
                //if (response.IsSuccessStatusCode)
                //{
                //    //    var content = await response.Content.ReadAsStringAsync();
                //    //    var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(content);

                //    string ssdfsdf = "";
                //}
                //LoginMessage.Text = "Welcome " + loginInfo.firstName + " " + loginInfo.lastName;
                LoadShelves();
                //}
                //else
                //{
                ShelffMessage.Text = "Shelf " + txtScaleID.Text + " has been registered to your mobile profile.";
                registerLayout.IsVisible = false;
                bReg.IsVisible = true;
                //}

            }
            catch (Exception ex)
            {
                var exst = ex.Message;
                // LoginMessage.Text = exst;
            }
        }
        private async void ShowRegister(Object sender, EventArgs e)
        {
            registerLayout.IsVisible = true;
            bReg.IsVisible = false;
        }
        private async void HideRegister(Object sender, EventArgs e)
        {
            registerLayout.IsVisible = false;
            bReg.IsVisible = true;
        }
        private async void OnShelfClicked(Object sender, EventArgs e)
        {
            try
            {
                
                var AppNavPage = new NavigationPage(new Home(((Button)sender).AutomationId))
                {
                    BarBackgroundColor = Color.Blue,
                    BarTextColor = Color.White
                };
                AppNavPage.Title = "Smart Shelf Mobile App!" + ((Button)sender).AutomationId;

                await Navigation.PushModalAsync(AppNavPage);

               

            }
            catch (Exception ex)
            {
                var exst = ex.Message;
               
            }
        }
        private async void LoadShelves()
        {
            try
            {
                var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/shelfs", string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    shelves = JsonConvert.DeserializeObject<List<Shelf>>(content);
                    string desc = string.Empty;
                    shelfLayout.Children.Clear();
                    shelfLayout.Children.Add(new Label { Text = "Registered Smart Shelves:", TextColor=Color.Black, FontSize=16 });
                    if (shelves.Count == 0)
                        shelfLayout.Children.Add(new Label { Text = "None." });
                    foreach (var s in shelves)
                    {
                        if (string.IsNullOrEmpty(s.name))
                            desc = "Shelf " + s.id;
                        else
                            desc = s.name;
                        shelfLayout.Children.Add(new Label() { Text = desc, FontSize = 22, TextColor = Color.Green });
                        Button b = new Button();
                        b.Text = "Monitor Scales";
                        b.AutomationId = s.id.ToString();
                        b.Clicked += OnShelfClicked;
                        shelfLayout.Children.Add(b);

                    }
                    BoxView box = new BoxView() { HeightRequest = 1, WidthRequest = 1, BackgroundColor = Color.Black };
                    
                    bReg.Clicked += ShowRegister;
                    shelfLayout.Children.Add(box);
                    shelfLayout.Children.Add(bReg);


                }
            }
            catch (Exception ex)
            {
                
                return;
               
            }
        }
        private async Task DoAsyncPut(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            
            byte[] postBytes = System.Text.UTF8Encoding.UTF8.GetBytes(postData);
            var content = new ByteArrayContent(postBytes);
            content.Headers.ContentLength = postBytes.Length;
            
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            using (var client = new HttpClient())
            {
                await client.PutAsync(url, content);
            }

            
        }
        private async void LogoutClicked()
        {
            try
            {
                var AppNavPage = new NavigationPage(new Login())
                {
                    BarBackgroundColor = Color.Blue,
                    BarTextColor = Color.White
                };
                AppNavPage.Title = "Smart Shelf Mobile App!";


                await Navigation.PushModalAsync(AppNavPage);
                

            }
            catch (Exception ex)
            {
                var exst = ex.Message;
                
            }

        }
    }
}
