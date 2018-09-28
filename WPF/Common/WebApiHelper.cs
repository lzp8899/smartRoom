using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace Web
{
    /// <summary>
    /// WebApi 访问帮助类
    /// </summary>
    public static class WebApiHelper
    {
        private static Dictionary<string, string> netAddressMapper;

        static WebApiHelper()
        {
            ServicePointManager.DefaultConnectionLimit = 500;
            netAddressMapper = new Dictionary<string, string>();
        }

        /// <summary>
        /// 获取JSON数据
        /// </summary>
        /// <typeparam name="T">请求参数类型</typeparam>
        /// <typeparam name="R">返回数据类型</typeparam>
        /// <param name="netAddress">请求地址，若以http://或https://开头使用此地址，否则使用Api网关地址</param>
        /// <param name="route">路由地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="requestAction">请求动作，默认为null</param>
        /// <param name="responseAction">返回动作，默认为null</param>
        /// <returns>返回数据</returns>
        public static R JsonRequest<T, R>(string netAddress, string route, HttpModel method, T parameters, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            return JsonRequest<T, R>(netAddress, route, method, parameters, out CookieCollection responseCookies, requestAction, responseAction);
        }

        /// <summary>
        /// 获取JSON数据
        /// </summary>
        /// <typeparam name="T">请求参数类型</typeparam>
        /// <typeparam name="R">返回数据类型</typeparam>
        /// <param name="netAddress">请求地址，若以http://或https://开头使用此地址，否则使用Api网关地址</param>
        /// <param name="route">路由地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="reponseCookies">请求参数</param>
        /// <param name="requestAction">请求动作，默认为null</param>
        /// <param name="responseAction">返回动作，默认为null</param>
        /// <returns>返回数据</returns>
        public static R JsonRequest<T, R>(string netAddress, string route, HttpModel method, T parameters, out CookieCollection reponseCookies, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            Exception requestEx = null;

            string url = GetHttpUrl(netAddress, route, out string key, requestAction);
            if (!String.IsNullOrEmpty(url))
            {
                try
                {
                    return JsonRequest<T, R>(url, method, parameters, out reponseCookies, requestAction, responseAction);
                }
                catch (Exception ex)
                {
                    requestEx = ex;
                }
            }

            //IApplicationContext applicationContext = DIManager.Instance.Get<IApplicationContext>();
            //if (!netAddress.StartsWith("http://") && !netAddress.StartsWith("https://") && applicationContext.FrameworkApiGate)
            //{
            //    string[] urls = GetHttpUrl(netAddress, route);

            //    for (int i = 0; i < urls.Length; i++)
            //    {
            //        try
            //        {
            //            R r = JsonRequest<T, R>(urls[i], method, parameters, out reponseCookies, requestAction, responseAction);
            //            UpdateNetAddressMapper(key, urls[i]);
            //            return r;
            //        }
            //        catch (Exception ex)
            //        {
            //            if (i == urls.Length - 1)
            //                throw ex;
            //        }
            //    }
            //}

            if (requestEx != null)
                throw requestEx;
            else
                throw new Exception("无可用服务地址");
        }

        private static R JsonRequest<T, R>(string url, HttpModel method, T parameters, out CookieCollection reponseCookies, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            reponseCookies = new CookieCollection();
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                AddWebRequestInfo(request, method.ToString());

                if (requestAction != null)
                    requestAction(request);

                AddWebParameters(request, parameters);

                response = request.GetResponse() as HttpWebResponse;
                if (responseAction != null)
                    responseAction(response);

                reponseCookies = response.Cookies;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string json = GetHttpResault(response);
                    if (!String.IsNullOrEmpty(json))
                    {
                        R rtData = JsonConvert.DeserializeObject<R>(json);
                        return rtData;
                    }
                    else
                    {
                        return default(R);
                    }
                }
                else
                {
                    throw GetResponseError(request, response);
                }
            }
            catch (WebException ex)
            {
                throw GetWebError(ex);
            }
            finally
            {
                if (response != null)
                    response.Close();

                if (request != null)
                    request.Abort();
            }
        }

        /// <summary>
        /// 发送 HTTP API 请求
        /// </summary>
        /// <param name="netAddress">请求地址，若以http://或https://开头使用此地址，否则使用Api网关地址</param>
        /// <param name="route">路由地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="requestAction">请求动作，默认为null</param>
        /// <param name="responseAction">返回动作，默认为null</param>
        public static void JsonRequest<T>(string netAddress, string route, HttpModel method, T parameters, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            Exception requestEx = null;

            string url = GetHttpUrl(netAddress, route, out string key, requestAction);
            if (!String.IsNullOrEmpty(url))
            {
                try
                {
                    JsonRequest<T>(url, method, parameters, requestAction, responseAction);
                    return;
                }
                catch (Exception ex)
                {
                    requestEx = ex;
                }
            }

            if (requestEx != null)
                throw requestEx;
            else
                throw new Exception("无可用服务地址");
        }

        private static void JsonRequest<T>(string url, HttpModel method, T parameters, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                AddWebRequestInfo(request, method.ToString());
                if (requestAction != null)
                    requestAction(request);

                AddWebParameters(request, parameters);

                response = request.GetResponse() as HttpWebResponse;
                if (responseAction != null)
                    responseAction(response);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw GetResponseError(request, response);
                }
            }
            catch (WebException ex)
            {
                throw GetWebError(ex);
            }
            finally
            {
                if (response != null)
                    response.Close();

                if (request != null)
                    request.Abort();
            }
        }

        /// <summary>
        /// 获取JSON数据
        /// </summary>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="netAddress">请求地址，若以http://或https://开头使用此地址，否则使用Api网关地址</param>
        /// <param name="route">路由地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="requestAction">请求动作，默认为null</param>
        /// <param name="responseAction">返回动作，默认为null</param>
        public static R JsonRequest<R>(string netAddress, string route, HttpModel method, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            Exception requestEx = null;

            string url = GetHttpUrl(netAddress, route, out string key, requestAction);
            if (!String.IsNullOrEmpty(url))
            {
                try
                {
                    return JsonRequest<R>(url, method, requestAction, responseAction);
                }
                catch (Exception ex)
                {
                    requestEx = ex;
                }
            }

            if (requestEx != null)
                throw requestEx;
            else
                throw new Exception("无可用服务地址");
        }

        private static R JsonRequest<R>(string url, HttpModel method, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                AddWebRequestInfo(request, method.ToString());
                request.ContentLength = 0;

                if (requestAction != null)
                    requestAction(request);

                response = request.GetResponse() as HttpWebResponse;
                if (responseAction != null)
                    responseAction(response);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string json = GetHttpResault(response);
                    if (!String.IsNullOrEmpty(json))
                    {
                        var rt = JsonConvert.DeserializeObject<R>(json);
                        return rt;
                    }
                    else
                    {
                        return default(R);
                    }
                }
                else
                {
                    throw GetResponseError(request, response);
                }

            }
            catch (WebException ex)
            {
                throw GetWebError(ex);
            }
            finally
            {
                if (response != null)
                    response.Close();

                if (request != null)
                    request.Abort();
            }
        }

        /// <summary>
        /// 获取字符串数据
        /// </summary>
        /// <param name="netAddress">请求地址，若以http://或https://开头使用此地址，否则使用Api网关地址</param>
        /// <param name="route">路由地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="requestAction">请求动作，默认为null</param>
        /// <param name="responseAction">返回动作，默认为null</param>
        public static string TextRequest(string netAddress, string route, HttpModel method, Dictionary<string, string> parameters = null, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            Exception requestEx = null;

            string url = GetHttpUrl(netAddress, route, out string key, requestAction);
            if (!String.IsNullOrEmpty(url))
            {
                try
                {
                    return TextRequest(url, method, parameters, requestAction, responseAction);
                }
                catch (Exception ex)
                {
                    requestEx = ex;
                }
            }

            if (requestEx != null)
                throw requestEx;
            else
                throw new Exception("无可用服务地址");
        }


        private static string TextRequest(string url, HttpModel method, Dictionary<string, string> parameters = null, Action<HttpWebRequest> requestAction = null, Action<HttpWebResponse> responseAction = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                AddWebRequestInfo(request, method.ToString());

                if (method == HttpModel.GET)
                    request.ContentType = "text/plain;charset:utf8";
                else
                    request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

                if (requestAction != null)
                    requestAction(request);

                AddWebParameters(request, method, parameters);

                response = request.GetResponse() as HttpWebResponse;
                if (responseAction != null)
                    responseAction(response);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var rh = GetHttpResault(response);
                    return rh;
                }
                else
                {
                    throw GetResponseError(request, response);
                }
            }
            catch (WebException ex)
            {
                throw GetWebError(ex);
            }
            finally
            {
                if (response != null)
                    response.Close();

                if (request != null)
                    request.Abort();
            }
        }

        private static string GetHttpResault(HttpWebResponse response)
        {
            string jsonData = String.Empty;
            using (Stream responseStream = response.GetResponseStream())
            {
                if (!responseStream.CanRead)
                    return "";

                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                jsonData = reader.ReadToEnd();
                responseStream.Close();
            }
            return jsonData;
        }

        private static void AddWebRequestInfo(HttpWebRequest request, string method)
        {
            request.Method = method;
            request.Timeout = 30000;
            request.KeepAlive = true;
            //request.Proxy = null;
            request.ContentType = "application/json;charset=utf-8";
            request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            request.Headers["Accept-Encoding"] = "gzip, deflate";
            request.Headers["Accept-Language"] = "zh-CN";
            request.Headers["Pragma"] = "no-cache";
            request.CookieContainer = new CookieContainer();
        }

        private static void AddWebParameters<T>(HttpWebRequest request, T parameters)
        {
            string param = JsonConvert.SerializeObject((object)parameters);

            if (!String.IsNullOrEmpty(param))
            {
                byte[] parameterBytes = Encoding.UTF8.GetBytes(param);
                request.ContentLength = parameterBytes.Length;
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(parameterBytes, 0, parameterBytes.Length);
                    writer.Close();
                }
            }
        }

        private static void AddWebParameters(HttpWebRequest request, HttpModel method, Dictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                string param = "?";

                if (method == HttpModel.POST)
                    param = String.Empty;

                foreach (var item in parameters)
                {
                    param = String.Format("{0}{1}={2}&", param, item.Key, item.Value);
                }
                if (!String.IsNullOrEmpty(param))
                    param = param.TrimEnd('&');

                byte[] parameterBytes = Encoding.UTF8.GetBytes(param);
                request.ContentLength = parameterBytes.Length;
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(parameterBytes, 0, parameterBytes.Length);
                    writer.Close();
                }
            }
        }

        private static Exception GetWebError(WebException ex)
        {
            if (ex.Response == null)
                return ex;

            using (StreamReader stream = new StreamReader(ex.Response.GetResponseStream()))
            {
                string errMsg = stream.ReadToEnd();
                stream.Close();

                if (!String.IsNullOrEmpty(errMsg))
                    return new Exception(errMsg);
                else
                    return new Exception(ex.Message);
            }
        }

        private static Exception GetResponseError(HttpWebRequest request, HttpWebResponse response)
        {
            var stateCode = response.StatusCode;
            var statusDescription = response.StatusDescription;
            throw new HttpListenerException((int)stateCode, statusDescription);
        }

        private static string GetHttpUrl(string netAddress, string route, out string key, Action<HttpWebRequest> requestAction = null)
        {
            key = route;

            if (netAddress.StartsWith("http://") || netAddress.StartsWith("https://"))
            {
                return $"{netAddress}{route}";
            }
            else
            {


                if (requestAction != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1");
                    requestAction(request);
                    key = request.Headers.Get("X-Resource-Key") ?? route;
                }

                if (netAddressMapper.ContainsKey(key))
                    return netAddressMapper[key];

                return null;
            }
        }
    }

    /// <summary>
    /// 请求方式
    /// </summary>
    public enum HttpModel
    {
        /// <summary>
        /// GET
        /// </summary>
        [Description("GET")]
        GET,

        /// <summary>
        /// POST
        /// </summary>
        [Description("POST")]
        POST,

        /// <summary>
        /// DELETE
        /// </summary>
        [Description("DELETE")]
        DELETE,

        /// <summary>
        /// PUT
        /// </summary>
        [Description("PUT")]
        PUT
    }
}

