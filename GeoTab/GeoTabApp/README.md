# GeoTab Real-Time Data Viewer

This is a .NET MAUI iOS application that demonstrates how to use the GeoTab API to view real-time data for a vehicle.

## How to Use

1.  Open the `GeoTab.sln` file in Visual Studio.
2.  Open the `MainPage.xaml.cs` file.
3.  Replace the placeholder credentials in the following line of code with your GeoTab server, username, password, and database:

    ```csharp
    var api = new API("my.geotab.com", "your_username", "your_password", "your_database");
    ```

4.  Build and run the application on an iOS simulator or device.
5.  Click the "Get Real-Time Data" button to fetch and display the current latitude, longitude, and speed for the first vehicle on your account.

## Functionality

This application connects to your GeoTab account and:

1.  Retrieves the first available vehicle (device).
2.  Fetches the latest device status information for that vehicle.
3.  Displays the vehicle's current latitude, longitude, and speed.
