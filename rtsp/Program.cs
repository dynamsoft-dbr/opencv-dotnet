using System;
using OpenCvSharp;
using Dynamsoft.Barcode;
using System.Runtime.InteropServices;

namespace rtsp
{
    class Program
    {
        static void Main(string[] args)
        {
            BarcodeReader reader = new BarcodeReader("t0068MgAAALLyUZ5pborJ8XVc3efbf4XdSvDAVUonA4Z3/FiYqz1MOHaUJD3d/uBmEtXVCn9fw9WIlNw6sRT/DepkdvVW4fs=");
            //VideoCapture capture = new VideoCapture("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
            VideoCapture capture = new VideoCapture(0);
            if (capture.IsOpened())
            {   
                while(true)
                {
                    Mat image = capture.RetrieveMat();
                    Cv2.ImShow("video", image);
                    int key = Cv2.WaitKey(20);
                    // 'ESC'
                    if (key == 27)
                    {
                        break;
                    }

                    // Read barcode
                    IntPtr data = image.Data;
                    int width = image.Width;
                    int height = image.Height;
                    int elemSize = image.ElemSize();
                    int buffer_size = width * height * elemSize;
                    byte[] buffer = new byte[buffer_size];
                    Marshal.Copy(data, buffer, 0, buffer_size);
                    BarcodeResult[] results = reader.DecodeBuffer(buffer, width, height, width * elemSize, ImagePixelFormat.IPF_RGB_888);
                    if (results != null)
                    {
                        Console.WriteLine("Total result count: " + results.Length);
                        foreach (BarcodeResult result in results)
                        {
                            Console.WriteLine(result.BarcodeText);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No barcode detected");
                    }
                    
                }
            }
        }
    }
}
