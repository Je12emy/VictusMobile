using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Victus.VictusWS;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Victus
{
    [Activity(Label = "MedidasForm")]
    public class MedidasForm : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MedidasForm);
            Button btnModificarMedidas = FindViewById<Button>(Resource.Id.btnModificarMedidasForm);
            btnModificarMedidas.Click += delegate {
                EditText bicepIzquierdo = FindViewById<EditText>(Resource.Id.inputBicepIzquierdo);
                EditText bicepDerecho = FindViewById<EditText>(Resource.Id.inputBicepDerecho);
                EditText abdomen = FindViewById<EditText>(Resource.Id.inputAbdomen);
                EditText cuadricepIzquierdo = FindViewById<EditText>(Resource.Id.inputCuadricepIzquierdo);
                EditText cuadricepDerecho = FindViewById<EditText>(Resource.Id.inputCuadricepDerecho);
                EditText pantorrillaIzquierdo = FindViewById<EditText>(Resource.Id.inputPantorrillaIzquierda);
                EditText pantorrillaDerecha = FindViewById<EditText>(Resource.Id.inputPantorrillaDerecha);

                if (!string.IsNullOrWhiteSpace(bicepIzquierdo.Text) && !string.IsNullOrWhiteSpace(bicepDerecho.Text) && !string.IsNullOrWhiteSpace(abdomen.Text) && !string.IsNullOrWhiteSpace(cuadricepIzquierdo.Text) && !string.IsNullOrWhiteSpace(cuadricepDerecho.Text) && !string.IsNullOrWhiteSpace(pantorrillaIzquierdo.Text) && !string.IsNullOrWhiteSpace(pantorrillaDerecha.Text))
                {
                    string _correo = Intent.GetStringExtra("correoUsuario");
                    VictusWebService cliente = new VictusWebService();
                    DateTime fecha = DateTime.Now;
                    int i = cliente.AgregarMedidas(fecha,_correo,Convert.ToDouble(bicepIzquierdo.Text), Convert.ToDouble(bicepDerecho.Text),Convert.ToDouble(abdomen.Text),Convert.ToDouble(cuadricepIzquierdo.Text),Convert.ToDouble(cuadricepDerecho.Text),Convert.ToDouble(pantorrillaIzquierdo.Text),Convert.ToDouble(pantorrillaDerecha.Text));
                    if (i>0)
                    {
                        Toast.MakeText(this, "Se han agregado tus Datos!", ToastLength.Long).Show();
                        Intent medidas = new Intent(this, typeof(Dashboard));
                        medidas.PutExtra("correoUsuario", _correo);
                        StartActivity(medidas);
                    }else
                        Toast.MakeText(this, "Se produjo en erro al agregar tus Datos!", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(this, "No dejes espacios en blanco!", ToastLength.Long).Show();

            };


        }
    }
}