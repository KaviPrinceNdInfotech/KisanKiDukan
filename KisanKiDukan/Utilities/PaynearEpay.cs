using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KisanKiDukan.Utilities
{
    /* this class is used to integrate merchant portal to Paynear Epay. */
    public class PaynearEpay
    {
        //Getting "merchantId and secretKey" from "Web.config"
        public string merchantId = ConfigurationManager.AppSettings["merchantId"];
        public string secretKey = ConfigurationManager.AppSettings["secretKey"];
        //UAT Url will be given by paynear
        public string paynearEpayURLTest = "http://mpos.sandbox.paynear.in";
        public string paynearEpayURLLive = "https://secure.paynear.in";
        public string paymentEndpoint = "http://mpos.sandbox.paynear.in:8080/epay/payment/request";
        public string verifyEndpoint = "http://mpos.sandbox.paynear.in:8080/epay/payment/verify";
        public string statusEndpoint = "http://mpos.sandbox.paynear.in:8080/epay/payment/status";
        public string refundEndpoint = "http://mpos.sandbox.paynear.in:8080/epay/payment/refund";


        public List<KeyValuePair<string, string>> initiatePayment(List<KeyValuePair<string, string>> inparams)
        {

            inparams.Remove(inparams.Find(x => x.Key == "__EVENTVALIDATION"));
            inparams.Remove(inparams.Find(x => x.Key == "__VIEWSTATE"));
            inparams.Remove(inparams.Find(x => x.Key == "__VIEWSTATEGENERATOR"));
            //Adding "merchantId" given by the paynear
            inparams.Add(new KeyValuePair<string, string>("merchantId", merchantId));
            //Generating and adding secure hash
            inparams.Add(new KeyValuePair<string, string>("secureHash", getChecksum(inparams, secretKey)));

            StringBuilder data = new StringBuilder();

            //Converting Map to JSON format
            data.Append("{");
            foreach (KeyValuePair<string, string> kvPair in inparams)
            {
                //long lgValue = 0; int intValue = 0;
                ////bool dblValue;
                ////if (bool.TryParse(kvPair.Value, out dblValue))
                //if (long.TryParse(kvPair.Value, out lgValue) || int.TryParse(kvPair.Value, out intValue))
                //    data.Append("\"" + kvPair.Key + "\"" + ":" + kvPair.Value + ",");
                //else
                    data.Append("\"" + kvPair.Key + "\"" + ":" + "\"" + kvPair.Value + "\"" + ",");
            }
            //Removing one "," from JSON format
            data.Remove(data.Length - 1, 1);
            data.Append("}");
            byte[] byteData = getbyteArray(data.ToString());

            //Sending PaymentRequest UAT Url given by paynear
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(paymentEndpoint);
            wreq.Credentials = CredentialCache.DefaultCredentials;
            wreq.Method = "POST";
            wreq.ContentType = "application/x-www-form-urlencoded";
            wreq.ContentLength = byteData.Length;

            using (Stream postStream = wreq.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            HttpWebResponse resp = null;
            try
            {
                //Getting Response from Paynear
                resp = wreq.GetResponse() as HttpWebResponse;

                Stream respStream = resp.GetResponseStream();
                MemoryStream respMemStr = new MemoryStream();
                int count = 0;
                do
                {
                    byte[] buf = new byte[2048];
                    count = respStream.Read(buf, 0, 2048);
                    respMemStr.Write(buf, 0, count);

                }
                while (respStream.CanRead && count > 0);
                respStream.Equals(respMemStr);
                respMemStr.Position = 0;

                //After getting Response from paynear Adding data in to RequestResponseVO
                RequestResponseVO reqResp = new RequestResponseVO();

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RequestResponseVO));
                reqResp = (RequestResponseVO)ser.ReadObject(respMemStr);
                respStream.Close();

                //After getting Response from paynear Generating and adding data to ResponseMap
                List<KeyValuePair<string, string>> responseMapp = new List<KeyValuePair<string, string>>();
                responseMapp.Add(new KeyValuePair<string, string>("amount", reqResp.getAmount()));
                responseMapp.Add(new KeyValuePair<string, string>("redirectURL", reqResp.getRedirectURL()));
                responseMapp.Add(new KeyValuePair<string, string>("referenceNo", reqResp.getReferenceNo()));
                responseMapp.Add(new KeyValuePair<string, string>("responseCode", reqResp.getResponseCode()));
                responseMapp.Add(new KeyValuePair<string, string>("responseMessage", reqResp.getResponseMessage()));
                //Generating new secure hash
                string generatedSecureHash = getChecksum(responseMapp, secretKey);
                //needs to compare/validate both secureHash (from response , calculated secureHash at merchant end)
                if (generatedSecureHash != null && reqResp.getSecureHash() != null && generatedSecureHash.Equals(reqResp.getSecureHash()))
                    if (reqResp != null && reqResp.getResponseMessage() != null && "Success".Equals(reqResp.getResponseMessage()) &&
                                     "000".Equals(reqResp.getResponseCode()))
                        if (reqResp.getAmount() == inparams.Find(k => k.Key == "amount").Value)
                            if (reqResp.getReferenceNo() == inparams.Find(k => k.Key == "referenceNo").Value)
                                //Success
                                //Returning ResponseMap to Default.cs page
                                return responseMapp;
                            else
                                //throw error response details correpted
                                //Returning ResponseMap to Default.cs page
                                return responseMapp;
                        else
                            //throw error response details correpted
                            //Returning ResponseMap to Default.cs page
                            return responseMapp;
                    else
                        //throw error response details correpted
                        //Returning ResponseMap to Default.cs page
                        return responseMapp;
                else
                    //throw error response details correpted
                    //Returning ResponseMap to Default.cs page
                    return responseMapp;
            }
            catch (WebException wex)
            {
                resp = (HttpWebResponse)wex.Response;
            }
            return null;
        }

        public List<KeyValuePair<string, string>> getPaymentResponse(List<KeyValuePair<string, string>> inparams)
        {
            //After getting response from payment page chacking incoming Response Null or Not-Null
            if (inparams != null)
            {
                //Storing "secureHash" in to string
                string hashFromServer = inparams.Find(x => x.Key == "secureHash").Value;
                //Removing "secureHash" from Response
                inparams.Remove(inparams.Find(x => x.Key == "secureHash"));
                //Generating  new secure hash
                String generatedhash = getChecksum(inparams, secretKey);
                //needs to compare/validate both secureHash (from response , calculated secureHash at merchant end)
                if (generatedhash != null && hashFromServer != null && generatedhash.Equals(hashFromServer))
                {
                    //Generating and adding data to statuMap
                    List<KeyValuePair<string, string>> statusMap = new List<KeyValuePair<string, string>>();
                    statusMap.Add(new KeyValuePair<string, string>("orderRefNo", inparams.Find(x => x.Key == "orderRefNo").Value));
                    statusMap.Add(new KeyValuePair<string, string>("paymentId", inparams.Find(x => x.Key == "paymentId").Value));
                    statusMap.Add(new KeyValuePair<string, string>("transactionId", inparams.Find(x => x.Key == "transactionId").Value));
                    statusMap.Add(new KeyValuePair<string, string>("amount", inparams.Find(x => x.Key == "amount").Value));
                    statusMap.Add(new KeyValuePair<string, string>("merchantId", inparams.Find(x => x.Key == "merchantId").Value));
                    statusMap.Add(new KeyValuePair<string, string>("secureHash", getChecksum(statusMap, secretKey)));

                    StringBuilder data = new StringBuilder();
                    //Converting Map to JSON format
                    data.Append("{");
                    foreach (KeyValuePair<string, string> kvPair in statusMap)
                    {
                        //double dblValue = 0; long lgValue = 0; int intValue = 0;

                        //if (double.TryParse(kvPair.Value, out dblValue) || long.TryParse(kvPair.Value, out lgValue) || int.TryParse(kvPair.Value, out intValue))
                        //bool dblValue;
                        //if (bool.TryParse(kvPair.Value, out dblValue))
                        //    data.Append("\"" + kvPair.Key + "\"" + ":" + kvPair.Value + ",");
                        //else
                            data.Append("\"" + kvPair.Key + "\"" + ":" + "\"" + kvPair.Value + "\"" + ",");
                    }
                    //Removing one "," from JSON format
                    data.Remove(data.Length - 1, 1);
                    data.Append("}");

                    byte[] byteData = getbyteArray(data.ToString());

                    //Sending StatusRequest UAT Url given by paynear for verify PaymentResponse
                    HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(statusEndpoint);
                    wreq.Credentials = CredentialCache.DefaultCredentials;
                    wreq.Method = "POST";
                    wreq.ContentType = "application/x-www-form-urlencoded";
                    wreq.ContentLength = byteData.Length;

                    using (Stream postStream = wreq.GetRequestStream())
                    {
                        postStream.Write(byteData, 0, byteData.Length);
                    }

                    HttpWebResponse resp = null;
                    try
                    {
                        //Getting PaymentResponse from Paynear
                        resp = wreq.GetResponse() as HttpWebResponse;

                        Stream respStream = resp.GetResponseStream();

                        MemoryStream respMemStr = new MemoryStream();
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[2048];
                            count = respStream.Read(buf, 0, 2048);
                            respMemStr.Write(buf, 0, count);

                        }
                        while (respStream.CanRead && count > 0);
                        respStream.Equals(respMemStr);
                        respMemStr.Position = 0;
                        //After getting Response from paynear Adding data in to RequestResponseVO
                        RequestResponseVO reqResp = new RequestResponseVO();

                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RequestResponseVO));
                        reqResp = (RequestResponseVO)ser.ReadObject(respMemStr);
                        respStream.Close();
                        //After getting Response from paynear Generating and adding data to StatusMap
                        List<KeyValuePair<string, string>> statusResponseMap = new List<KeyValuePair<string, string>>();
                        statusResponseMap.Add(new KeyValuePair<string, string>("orderRefNo", reqResp.getOrderRefNo()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("amount", reqResp.getAmount()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("paymentId", reqResp.getPaymentId()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("merchantId", reqResp.getMerchantId() + ""));
                        statusResponseMap.Add(new KeyValuePair<string, string>("transactionId", reqResp.getTransactionId()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("responseMessage", reqResp.getResponseMessage()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("responseCode", reqResp.getResponseCode()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("transactionType", reqResp.getTransactionType()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("transactionDate", reqResp.getTransactionDate()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("currencyCode", reqResp.getCurrencyCode()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("paymentMethod", reqResp.getPaymentMethod()));
                        statusResponseMap.Add(new KeyValuePair<string, string>("paymentBrand", reqResp.getPaymentBrand()));
                        //Generating new secure hash
                        string statusSecureHash = getChecksum(statusResponseMap, secretKey);
                        //needs to compare/validate both secureHash (from response , calculated secureHash at merchant end)
                        if (statusSecureHash != null && reqResp.getSecureHash() != null && statusSecureHash.Equals(reqResp.getSecureHash()))
                            if (reqResp != null && reqResp.getResponseMessage() != null && "Success".Equals(reqResp.getResponseMessage()) &&
                                    "000".Equals(reqResp.getResponseCode()))
                                if (reqResp.getAmount() == inparams.Find(k => k.Key == "amount").Value)
                                    if (reqResp.getOrderRefNo() == inparams.Find(k => k.Key == "orderRefNo").Value)
                                        //Success
                                        //Returning StatusResponseMap to Response.cs page
                                        return statusResponseMap;
                                    else
                                        //throw error response details correpted
                                        //Returning StatusResponseMap to Response.cs page
                                        return statusResponseMap;
                                else
                                    //throw error response details correpted
                                    //Returning StatusResponseMap to Response.cs page
                                    return statusResponseMap;
                            else
                                //throw error response details correpted
                                //Returning StatusResponseMap to Response.cs page
                                return statusResponseMap;
                        else
                            //throw error response details correpted
                            //Returning StatusResponseMap to Response.cs page
                            return statusResponseMap;
                    }
                    catch (WebException wex)
                    {
                        resp = (HttpWebResponse)wex.Response;
                    }
                }
            }
            return null;
        }

        public List<KeyValuePair<string, string>> initiateVerify(List<KeyValuePair<string, string>> inparams)
        {


            inparams.Remove(inparams.Find(x => x.Key == "__EVENTVALIDATION"));
            inparams.Remove(inparams.Find(x => x.Key == "__VIEWSTATE"));
            inparams.Remove(inparams.Find(x => x.Key == "__VIEWSTATEGENERATOR"));
            //Adding "merchantId" given by the paynear
            inparams.Add(new KeyValuePair<string, string>("merchantId", merchantId));
            //Generating and adding secure hash
            inparams.Add(new KeyValuePair<string, string>("secureHash", getChecksum(inparams, secretKey)));

            StringBuilder data = new StringBuilder();
            //Converting Map to JSON format
            data.Append("{");
            foreach (KeyValuePair<string, string> kvPair in inparams)
            {
                //double dblValue = 0; long lgValue = 0; int intValue = 0;

                //if (double.TryParse(kvPair.Value, out dblValue) || long.TryParse(kvPair.Value, out lgValue) || int.TryParse(kvPair.Value, out intValue))
                //bool dblValue;
                //if (bool.TryParse(kvPair.Value, out dblValue))
                //    data.Append("\"" + kvPair.Key + "\"" + ":" + kvPair.Value + ",");
                //else
                    data.Append("\"" + kvPair.Key + "\"" + ":" + "\"" + kvPair.Value + "\"" + ",");
            }
            //Removing one "," from JSON format
            data.Remove(data.Length - 1, 1);
            data.Append("}");

            byte[] byteData = getbyteArray(data.ToString());
            //Sending VerifyRequest UAT Url given by paynear
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(verifyEndpoint);
            wreq.Credentials = CredentialCache.DefaultCredentials;
            wreq.Method = "POST";
            wreq.ContentType = "application/x-www-form-urlencoded";
            wreq.ContentLength = byteData.Length;

            using (Stream postStream = wreq.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            HttpWebResponse resp = null;
            try
            {
                //Getting Response from Paynear
                resp = wreq.GetResponse() as HttpWebResponse;

                Stream respStream = resp.GetResponseStream();
                //After getting Response from paynear Adding data in to RequestResponseVO
                RequestResponseVO reqResp = new RequestResponseVO();

                MemoryStream respMemStr = new MemoryStream();
                int count = 0;
                do
                {
                    byte[] buf = new byte[2048];
                    count = respStream.Read(buf, 0, 2048);
                    respMemStr.Write(buf, 0, count);

                }
                while (respStream.CanRead && count > 0);
                respStream.Equals(respMemStr);
                respMemStr.Position = 0;

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RequestResponseVO));
                reqResp = (RequestResponseVO)ser.ReadObject(respMemStr);
                respStream.Close();
                //After getting Response from paynear Generating and adding data to VerifystatusMap
                List<KeyValuePair<string, string>> varifystatusMap = new List<KeyValuePair<string, string>>();
                varifystatusMap.Add(new KeyValuePair<string, string>("orderRefNo", reqResp.getOrderRefNo()));
                varifystatusMap.Add(new KeyValuePair<string, string>("amount", reqResp.getAmount()));
                varifystatusMap.Add(new KeyValuePair<string, string>("paymentId", reqResp.getPaymentId()));
                varifystatusMap.Add(new KeyValuePair<string, string>("merchantId", reqResp.getMerchantId()+""));
                varifystatusMap.Add(new KeyValuePair<string, string>("transactionId", reqResp.getTransactionId()));
                varifystatusMap.Add(new KeyValuePair<string, string>("responseMessage", reqResp.getResponseMessage()));
                varifystatusMap.Add(new KeyValuePair<string, string>("responseCode", reqResp.getResponseCode()));
                varifystatusMap.Add(new KeyValuePair<string, string>("transactionType", reqResp.getTransactionType()));
                varifystatusMap.Add(new KeyValuePair<string, string>("transactionDate", reqResp.getTransactionDate()));
                varifystatusMap.Add(new KeyValuePair<string, string>("currencyCode", reqResp.getCurrencyCode()));
                varifystatusMap.Add(new KeyValuePair<string, string>("paymentMethod", reqResp.getPaymentMethod()));
                varifystatusMap.Add(new KeyValuePair<string, string>("paymentBrand", reqResp.getPaymentBrand()));
                //Generating new secure hash
                String statusSecureHash = getChecksum(varifystatusMap, secretKey);
                //needs to compare/validate both secureHash (from response , calculated secureHash at merchant end)
                if (statusSecureHash != null && reqResp.getSecureHash() != null && statusSecureHash.Equals(reqResp.getSecureHash()))
                    if (reqResp != null && reqResp.getResponseMessage() != null && "Success".Equals(reqResp.getResponseMessage()) &&
                          "000".Equals(reqResp.getResponseCode()))
                        if (reqResp.getAmount() == inparams.Find(k => k.Key == "amount").Value)
                            if (reqResp.getReferenceNo() == inparams.Find(k => k.Key == "referenceNo").Value)
                                //Success
                                //Returning VerifystatusMap to Verify.cs page
                                return varifystatusMap;
                            else
                                //throw error response details correpted
                                //Returning VerifystatusMap to Verify.cs page
                                return varifystatusMap;
                        else
                            //throw error response details correpted
                            //Returning VerifystatusMap to Verify.cs page
                            return varifystatusMap;
                    else
                        //throw error response details correpted
                        //Returning VerifystatusMap to Verify.cs page
                        return varifystatusMap;
                else
                    //throw error response details correpted
                    //Returning VerifystatusMap to Verify.cs page
                    return varifystatusMap;
            }
            catch (WebException wex)
            {
                resp = (HttpWebResponse)wex.Response;
            }
            return null;
        }
        public List<KeyValuePair<string, string>> initiateRefund(List<KeyValuePair<string, string>> inparams)
        {


            inparams.Remove(inparams.Find(x => x.Key == "__EVENTVALIDATION"));
            inparams.Remove(inparams.Find(x => x.Key == "__VIEWSTATE"));
            inparams.Remove(inparams.Find(x => x.Key == "__VIEWSTATEGENERATOR"));
            //Adding "merchantId" given by the paynear
            inparams.Add(new KeyValuePair<string, string>("merchantId", merchantId));
            //Generating and adding secure hash
            inparams.Add(new KeyValuePair<string, string>("secureHash", getChecksum(inparams, secretKey)));

            StringBuilder data = new StringBuilder();
            //Converting Map to JSON format
            data.Append("{");
            foreach (KeyValuePair<string, string> kvPair in inparams)
            {
                //double dblValue = 0; long lgValue = 0; int intValue = 0;

                //if (double.TryParse(kvPair.Value, out dblValue) || long.TryParse(kvPair.Value, out lgValue) || int.TryParse(kvPair.Value, out intValue))
                //bool dblValue;
                //if (bool.TryParse(kvPair.Value, out dblValue))
                //    data.Append("\"" + kvPair.Key + "\"" + ":" + kvPair.Value + ",");
                //else
                    data.Append("\"" + kvPair.Key + "\"" + ":" + "\"" + kvPair.Value + "\"" + ",");
            }
            //Removing one "," from JSON format
            data.Remove(data.Length - 1, 1);
            data.Append("}");

            byte[] byteData = getbyteArray(data.ToString());
            //Sending RefundRequest UAT Url given by paynear
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(refundEndpoint);
            wreq.Credentials = CredentialCache.DefaultCredentials;
            wreq.Method = "POST";
            wreq.ContentType = "application/x-www-form-urlencoded";
            wreq.ContentLength = byteData.Length;

            using (Stream postStream = wreq.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            HttpWebResponse resp = null;
            try
            {
                //Getting Response from Paynear
                resp = wreq.GetResponse() as HttpWebResponse;

                Stream respStream = resp.GetResponseStream();
                //After getting Response from paynear Adding data in to RequestResponseVO
                RequestResponseVO reqResp = new RequestResponseVO();

                MemoryStream respMemStr = new MemoryStream();
                int count = 0;
                do
                {
                    byte[] buf = new byte[2048];
                    count = respStream.Read(buf, 0, 2048);
                    respMemStr.Write(buf, 0, count);

                }
                while (respStream.CanRead && count > 0);
                respStream.Equals(respMemStr);
                respMemStr.Position = 0;

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RequestResponseVO));
                reqResp = (RequestResponseVO)ser.ReadObject(respMemStr);
                respStream.Close();
                //After getting Response from paynear Generating and adding data to RefundstatusMap
                List<KeyValuePair<string, string>> refundstatusMap = new List<KeyValuePair<string, string>>();
                refundstatusMap.Add(new KeyValuePair<string, string>("orderRefNo", reqResp.getOrderRefNo()));
                refundstatusMap.Add(new KeyValuePair<string, string>("amount", reqResp.getAmount()));
                refundstatusMap.Add(new KeyValuePair<string, string>("paymentId", reqResp.getPaymentId()));
                refundstatusMap.Add(new KeyValuePair<string, string>("merchantId", reqResp.getMerchantId()+""));
                refundstatusMap.Add(new KeyValuePair<string, string>("transactionId", reqResp.getTransactionId()));
                refundstatusMap.Add(new KeyValuePair<string, string>("responseMessage", reqResp.getResponseMessage()));
                refundstatusMap.Add(new KeyValuePair<string, string>("responseCode", reqResp.getResponseCode()));
                refundstatusMap.Add(new KeyValuePair<string, string>("transactionType", reqResp.getTransactionType()));
                refundstatusMap.Add(new KeyValuePair<string, string>("transactionDate", reqResp.getTransactionDate()));
                refundstatusMap.Add(new KeyValuePair<string, string>("currencyCode", reqResp.getCurrencyCode()));
                //Generating new secure hash
                String statusSecureHash = getChecksum(refundstatusMap, secretKey);
                //needs to compare/validate both secureHash (from response , calculated secureHash at merchant end)
                if (statusSecureHash != null && reqResp.getSecureHash() != null && statusSecureHash.Equals(reqResp.getSecureHash()))
                    if ("303".Equals(reqResp.getResponseCode()))
                        if (reqResp.getAmount() == inparams.Find(k => k.Key == "amount").Value)
                            if (reqResp.getReferenceNo() == inparams.Find(k => k.Key == "referenceNo").Value)
                                //Success
                                //Returning RefundstatusMap to Refund.cs page
                                return refundstatusMap;
                            else
                                //throw error response details correpted
                                //Returning RefundstatusMap to Refund.cs page
                                return refundstatusMap;
                        else
                            //throw error response details correpted
                            //Returning RefundstatusMap to Refund.cs page
                            return refundstatusMap;
                    else
                        //throw error response details correpted
                        //Returning RefundstatusMap to Refund.cs page
                        return refundstatusMap;
                else
                    //throw error response details correpted
                    //Returning RefundstatusMap to Refund.cs page
                    return refundstatusMap;
            }
            catch (WebException wex)
            {
                resp = (HttpWebResponse)wex.Response;
            }
            return null;
        }
        //Converting JSON to byte[] format
        public static byte[] getbyteArray(string s)
        {
            byte[] byteArray = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            { byteArray[i] = Convert.ToByte(s[i]); }

            return byteArray;
        }
        //Converting dataMap and secretKey in to secure hash
        public string getChecksum(List<KeyValuePair<string, string>> dataMap, string secretKey)
        {
            StringBuilder builder = new StringBuilder();
            //Add secret key in starting position
            builder.Append(secretKey);
            //Sorting Map Data in to ascending order
            dataMap.Sort((x, y) => String.Compare(x.Key, y.Key));
            //Append all parameters values into single separated by pipe ‘|’
            for (int i = 0; i < dataMap.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataMap[i].Value))
                    builder.Append("|" + dataMap[i].Value);
                else
                    builder.Append("|");
            }
            //Converting JSON in to  SHA-512
            HashAlgorithm digest = new SHA512CryptoServiceProvider();

            byte[] hash = digest.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));

            StringBuilder hexString = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                string hex = (0xff & hash[i]).ToString("x2");

                if (hex.Length == 1) hexString.Append('0');

                hexString.Append(hex);
            }
            //Converting lower case hexString to uppercase
            return hexString.ToString().ToUpper();

        }
    }

    //This class is used to sorting response data to RequestResponseVO
    [DataContract]
    public class RequestResponseVO
    {
        [DataMember]
        public string referenceNo;

        [DataMember]
        public string responseCode;

        [DataMember]
        public string responseMessage;

        [DataMember]
        public string amount;

        [DataMember]
        public string redirectURL;

        [DataMember]
        public string secureHash;

        [DataMember]
        public string orderRefNo;

        [DataMember]
        public string paymentId;

        [DataMember]
        public long merchantId;

        [DataMember]
        public string transactionId;

        [DataMember]
        public string transactionType;

        [DataMember]
        public string transactionDate;

        [DataMember]
        public string currencyCode;

        [DataMember]
        public string paymentMethod;

        [DataMember]
        public string paymentBrand;

        public String getPaymentMethod()
        {
            return paymentMethod;
        }
        public void setPaymentMethod(string paymentMethod)
        {
            this.paymentMethod = paymentMethod;
        }

        public String getPaymentBrand()
        {
            return paymentBrand;
        }
        public void setPaymentBrand(string paymentBrand)
        {
            this.paymentBrand = paymentBrand;
        }

        public String getSecureHash()
        {
            return secureHash;
        }
        public void setSecureHash(string secureHash)
        {
            this.secureHash = secureHash;
        }
        public String getReferenceNo()
        {
            return referenceNo;
        }

        public void setReferenceNo(String referenceNo)
        {
            this.referenceNo = referenceNo;
        }

        public String getResponseCode()
        {
            return responseCode;
        }

        public void setResponseCode(String responseCode)
        {
            this.responseCode = responseCode;
        }

        public String getResponseMessage()
        {
            return responseMessage;
        }

        public void setResponseMessage(String responseMessage)
        {
            this.responseMessage = responseMessage;
        }

        public string getAmount()
        {
            return amount;
        }

        public void setAmount(string amount)
        {
            this.amount = amount;
        }

        public String getRedirectURL()
        {
            return redirectURL;
        }

        public void setRedirectURL(String redirectURL)
        {
            this.redirectURL = redirectURL;
        }

        public string getOrderRefNo()
        {
            return orderRefNo;
        }

        public void setOrderRefNo(String orderRefNo)
        {
            this.orderRefNo = orderRefNo;
        }

        public string getPaymentId()
        {
            return paymentId;
        }

        public void setPaymentId(String paymentId)
        {
            this.paymentId = paymentId;
        }

        public long getMerchantId()
        {
            return merchantId;
        }

        public void seMerchantId(long merchantId)
        {
            this.merchantId = merchantId;
        }

        public string getTransactionId()
        {
            return transactionId;
        }

        public void setTransactionId(String transactionId)
        {
            this.transactionId = transactionId;
        }

        public string getTransactionType()
        {
            return transactionType;
        }

        public void setTransactionType(String transactionType)
        {
            this.transactionType = transactionType;
        }

        public string getTransactionDate()
        {
            return transactionDate;
        }

        public void setTransactionDate(String transactionDate)
        {
            this.transactionDate = transactionDate;
        }

        public string getCurrencyCode()
        {
            return currencyCode;
        }

        public void setCurrencyCode(String currencyCode)
        {
            this.currencyCode = currencyCode;
        }
    }
}