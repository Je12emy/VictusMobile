﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Victus
{
    [Activity(Label = "Dashboard")]
    public class Dashboard : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
           
            Toast.MakeText(this, "Correo: "+ Intent.GetStringExtra("correoUsuaio"), ToastLength.Long).Show();
            SetContentView(Resource.Layout.Dashboard);
            Button btnDatos = FindViewById<Button>(Resource.Id.btnDatos);
            btnDatos.Click += delegate {
                Intent misDatos = new Intent(this, typeof(MisDatos));
                misDatos.PutExtra("correoUsuario", Intent.GetStringExtra("correoUsuaio"));
                StartActivity(misDatos);
            };
        }
    }
}