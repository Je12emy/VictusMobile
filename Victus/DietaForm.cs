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
    [Activity(Label = "DietaForm")]
    public class DietaForm : Activity
    {
        string opcion;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DietaForm);

            RadioButton radioBajar = FindViewById<RadioButton>(Resource.Id.radioBajarPeso);
            RadioButton radioMantener = FindViewById<RadioButton>(Resource.Id.radioMantenerPeso);
            RadioButton radioSubir = FindViewById<RadioButton>(Resource.Id.radioSubirPeso);

            opcion = radioBajar.Text;

            radioBajar.Click += radioClick;
            radioMantener.Click += radioClick;
            radioSubir.Click += radioClick;

            Button btnModificarDieta = FindViewById<Button>(Resource.Id.btnModificarDietaForm);
            btnModificarDieta.Click += delegate {
                VictusWebService cliente = new VictusWebService();
                DataTable tabla = new DataTable();

                string _correo = Intent.GetStringExtra("correoUsuario");
                tabla = cliente.BuscarUltimoRegistroHarris(Intent.GetStringExtra("correoUsuario"));
                if (!string.IsNullOrWhiteSpace(tabla.Rows[0][0].ToString()))
                {
                    DateTime fecha;
                    fecha = Convert.ToDateTime(tabla.Rows[0][0]);
                    DateTime fechaDieta = DateTime.Now;
                    tabla = cliente.BuscarRegistroHarris(_correo, fecha.AddHours(-1));
                    if (tabla.Rows.Count > 0)
                    {
                        double calorias = Convert.ToDouble(tabla.Rows[0][3]);
                        int i = cliente.AgregarDieta(_correo,fechaDieta, Convert.ToInt32(tabla.Rows[0][0]),opcion);
                        if (i > 0)
                        {
                            Toast.MakeText(this, "Se esta procesando tu nueva dieta", ToastLength.Long).Show();
                            tabla = cliente.ObtenerUltimaDieta(_correo);
                            if (!string.IsNullOrWhiteSpace(tabla.Rows[0][0].ToString()))
                            {
                                fecha = Convert.ToDateTime(tabla.Rows[0][0]);
                                tabla = cliente.ObtenerDieta(_correo,fecha.AddHours(-1));
                                if (tabla.Rows.Count > 0)
                                {
                                    int codigoDieta = Convert.ToInt32(tabla.Rows[0][0]);
                                    tabla = cliente.ObtenerCatalogoAlimentos();

                                    int _calorias = 0;
                                    if (opcion == "Bajar Peso")
                                    {
                                        opcion = "bajar";
                                        calorias = Math.Round(calorias - 500);
                                        _calorias = Convert.ToInt32(calorias);

                                    } else if (opcion == "Mantener Peso") {
                                        opcion = "mantener";
                                        _calorias = Convert.ToInt32(Math.Round(calorias));
                                    }
                                    else if (opcion == "Subir Peso")
                                    {
                                        opcion = "subir";
                                        calorias = Math.Round(calorias + 500);
                                        _calorias = Convert.ToInt32(calorias);
                                    }

                                    int acumuladorCalorias = 0;
                                    Random r = new Random();
                                    int Indice;
                                    int codigoAlimento;
                                    
                                    while (acumuladorCalorias <= _calorias)
                                    {
                                        Indice = r.Next(0, tabla.Rows.Count);
                                        acumuladorCalorias = acumuladorCalorias + Convert.ToInt32(tabla.Rows[Indice][2]);
                                        codigoAlimento = Convert.ToInt32(tabla.Rows[Indice][0]);

                                        cliente.AgregarRelacion(codigoDieta, codigoAlimento);
                                    }
                                    Intent dashboard = new Intent(this, typeof(Dashboard));
                                    dashboard.PutExtra("correoUsuario", Intent.GetStringExtra("correoUsuario").ToString());
                                    StartActivity(dashboard);
                                }
                                else
                                    Toast.MakeText(this, "Se produjo un error al procesar tu dieta", ToastLength.Long).Show();
                                System.Diagnostics.Debug.WriteLine(tabla.Rows[0][0].ToString());

                            }
                            else
                                Toast.MakeText(this, "Se produjo un error al procesar tu nueva dieta", ToastLength.Long).Show();
                        }
                        else
                            Toast.MakeText(this, "Se produjo un error al procesar tu nueva dieta", ToastLength.Long).Show();
                    }
                }
                else
                    Toast.MakeText(this, "Parece que no tienes tu Nivel Calorico", ToastLength.Long).Show();
            };

        }
        private void radioClick(object sender, EventArgs e) {
            RadioButton seleccion = (RadioButton)sender;
            opcion = seleccion.Text;
        }
    }
}