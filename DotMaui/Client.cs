using System;
using System.IO;
using System.Net;
using System.Text;
using DotMaui.Models;

namespace DotMaui
{
    public class Client
    {

        private readonly string ENDPOINT = "https://api.dotmaui.com/client/";
        private readonly string CLIENT_VERSION = "1.0";
        private string apikey;

        public Client(string apikey)
        {
            this.apikey = apikey;
        }
        
        public string MinifyHTMLFromUrl(string url)
        {
            string data = String.Format("url={0}", System.Web.HttpUtility.UrlEncode(url));
            return this.makeRequest("htmlmin", data);
        }

        public string MinifyHTMLFromString(string html)
        {
            string data = String.Format("html={0}", System.Web.HttpUtility.UrlEncode(html));
            return this.makeRequest("htmlmin", data);
        }

        public string MinifyCSSFromUrl(string url)
        {
            string data = String.Format("url={0}", System.Web.HttpUtility.UrlEncode( url));
            return this.makeRequest("cssmin", data);
        }

        public string MinifyCSSFromString(string css)
        {
            string data = String.Format("css={0}", System.Web.HttpUtility.UrlEncode(css));
            return this.makeRequest("cssmin", data);
        }

        public string MinifyJSFromUrl(string url)
        {
            string data = String.Format("url={0}", System.Web.HttpUtility.UrlEncode(url));
            return this.makeRequest("jsmin", data);
        }

        public string MinifyJSFromString(string js)
        {
            string data = String.Format("js={0}", System.Web.HttpUtility.UrlEncode(js));
            return this.makeRequest("jsmin", data);
        }
        public string BeautifyJSFromUrl(string url)
        {
            string data = String.Format("url={0}", System.Web.HttpUtility.UrlEncode(url));
            return this.makeRequest("jsbeautify", data);
        }

        public string BeautifyJSFromString(string js)
        { 
        
            string data = String.Format("js={0}", System.Web.HttpUtility.UrlEncode(js));
            return this.makeRequest("jsbeautify", data);
        }

        public bool SaveImgResizedFromUrl(ImgResizerRequest req, string saveLocation)
        {

            string data_img = String.Format("url={0}", req.Url);

            if (String.IsNullOrEmpty(req.Url))
            {
                throw new Exception("Url required");
            }

            if (req.Width == 0 && req.Height == 0)
            {
                throw new Exception("Height or width required");
            }

            if (req.Width != 0)
            {
                data_img += String.Format("&width={0}", req.Width);
            }

            if (req.Height != 0)
            {
                data_img += String.Format("&height={0}", req.Height);
            }
            
            string data_request = String.Format("apikey={0}&{1}", this.apikey, data_img);
            string url_request = String.Format("{0}{1}/{2}/", this.ENDPOINT, this.CLIENT_VERSION, "imgresize");

            bool result = true;

            byte[] imageBytes;
            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(url_request);

            imageRequest.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(data_request);
            imageRequest.ContentType = "application/x-www-form-urlencoded";
            imageRequest.ContentLength = byteArray.Length;

            Stream dataStream = imageRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse imageResponse = imageRequest.GetResponse();
            Stream responseStream = imageResponse.GetResponseStream();

            using (BinaryReader br = new BinaryReader(responseStream))
            {
                imageBytes = br.ReadBytes(500000);
                br.Close();
            }

            responseStream.Close();
            imageResponse.Close();

            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            try
            {
                bw.Write(imageBytes);
            }
            catch
            {
                result = false;
            }
            finally
            {
                fs.Close();
                bw.Close();
            }

            return result;

        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/debx8sh9.aspx
        /// </summary>
        /// <returns></returns>
        private string makeRequest(string action, string postData)
        {

            string data_request = String.Format("apikey={0}&{1}", this.apikey, postData);
            string url_request = String.Format("{0}{1}/{2}/", this.ENDPOINT, this.CLIENT_VERSION, action);

            WebRequest request = WebRequest.Create(url_request);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(data_request);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            //Console.WriteLine(((HttpWebResponse)response).StatusCode);

            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            //Console.WriteLine(responseFromServer);

            reader.Close();
            dataStream.Close();
            response.Close();

            if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(responseFromServer);
            }

            return responseFromServer;

        }

    }


}
