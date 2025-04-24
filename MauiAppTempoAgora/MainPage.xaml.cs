﻿
using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Diagnostics;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_Previsao(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n";
                        lbl_res.Text = dados_previsao;

                        string mapa = $"https://embed.windy.com/embed.html?" +
                                       $"type=map&location=coordinates&metricRain=mm&metricTemp=°C" +
                                       $"&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                                       $"&lat={t.lat.ToString().Replace(",", ".")}" +
                                       $"&lon={t.lon.ToString().Replace(",", ".")}";

                        wv_mapa.Source = mapa;
                        Debug.WriteLine(mapa);

                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsao";
                    }
                }
                else // Fecha if cidade
                {
                    lbl_res.Text = "Preenche a cidade";
                }
            }
            catch(Exception ex)
            {
                 await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
        private async void Button_Clicked_localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10)
                );

                Location? local = await Geolocation.Default.GetLocationAsync(request);
                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                                        $"Longitude: {local.Longitude} \n";
                    lbl_coords.Text = local_disp;

                    // pega nome da cidade que está nas coordenadas
                    GetCidade(local.Latitude, local.Longitude);
                }
                else
                {
                    lbl_coords.Text = "Nenhuma localização";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não Suporte", fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fnsEx)
            {
                await DisplayAlert("Erro: Localização Desabilitada", fnsEx.Message, "OK");
            }
            catch (PermissionException fnsEx)
            {
                await DisplayAlert("Erro: Premissão da Localização", fnsEx.Message, "OK");
            }
            catch (Exception fnsEx)
            {
                await DisplayAlert("Erro", fnsEx.Message, "OK");
            }
        }// Fecha Button_Clicked_localizacao

        private async void GetCidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();

                if (place != null)
                {
                    txt_cidade.Text = place.Locality;
                }
            }
             catch (Exception ex) {
                await DisplayAlert("Erro: Obtenção do nome da Cidade", ex.Message, "OK");
            }
        }
    }

 }
 

