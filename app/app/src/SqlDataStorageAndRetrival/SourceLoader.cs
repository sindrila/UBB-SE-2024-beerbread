using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.src.SqlDataStorageAndRetrival
{
    public class SourceLoader
    {
        private static string protocol = "http://";
        private static string ip = "188.24.47.96";
        private static string port = "1444";
        private static String mp3Path = "/api/source/mp3";
        private static String pngPath = "/api/source/png";

        public SourceLoader()
        {
        }

        public static string GetMp3Path()
        {
            return protocol + ip + ":" + port + mp3Path;
        }
        public static string GetPngPath()
        {
            return protocol + ip + ":" + port + pngPath;
        }
    }
}
