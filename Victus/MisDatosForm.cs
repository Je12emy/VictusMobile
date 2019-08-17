using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Victus.VictusWS;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Victus
{
    [Activity(Label = "MisDatosForm")]
    public class MisDatosForm : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MisDatosForm);

            Button btnDatos = FindViewById<Button>(Resource.Id.btnDatos);
            btnDatos.Click += delegate{
                EditText inputPeso = FindViewById<EditText>(Resource.Id.inputPeso);
                EditText inputAltura = FindViewById<EditText>(Resource.Id.inputAltura);
                EditText inputEdad = FindViewById<EditText>(Resource.Id.inputEdad);
                Toast.MakeText(this, "Correo: " + Intent.GetStringExtra("correoUsuario"), ToastLength.Long).Show();
                if (!string.IsNullOrEmpty(inputPeso.Text) && !string.IsNullOrEmpty(inputAltura.Text) && !string.IsNullOrEmpty(inputEdad.Text))
                {
                    
                    VictusWebService cliente = new VictusWebService();

                    DateTime fecha = DateTime.Now;
                    // Calculos
                    // IMC = Peso / Altura^2
                    double imc =  Convert.ToDouble(inputPeso.Text) / Math.Pow(Convert.ToDouble(inputAltura.Text),2) * 100;
                    // // Peso en KG/7 y redondeo a Entero
                    int agua = Convert.ToInt32(Math.Round(Convert.ToDouble(inputPeso.Text) / 7));

                    int i = cliente.AgregarDatosCliente(Intent.GetStringExtra("correoUsuario").ToString(), Convert.ToDouble(inputPeso.Text), Convert.ToInt32(inputAltura.Text), Convert.ToInt32(inputEdad.Text), imc, agua, fecha);
                    if (i > 0)
                    {
                        {
                            Toast.MakeText(this, "Se han agregado tus Datos!", ToastLength.Long).Show();
                            Intent misDatos = new Intent(this, typeof(MisDatos));
                            misDatos.PutExtra("correoUsuario", Intent.GetStringExtra("correoUsuario"));
                            StartActivity(misDatos);
                        }
                    }
                    else
                        Toast.MakeText(this, "Se ha dado un error al agregar tus Datos", ToastLength.Long).Show();

                }
                else
                    Toast.MakeText(this, "Por favor rellena todos los espacios!", ToastLength.Long).Show();
            };
        }
    }
}