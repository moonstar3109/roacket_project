using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace ADMIN.WEB.API.Common
{
    public class NaverLoginApi
    {
        public Dictionary<String, object> NaverLogin(String CT)
        {
            string token = CT;
            string header = "Bearer " + token; // Bearer 다음에 공백 추가
            string apiURL = "https://openapi.naver.com/v1/nid/me";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL);
            request.Headers.Add("X-Naver-Client-Id", "gUldK_M5NOvuzD7tAWtK");
            request.Headers.Add("X-Naver-Client-Secret", "1cyL7aPhg4");
            request.Headers.Add("Authorization", header);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string status = response.StatusCode.ToString();

            //결과
            string text = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (status == "OK")
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                text = reader.ReadToEnd();

                dic["status"] = 200;
                dic["result"] = text;

            }
            else
            {
                dic["status"] = status;
                dic["result"] = "error";

            }

            return dic;
        }
    }
}