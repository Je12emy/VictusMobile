using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;

namespace Victus
{
    [Activity(Label = "Victus", MainLauncher = true)]
    public class MainActivity : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button btnRegistro = FindViewById<Button>(Resource.Id.btnRegistrarme);
            btnRegistro.Click += delegate {
                Intent registro;
                registro = new Intent(this, typeof(Registro));
                StartActivity(registro);
            };

            Button btnIngresar = FindViewById<Button>(Resource.Id.btnIngresar);
            btnIngresar.Click += delegate {
                Intent ingresar;
                ingresar = new Intent(this, typeof(Login));
                StartActivity(ingresar);
            };
        }
    }
}

