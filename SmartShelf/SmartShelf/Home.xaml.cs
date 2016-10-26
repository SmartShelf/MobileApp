﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartShelf.Entities;

using Xamarin.Forms;
using System.Net.Http.Headers;

namespace SmartShelf
{
    public partial class Home : ContentPage
    {

        HttpClient client;
        List<Product> products;
        Picker picker;
        string ShelfId;
        public Home(string shelfId)
        {
            ShelfId = shelfId;
            InitializeComponent();
            client = new HttpClient();
            var shelfButton = new ToolbarItem
            {

                Name = "Shelf Home",
                Command = new Command(this.HomeClicked)
            };
            ToolbarItems.Add(shelfButton);
            var logoutButton = new ToolbarItem
            {
                
                Name = "Logout",
                
                Command = new Command(this.LogoutClicked)
            };
            ToolbarItems.Add(logoutButton);
            LoadStatic();
            
            LoadScales();
            var seconds = TimeSpan.FromSeconds(5);

            Device.StartTimer(seconds, () => {

                // call your method to check for notifications here
                LoadScales();
                // Returning true means you want to repeat this timer
                return true;
            });

        }
        private async Task LoadStatic()
        {
            picker = new Picker();
            await LoadProducts();
            foreach (var p in products)
            {
                picker.Items.Add(p.name + " - weight: " + p.weight + " grams");
            }
            
            //picker.SelectedIndex = picker.Items.IndexOf(s.productId.ToString());
            //int i = 1;
            //shelf.scales.ForEach(delegate(Scale s))
            //    { 

            staticLayout.Children.Add(new Label() { Text = "Register New Smart Shelf Product", Font = Font.BoldSystemFontOfSize(17) });
            staticLayout.Children.Add(picker);
        }
        private async Task LoadProducts()
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
        private async void LoadScales()
        {
            try
            {
                // var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/login?username={0}&password={1}", txtUsername.Text, txtPassword.Text));
                var uri = new Uri(string.Format("http://smartshelf.mybluemix.net/main/shelf/{0}", ShelfId));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var shelf = JsonConvert.DeserializeObject<Shelf>(content);


                    prodLayout.Children.Clear();


                    
                    for (int i = 0; i < shelf.scales.Count; i++)
                    //foreach (Scale s in shelf.scales.ToList())
                    {
                        Scale s = shelf.scales[i];
                        var back = Color.Green;
                        var backText = Color.White;
                        string perc = "";
                        //if (shelf.scales[i].persentage != null)
                        //{
                        //    perc = shelf.scales[i].persentage + "%";
                        //    if (s.persentage < 15)
                        //    {
                        //        back = Color.Red;
                        //    }
                        //}
                        //else
                        //{
                        lblTitle.Text = "Details for Shelf: " + shelf.name;
                        shelf.scales[i].estimatedDate = "01/01/2019";
                        // await SaveScale(shelf.scales[i]);
                        long tempPerc = 0;

                        if (!long.TryParse(shelf.scales[i].persentage, out tempPerc) || tempPerc == 0)
                        {
                            long shelfweight = 0;
                            long productweight = 0;
                            long temp = 0;
                            long calcPerc = 0;
                            if (shelf.scales[i].weight != null && long.TryParse(shelf.scales[i].weight, out temp))
                                shelfweight = temp;
                            if (!string.IsNullOrEmpty(shelf.scales[i].productId))
                            {
                                var product = products.Where(p => p.id == shelf.scales[i].productId).FirstOrDefault();
                                if (product != null)
                                {
                                    if (long.TryParse(product.weight, out temp))
                                        productweight = temp;
                                }
                            }
                            if (productweight != 0)
                            {
                                calcPerc = shelfweight * 100 / productweight;
                                perc = string.Format("{0:0.00}", calcPerc) +"%";
                                if (calcPerc < 15)
                                {
                                    back = Color.Red;
                                }
                                else if (calcPerc < 50)
                                {

                                    back = Color.Yellow;
                                    backText = Color.Blue;
                                }
                            }
                            else
                            {
                                perc = "n/a%";
                                back = Color.Purple;
                            }
                        }
                        else
                        {
                            perc = shelf.scales[i].persentage + "%";
                            if (tempPerc < 15)
                            {
                                back = Color.Red;
                            }
                            else if (tempPerc < 50)
                            {
                                back = Color.Yellow;
                                backText = Color.Blue;
                            }
                        }
                        //  }
                        string productName = "n/a";
                        string capacity = "0";
                        if (shelf.scales[i].productId != null)
                        {
                            var prod = products.Where(p => p.id == shelf.scales[i].productId).FirstOrDefault();
                            if (prod != null)
                            {
                                if (!string.IsNullOrEmpty(prod.name))
                                    productName = prod.name;
                                if (!string.IsNullOrEmpty(prod.weight))
                                    capacity = prod.weight;
                            }
                        }
                        string scaleWeight = "0";
                        if (!string.IsNullOrEmpty(shelf.scales[i].weight))
                            scaleWeight = shelf.scales[i].weight;

                        string estmatedDate = "01/01/2017";
                        if (shelf.scales[i].estimatedDate != null)
                            estmatedDate = shelf.scales[i].estimatedDate;
                        var image = new Image
                        {
                            // Some differences with loading images in initial release.
                            Source =
                                ImageSource.FromResource("scale.png"),
                            
                           
                        };
                        // Label l1 = new Label() { Text = "Scale " + (i + 1).ToString() + ":  ", FontSize = 17 };
                        var grid = new Grid();
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                       // grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                       // grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        
                       

                        Label l2 = new Label() { Text = "Product: " + productName, Font=Font.BoldSystemFontOfSize(19), TextColor = Color.Green };
                        Label l3 = new Label() { Text = "Scale Weight: " + scaleWeight + ", Capacity Weight: " + capacity, FontSize = 14 };
                        grid.Children.Add(l2);
                        if (i == 0)
                        {
                            Button btnScale1 = new Button();
                            btnScale1.Text = "Update Product";
                            btnScale1.AutomationId = "1";
                            btnScale1.Clicked += SaveShelf;
                            btnScale1.Font = Font.SystemFontOfSize(NamedSize.Small);
                            btnScale1.BorderWidth = 1;
                            btnScale1.HorizontalOptions = LayoutOptions.Center;
                            btnScale1.VerticalOptions = LayoutOptions.CenterAndExpand;
                           // grid.Children.Add(new Label() { Text = "Select New Prodcut Above" }, 0, 0);
                            grid.Children.Add(btnScale1, 1, 0);
                            // prodLayout.Children.Add(btnScale1);
                        }
                        if (i == 1)
                        {
                            Button btnScale2 = new Button();
                            btnScale2.Text = "Update Product";
                            btnScale2.AutomationId = "2";
                            btnScale2.Font = Font.SystemFontOfSize(NamedSize.Small);
                            btnScale2.BorderWidth = 1;
                            btnScale2.HorizontalOptions = LayoutOptions.Center;
                            btnScale2.VerticalOptions = LayoutOptions.CenterAndExpand;
                            btnScale2.Clicked += SaveShelf;
                          //  grid.Children.Add(new Label() { Text = "Select New Prodcut Above" }, 0, 0);
                            grid.Children.Add(btnScale2, 1, 0);
                            //  prodLayout.Children.Add(btnScale2);
                        }
                        
                        
                       // grid.Children.Add(bottomLeft, 1, 0);
                      //  grid.Children.Add(bottomRight, 1, 1);
                        BoxView box = new BoxView() { HeightRequest = 1, WidthRequest = 1, BackgroundColor = Color.Black };

                             Label l5 = new Label() { Text = perc + " Full, Estimated Refill Date: " + estmatedDate, BackgroundColor = back, FontSize = 15, TextColor = backText };


                        //prodLayout.Children.Add(image);

                        // prodLayout.Children.Add(l1);
                       // prodLayout.Children.Add(l2);
                        prodLayout.Children.Add(grid);
                        prodLayout.Children.Add(l3);
                        
                        prodLayout.Children.Add(l5);

                        prodLayout.Children.Add(box);


                    }
                    Button btnRefresh = new Button();
                    btnRefresh.Text = "Refresh Scales";
                    btnRefresh.AutomationId = "2";
                    btnRefresh.Clicked += RefreshShelf;
                    //prodLayout.Children.Add(btnRefresh);
                    
                    
                    

                    // prodLayout.Children.Add(picker2);
                    // prodLayout.Children.Add(new Button() { Text = "Update Product" });
                }
                


            }
            catch (Exception ex)
            {
                var exst = ex.Message;
               // LoginMessage.Text = exst;
            }
            // await _viewModel.SaveCarAsync();
        }

        //public class BufferNonStreamedContentHandler : DelegatingHandler
        //{
        //    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        //                                                           System.Threading.CancellationToken cancellationToken)
        //    {
        //        var response = await base.SendAsync(request, cancellationToken);
        //        if (response.Content != null)
        //        {
        //            var services = request.GetConfiguration().Services;
        //            var bufferPolicy = (IHostBufferPolicySelector)services.GetService(typeof(IHostBufferPolicySelector));

        //            // If the host is going to buffer it anyway
        //            if (bufferPolicy.UseBufferedOutputStream(response))
        //            {
        //                // Buffer it now so we can catch the exception
        //                await response.Content.LoadIntoBufferAsync();
        //            }
        //        }
        //        return response;
        //    }
        //}
        private async void RefreshShelf(Object sender, EventArgs e)
        {
            try
            {
                LoadScales();
            }
            catch (Exception ex)
            {

            }
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
                        BarBackgroundColor = Color.Blue,
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

        private async void HomeClicked()
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
                var AppNavPage = new NavigationPage(new ShelfSelect())
                {
                    BarBackgroundColor = Color.Blue,
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

        //public async Task<Shelf> SaveShelf(Shelf shelf)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("http://smartshelf.mybluemix.net/");
        //            client.DefaultRequestHeaders.Accept.Clear();
                    
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            StringContent content = new StringContent(JsonConvert.SerializeObject(shelf), Encoding.UTF8, "application/json");
        //            // HTTP POST
        //            HttpResponseMessage response = await client.PostAsync("main/shelf", content);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string data = await response.Content.ReadAsStringAsync();
        //                shelf = JsonConvert.DeserializeObject<Shelf>(data);
        //            }
        //        }
        //        return shelf;
        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public async void SaveShelf(Object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    
                    StringContent strcontent = new StringContent("");
                  
                    HttpResponseMessage response = await client.PostAsync(new Uri("http://smartshelf.mybluemix.net/main/product/register/shelf/" + ShelfId + "/scale/" + ((Button)sender).AutomationId + "/product/" + (picker.SelectedIndex + 1).ToString() ), strcontent);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        //scale = JsonConvert.DeserializeObject<Scale>(data);
                        LoadScales();
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }

}
