using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace apitest
{
    public class api
    {
        string SessionID = "";

        public bool HLRLogin(string randomNo)
        {
            bool isTrue = false;
            try
            {
                var xmlFormat = @"<?xml version='1.0' encoding='UTF-8' ?>";
                xmlFormat = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:lgi=""http://www.huawei.com/HLR9820/LGI"">
						<soapenv:Body>
							<lgi:LGI>
								<lgi:OPNAME>username</lgi:OPNAME>
								<lgi:PWD>password</lgi:PWD>
							</lgi:LGI>
						</soapenv:Body>
					</soapenv:Envelope>";
                var xml = string.Format(xmlFormat);
                var url = "http://localhost:8001/";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var data = UTF8Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;
                request.Headers.Add("SOAPAction: 'Notification");
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.ContentLength = data.Length;
                request.Method = "POST";
                request.GetRequestStream().Write(data, 0, data.Length);
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var resText = reader.ReadToEnd();
                if (resText.Contains("Operation is successful"))
                {
                    var responseUrl = response.ResponseUri;
                    SessionID = responseUrl.ToString();
                    int UrlLength = url.Length;
                    int StartPosition = SessionID.IndexOf(url) + UrlLength;
                    SessionID = SessionID.Substring(StartPosition);
                    isTrue = true;
                }
                else
                {
                }


            }
            catch (Exception ex)
            {
                return isTrue;
            }
            return isTrue;
        }
        public string HLRLogout(string randomNo)
        {
            string responseStr = "";
            try
            {
                var xmlFormat = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:lgo=""http://www.huawei.com/HLR9820/LGO"">
            <soapenv:Header/>
                <soapenv:Body>
                    <lgo:LGO>
                   </lgo:LGO>
               </soapenv:Body>
           </soapenv:Envelope>";
                var xml = string.Format(xmlFormat);
                var url = "http://localhost:8001/" + SessionID;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = false;
                var data = UTF8Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;
                request.Headers.Add("SOAPAction: 'Notification");
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";
               
                string address = request.Address.ToString();
                request.GetRequestStream().Write(data, 0, data.Length);



                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var resText = reader.ReadToEnd();
                responseStr = resText;
                responseStr = address + "\n" + resText;
                if (resText.Contains("Operation is successful"))
                {

                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                return responseStr;
            }
            return responseStr;
        }
        public string HLRLocation(string msisdn, string randomNo)
        {
            string vlrLocation = "";
            string AreaLocation = "";
            string CellLocationSL = "";
            string CellLocationRoaming = "";
            string combined = ";";

            try
            {
                var xmlFormat = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:get=""http://www.huawei.com/HLR9820/GET_TERMSTATE"">
            <soapenv:Header/>
               <soapenv:Body>
               <get:GET_TERMSTATE>
               <get:ISDN>{0}</get:ISDN>
               <get:STATUS>TRUE</get:STATUS>
               <get:OnHLR>0</get:OnHLR>
               <get:DetailFLAG>TRUE</get:DetailFLAG>
               </get:GET_TERMSTATE>
               </soapenv:Body>
                </soapenv:Envelope>";
                var xml = string.Format(xmlFormat, msisdn);
                var url = "http://localhost:8001/" + SessionID;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var data = UTF8Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;
                request.Headers.Add("SOAPAction: 'Notification");
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";
                request.GetRequestStream().Write(data, 0, data.Length);
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var resText = reader.ReadToEnd();

                if (resText.Contains("Operation is successful"))
                {

                    int StartPosition = resText.IndexOf("<VLRNumber>") + 11;
                    int EndPosition = resText.IndexOf("</VLRNumber>");
                    int StartPosition2 = resText.IndexOf("<LocationAreaCode>") + 18;
                    int EndPosition2 = resText.IndexOf("</LocationAreaCode>");
                    int StartPosition3 = resText.IndexOf("<CellIdentity>") + 14;
                    int EndPosition3 = resText.IndexOf("</CellIdentity>");
                    int StartPosition4 = resText.IndexOf("<ServiceAreaCode>") + 17;
                    int EndPosition4 = resText.IndexOf("</ServiceAreaCode>");


                    vlrLocation = resText.Substring(StartPosition, EndPosition - StartPosition);
                    AreaLocation = resText.Substring(StartPosition2, EndPosition2 - StartPosition2);
                    if (resText.Contains("<CellIdentity>"))
                    {
                        CellLocationSL = resText.Substring(StartPosition3, EndPosition3 - StartPosition3);
                    }
                    if (resText.Contains("<ServiceAreaCode>"))
                    {
                        CellLocationRoaming = resText.Substring(StartPosition4, EndPosition4 - StartPosition4);
                    }

                    if (resText.Contains("<LocationAreaCode>"))
                    {
                        combined = vlrLocation + ";" + "63771-" + AreaLocation + "-" + CellLocationSL;
                    }
                    if (resText.Contains("<ServiceAreaCode>"))
                    {
                        combined = vlrLocation + ";" + "63771-" + AreaLocation + "-" + CellLocationRoaming;
                    }



                }

                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            return combined;
        }
    }
}
