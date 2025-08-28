using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;

namespace GeoTabApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetRealTimeDataClicked(object sender, EventArgs e)
        {
            try
            {
                // NOTE: Replace with your MyGeotab server, username and password.
                var api = new API("my.geotab.com", "your_username", "your_password", "your_database");
                await api.AuthenticateAsync();

                // Get the first device (vehicle).
                var devices = await api.CallAsync<IList<Geotab.Checkmate.ObjectModel.Device>>("Get", typeof(Geotab.Checkmate.ObjectModel.Device), new { limit = 1 });
                if (devices == null || devices.Count() == 0)
                {
                    await DisplayAlert("Error", "No devices found on your account.", "OK");
                    return;
                }
                var device = devices[0];

                // Get the device status information for the device.
                var deviceStatusInfos = await api.CallAsync<IList<DeviceStatusInfo>>("Get", typeof(DeviceStatusInfo), new
                {
                    search = new
                    {
                        deviceSearch = new { id = device.Id }
                    }
                });

                if (deviceStatusInfos == null || deviceStatusInfos.Count() == 0)
                {
                    await DisplayAlert("No Data", "No real-time data found for the selected device.", "OK");
                    return;
                }

                var deviceStatus = deviceStatusInfos[0];

                LatitudeLabel.Text = $"Latitude: {deviceStatus.Latitude}";
                LongitudeLabel.Text = $"Longitude: {deviceStatus.Longitude}";
                SpeedLabel.Text = $"Speed: {deviceStatus.Speed} km/h";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}