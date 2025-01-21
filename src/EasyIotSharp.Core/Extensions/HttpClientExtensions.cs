using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using static EasyIotSharp.Core.GlobalConsts;

namespace EasyIotSharp.Core.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// 请求发送链接
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T PostFormData<T>(string url, NameValueCollection parameters)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            var parassb = new StringBuilder();
            foreach (string key in parameters.Keys)
            {
                if (parassb.Length > 0)
                    parassb.Append("&");

                parassb.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(parameters[key]));
            }
            var data = Encoding.UTF8.GetBytes(parassb.ToString());
            var reqstream = req.GetRequestStream();
            reqstream.Write(data, 0, data.Length);
            reqstream.Close();
            string result;
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream: req.GetResponse().GetResponseStream(), encoding: Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>
        /// http/https请求响应
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url">地址（要带上http或https）</param>
        /// <param name="headers">请求头</param>
        /// <param name="parameters">提交数据</param>
        /// <param name="dataEncoding">编码类型 utf-8</param>
        /// <param name="contentType">application/x-www-form-urlencoded</param>
        /// <returns></returns>
        public static T Request<T>(string method, string url, Dictionary<string, string> parameters, Encoding dataEncoding, Dictionary<string, string> headers = null, string contentType = "application/json")
        {
            method = method.ToUpper();
            var request = CreateRequest(method, url, headers, parameters, dataEncoding, contentType);
            //如果需要POST数据
            if (method == KnowHttpMethods.POST && !(parameters == null || parameters.Count == 0))
            {
                var data = FormatPostParameters(parameters, dataEncoding, contentType);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
            WebResponse res = null;
            HttpWebResponse webResponse = null;
            try
            {
                res = request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            catch (Exception e)
            {
                throw e;
            }
            if (res.IsNull())
            {
                return default(T);
            }

            webResponse = (HttpWebResponse)res;
            var streamRep = webResponse.GetResponseStream();
            byte[] arr = streamRep.ReadFully();
            var result = Encoding.UTF8.GetString(arr);
            webResponse.Close();
            streamRep.Close();
            return result.DeserializeFromJson<T>();
        }

        /// <summary>
        /// http/https请求响应
        /// </summary>
        /// <param name="url">地址（要带上http或https）</param>
        /// <param name="headers">请求头</param>
        /// <param name="json">提交数据</param>
        /// <param name="dataEncoding">编码类型 utf-8</param>
        /// <param name="contentType">application/x-www-form-urlencoded</param>
        /// <returns></returns>
        public static T RequestPost<T>(string url, string json, Encoding dataEncoding, Dictionary<string, string> headers = null, string contentType = "application/json")
        {
            HttpWebRequest request = null;
            //判断是否是https
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (contentType == null)
            {
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            }
            else
            {
                request.ContentType = contentType;
            }
            request.Method = "POST";
            //POST的数据大于1024字节的时候，如果不设置会分两步
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.ConnectionLimit = int.MaxValue;

            if (headers != null)
            {
                FormatRequestHeaders(headers, request);
            }

            //如果需要POST数据
            if (json.IsNotNullOrWhiteSpace())
            {
                byte[] data = dataEncoding.GetBytes(json);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
            WebResponse res = null;
            HttpWebResponse webResponse = null;
            try
            {
                res = request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            catch (Exception e)
            {
                throw e;
            }
            if (res.IsNull())
            {
                return default(T);
            }

            webResponse = (HttpWebResponse)res;
            var streamRep = webResponse.GetResponseStream();
            byte[] arr = streamRep.ReadFully();
            var result = Encoding.UTF8.GetString(arr);
            webResponse.Close();
            streamRep.Close();
            return result.DeserializeFromJson<T>();
        }

        /// <summary>
        /// http/https请求响应
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="dataEncoding"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static HttpWebResponse RequestPost(string url, string json, Encoding dataEncoding, Dictionary<string, string> headers = null, string contentType = "application/json")
        {
            HttpWebRequest request = null;
            //判断是否是https
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (contentType == null)
            {
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            }
            else
            {
                request.ContentType = contentType;
            }
            request.Method = "POST";
            //POST的数据大于1024字节的时候，如果不设置会分两步
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.ConnectionLimit = int.MaxValue;

            if (headers != null)
            {
                FormatRequestHeaders(headers, request);
            }

            //如果需要POST数据
            if (json.IsNotNullOrWhiteSpace())
            {
                byte[] data = dataEncoding.GetBytes(json);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }

            WebResponse Res = null;
            try
            {
                Res = request.GetResponse();
            }
            catch (WebException ex)
            {
                Res = (HttpWebResponse)ex.Response;
            }
            catch (Exception e)
            {
                throw e;
            }

            if (null == Res)
            {
                return request.GetResponse() as HttpWebResponse;
            }
            return (HttpWebResponse)Res;
        }

        /// <summary>
        /// http上传文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="buffer"></param>
        /// <param name="paramName"></param>
        /// <param name="contentType"></param>
        /// <param name="nameValueCollection"></param>
        /// <returns></returns>
        public static T HttpUploadFile<T>(string url, byte[] buffer, string paramName, string contentType = "image/jpeg", NameValueCollection nameValueCollection = null)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            Stream requestStream = request.GetRequestStream();
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            if (nameValueCollection.IsNotNull())
            {
                foreach (string key in nameValueCollection.Keys)
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nameValueCollection[key]);
                    byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }
            }
            requestStream.Write(boundarybytes, 0, boundarybytes.Length);
            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, paramName, contentType);
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            requestStream.Write(headerbytes, 0, headerbytes.Length);

            requestStream.Write(buffer, 0, buffer.Length);

            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();
            WebResponse webResponse = null;
            var res = default(T);
            try
            {
                webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                var result = streamReader.ReadToEnd();
                res = result.DeserializeFromJson<T>();
            }
            catch (Exception ex)
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
            }
            finally
            {
                request = null;
            }
            return res;
        }

        /// <summary>
        /// 创建HTTP请求对象
        /// </summary>
        /// <param name="getOrPost"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="parameters"></param>
        /// <param name="paraEncoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateRequest(string getOrPost, string url, Dictionary<string, string> headers, Dictionary<string, string> parameters, Encoding paraEncoding, string contentType)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (parameters != null && parameters.Count > 0 && paraEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }

            HttpWebRequest request = null;
            //判断是否是https
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (getOrPost == "GET")
            {
                request.Method = "GET";

                if (parameters != null && parameters.Count > 0)
                {
                    url = FormatGetParametersToUrl(url, parameters, paraEncoding);
                }
            }
            else
            {
                request.Method = "POST";
            }

            if (contentType == null)
            {
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            }
            else
            {
                request.ContentType = contentType;
            }

            //POST的数据大于1024字节的时候，如果不设置会分两步
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.ConnectionLimit = int.MaxValue;

            if (headers != null)
            {
                FormatRequestHeaders(headers, request);
            }

            return request;
        }

        /// <summary>
        /// 格式化请求头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="request"></param>
        private static void FormatRequestHeaders(Dictionary<string, string> headers, HttpWebRequest request)
        {
            foreach (var hd in headers)
            {
                //因为HttpWebRequest中很多标准标头都被封装成只能通过属性设置，添加的话会抛出异常
                switch (hd.Key.ToLower())
                {
                    case "connection":
                        request.KeepAlive = false;
                        break;

                    case "content-type":
                        request.ContentType = hd.Value;
                        break;

                    case "transfer-enconding":
                        request.TransferEncoding = hd.Value;
                        break;

                    default:
                        request.Headers.Add(hd.Key, hd.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// 格式化Get请求参数
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="parameters">参数</param>
        /// <param name="paraEncoding">编码格式</param>
        /// <returns></returns>

        private static string FormatGetParametersToUrl(string url, Dictionary<string, string> parameters, Encoding paraEncoding)
        {
            if (url.IndexOf("?") < 0)
                url += "?";
            int i = 0;
            string sendContext = "";
            foreach (var parameter in parameters)
            {
                if (i > 0)
                {
                    sendContext += "&";
                }

                sendContext += HttpUtility.UrlEncode(parameter.Key, paraEncoding)
                       + "=" + HttpUtility.UrlEncode(parameter.Value, paraEncoding);
                ++i;
            }

            url += sendContext;
            return url;
        }

        /// <summary>
        /// 格式化Post请求参数
        /// </summary>
        /// <param name="parameters">编码格式</param>
        /// <param name="dataEncoding">编码格式</param>
        /// <param name="contentType">类型</param>
        /// <returns></returns>
        private static byte[] FormatPostParameters(Dictionary<string, string> parameters, Encoding dataEncoding, string contentType)
        {
            List<string> list = new List<string>();
            foreach (var item in parameters)
            {
                list.Add(string.Format("\"{0}\":\"{1}\"", item.Key, item.Value));
            }
            string sendContext = "{" + list.JoinAsString(",") + "}";

            byte[] data = dataEncoding.GetBytes(sendContext);
            return data;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }

        /// <summary>
        ///  获取响应的json数据
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public static string GetResponseStringJson(this HttpWebResponse httpResponse)
        {
            string resp = string.Empty;
            if (httpResponse.IsNull())
            {
                return resp;
            }
            using (Stream s = httpResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                resp = reader.ReadToEnd();
                reader.Close();
                return resp;
            }
        }

        /// <summary>
        /// 数据流转byte数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadFully(this Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}