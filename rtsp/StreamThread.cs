using System;
using System.Threading;
using OpenCvSharp;
using Dynamsoft.Barcode;
using System.Runtime.InteropServices;

namespace rtsp
{
    class StreamThread
    {
        private Thread _thread;
        private VideoCapture capture;
        private BarcodeReader reader;
        private string windowName;

        public StreamThread(string url, string window)
        {
            _thread = new Thread(new ThreadStart(this.run));
            capture = new VideoCapture(url);
            reader = new BarcodeReader("t0068MgAAALLyUZ5pborJ8XVc3efbf4XdSvDAVUonA4Z3/FiYqz1MOHaUJD3d/uBmEtXVCn9fw9WIlNw6sRT/DepkdvVW4fs=");
            windowName = window;
        }

        public StreamThread(int cameraIndex, string window)
        {
            _thread = new Thread(new ThreadStart(this.run));
            capture = new VideoCapture(cameraIndex);
            reader = new BarcodeReader("t0068MgAAALLyUZ5pborJ8XVc3efbf4XdSvDAVUonA4Z3/FiYqz1MOHaUJD3d/uBmEtXVCn9fw9WIlNw6sRT/DepkdvVW4fs=");
            windowName = window;
        }

        // Thread methods / properties
        public void Start() => _thread.Start();
        public void Join() => _thread.Join();
        public bool IsAlive => _thread.IsAlive;

        public void run()
        {
            if (capture != null && capture.IsOpened())
            {
                while (true)
                {
                    Mat image = capture.RetrieveMat();
                    Cv2.ImShow(windowName, image);

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
