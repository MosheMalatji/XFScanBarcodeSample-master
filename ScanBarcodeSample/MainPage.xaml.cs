using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace ScanBarcodeSample
{
    public partial class MainPage : ContentPage
    {   private string scanType;
        private ZXingScannerPage ScannerPage;
        private bool _scanning;
        private bool _tryHarder = false;
        private bool _flash = false;
        private bool _tryInverted = false;
        private bool _NativeScanning = false;
        private bool _autorotate = false;
        public MainPage()
        {
            InitializeComponent();
        }


        private void btnStart_Clicked(object sender, EventArgs e)
        {
            ShowScanner();
        }

        private async void ShowScanner()
        {

            //Grid0.IsVisible = false;
            // ScannerLayout.IsVisible = true;
            var scanner = new ZXing.Mobile.MobileBarcodeScanner
            {
                FlashButtonText = "Flash",
                UseCustomOverlay = false,
                CancelButtonText = "Cancel"
            };
            var overlay = new ZXingDefaultOverlay
            {
                TopText = string.Empty,
                BottomText = string.Empty,
                ShowFlashButton = _flash,
            };

            var options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                AutoRotate = _autorotate,
                TryHarder = _tryHarder,

                TryInverted = _tryInverted,
                UseNativeScanning = _NativeScanning,
                PossibleFormats = new List<ZXing.BarcodeFormat>()
                 { ZXing.BarcodeFormat.QR_CODE,
                ZXing.BarcodeFormat.AZTEC, ZXing.BarcodeFormat.CODE_128,  ZXing.BarcodeFormat.CODE_39,
                ZXing.BarcodeFormat.CODE_93, ZXing.BarcodeFormat.EAN_13, ZXing.BarcodeFormat.EAN_8,
                ZXing.BarcodeFormat.PDF_417, ZXing.BarcodeFormat.UPC_E}

            };

            ScannerPage = new ZXingScannerPage(options, overlay)
            {
                DefaultOverlayShowFlashButton = true,
                HasTorch = true,
                IsScanning = true,
                IsAnalyzing = true,
            };

            overlay.FlashButtonClicked += (s, ed) =>
            {
                ScannerPage.ToggleTorch();
            };

            var toolbarItem = new ToolbarItem { Text = "Cancel" };
            toolbarItem.Clicked += (s, e) =>
            {
                ScannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                });
            };

            try
            {


                ScannerPage.AutoFocus();
                ScannerPage.OnScanResult += (result) =>
                {
                    ScannerPage.IsScanning = false;

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {

                        await Navigation.PopModalAsync();
                        if (_scanning)
                            return;

                        _scanning = true;

                        if (DateTime.Now.Year < 2019)
                        {
                            return;
                        }

                        if (result != null)
                        {
                            ScannerPage.IsScanning = false;
                            scanResultText.Text = result.Text;
                        }

                        _scanning = false;
                    });
                };

                var navPage = new NavigationPage(ScannerPage);
                navPage.ToolbarItems.Add(toolbarItem);

                await Navigation.PushModalAsync(navPage);

                //    await Navigation.PushModalAsync(ScannerPage);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void swTryHarder_Toggled(object sender, ToggledEventArgs e)
        {
            if (swTryHarder.IsToggled == true)
            { _tryHarder = true; }
           
        }

        
        private void swFlashButton_Toggled(object sender, ToggledEventArgs e)
        {
            if (swFlashButton.IsToggled == true)
            { _flash = true; }
            
           
        }

        private void swTryInverted_Toggled(object sender, ToggledEventArgs e)
        {
            if(swTryInverted.IsToggled ==true)
            { _tryInverted = true; }
        }

        private void swNativeScanning_Toggled(object sender, ToggledEventArgs e)
        {
            if(swNativeScanning.IsToggled == true)
            { _NativeScanning = true; }
        }

        private void swAutoRotate_Toggled(object sender, ToggledEventArgs e)
        {
        if (swAutoRotate.IsToggled == true)
            { _autorotate = true; }
        }
    }
}
