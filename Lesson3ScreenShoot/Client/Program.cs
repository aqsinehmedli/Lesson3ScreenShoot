
//using System.Drawing;
//using System.Net;
//using System.Net.Sockets;

//Console.WriteLine("Client");

//var ipAdress = IPAddress.Parse("192.168.1.99");
//var port = 27001;
//var endPoint = new IPEndPoint(ipAdress, port);

//while (true)
//{
//    var client = new TcpClient();

//    try
//    {
//        client.Connect(endPoint);
//        if (client.Connected)
//        {
//            Console.WriteLine("Client baglandi..");
//            Bitmap memoryImage;
//            memoryImage = new Bitmap(1920, 1080);
//            Size s = new Size(memoryImage.Width, memoryImage.Height);

//            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

//            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

//            var networkStream = client.GetStream();
//            using (var memoryStream = new MemoryStream())
//            {
//                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
//                string path = $"ScreenShot_{timestamp}.png";
//                memoryImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
//                var buffer = memoryStream.ToArray();
//                networkStream.Write(buffer, 0, buffer.Length);
//                var fileNameBytes = System.Text.Encoding.UTF8.GetBytes(path);
//                networkStream.Write(fileNameBytes, 0, fileNameBytes.Length);
//            }
//            networkStream.Close();
//            client.Close();
//        }

//    }
//    catch (Exception ex)
//    {

//        Console.WriteLine(ex.Message);
//    }
//    Thread.Sleep(10000);

//}

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

Console.WriteLine("Client");

var ipAddress = IPAddress.Parse("192.168.1.99");
var port = 27001;
var endPoint = new IPEndPoint(ipAddress, port);

while (true)
{
    using (var client = new TcpClient())
    {
        try
        {
            client.Connect(endPoint);
            if (client.Connected)
            {
                Console.WriteLine("Client bağlı..");

                using (Bitmap memoryImage = new Bitmap(1920, 1080))
                {
                    using (Graphics memoryGraphics = Graphics.FromImage(memoryImage))
                    {
                        memoryGraphics.CopyFromScreen(0, 0, 0, 0, memoryImage.Size);
                    }

                    var networkStream = client.GetStream();
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string path = $"ScreenShot_{timestamp}.png";
                    using (var memoryStream = new MemoryStream())
                    {
                        

                        memoryImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        var buffer = memoryStream.ToArray();
                        networkStream.Write(buffer, 0, buffer.Length);

                        var fileNameBytes = System.Text.Encoding.UTF8.GetBytes(path);
                        networkStream.Write(fileNameBytes, 0, fileNameBytes.Length);
                    }


                    Console.WriteLine($"Ekran görüntüsü kaydedildi: {path}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata: " + ex.Message);
        }

        Thread.Sleep(10000); 
    }
}

