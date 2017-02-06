using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShelf
{
    public class App : Application
    {
        public App()
        {
            var AppNavPage = new NavigationPage(new Login())
            {
                BarBackgroundColor = Color.Blue,
                BarTextColor = Color.White
            };
            AppNavPage.Title = "Smart Shelf! Hi";
            

            MainPage = AppNavPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
[ContentProperty("Source")]
public class ImageResourceExtension : IMarkupExtension
{
    public string Source { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Source == null)
        {
            return null;
        }
        // Do your translation lookup here, using whatever method you require
        var imageSource = ImageSource.FromResource(Source);

        return imageSource;
    }
}