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
                if(!string.IsNullOrEmpty(txt_cidade.Text))
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
                        lbl_resp.Text = "Sem dados de Previsao";
                    }
                    else{
                        lbl_resp.Text = "Preenche a cidade";
                    }
                 catch(Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");

            }

        }

            


        private void Button_Clicked_localizacao(object sender, EventArgs e)
        {

        }
     }

 }
 

