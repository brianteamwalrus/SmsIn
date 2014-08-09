using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SmsIn.Models;

namespace SmsIn
{
    public static class SmsData
    {
        public static void SaveSMS(SmsClass sms)
        {
            using (
                SqlConnection conn =
                    new SqlConnection(
                        System.Configuration.ConfigurationManager.ConnectionStrings["SmsDatabase"].ConnectionString))
            {
                using (
                    SqlCommand Cmd =
                        new SqlCommand(
                            "INSERT INTO SmsMessages (SenderMobile,ReceiverMobile,SmsMessage,Received) VALUES (@dSenderMobile,@dReceiverMobile,@dSmsMessage,@dReceived);",
                            conn))
                {
                    Cmd.Parameters.Add("@dSenderMobile", System.Data.SqlDbType.NVarChar).Value =
                        HttpUtility.HtmlEncode(sms.SenderMobile);
                    Cmd.Parameters.Add("@dReceiverMobile", System.Data.SqlDbType.NVarChar).Value =
                        HttpUtility.HtmlEncode(sms.ReceiverMobile);
                    Cmd.Parameters.Add("@dSmsMessage", System.Data.SqlDbType.NVarChar).Value =
                        HttpUtility.HtmlEncode(sms.Message);
                    Cmd.Parameters.Add("@dReceived", System.Data.SqlDbType.DateTime).Value = sms.Received;
                    conn.Open();
                    Cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static void Purge()
        {
            using (
                SqlConnection conn =
                    new SqlConnection(
                        System.Configuration.ConfigurationManager.ConnectionStrings["SmsDatabase"].ConnectionString))
            {
                using (
                    SqlCommand Cmd =
                        new SqlCommand(
                            "DELETE FROM SmsMessages;",
                            conn))
                {
                    conn.Open();
                    Cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            
        }

        public static SmsGrid GetMessages(int page = 1, int sortBy = 1, bool isAsc = true)
        {
            int pageSize = 20;
            SmsGrid obj = new SmsGrid();
            string sortColumn = string.Empty;

            #region SortingColumn

            switch (sortBy)
            {
                case 1:
                    if (isAsc)
                        sortColumn = "Received";
                    else
                        sortColumn = "Received Desc";
                    break;

                case 2:
                    if (isAsc)
                        sortColumn = "SenderMobile";
                    else
                        sortColumn = "SenderMobile Desc";
                    break;

                case 3:
                    if (isAsc)
                        sortColumn = "ReceiverMobile";
                    else
                        sortColumn = "ReceiverMobile Desc";
                    break;
            }

            #endregion


            using (
                SqlConnection conn =
                    new SqlConnection(
                        System.Configuration.ConfigurationManager.ConnectionStrings["SmsDatabase"].ConnectionString)
                )
            {
                string sqlQuery = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + sortColumn +
                                  ") AS NUM, * FROM SmsMessages) A WHERE NUM > @dLowRecord AND NUM < @dHighRecord;";
                using (SqlCommand Cmd = new SqlCommand(sqlQuery, conn))
                {

                    Cmd.Parameters.Add("@dLowRecord", System.Data.SqlDbType.Int).Value = ((page - 1)*pageSize);
                    Cmd.Parameters.Add("@dHighRecord", System.Data.SqlDbType.Int).Value = ((page*pageSize) + 1);


                    conn.Open();
                    using (SqlDataReader Reader = Cmd.ExecuteReader())
                    {
                        SmsClass sms;
                        while (Reader.Read())
                        {
                            sms = new SmsClass();
                            sms.Received = Convert.ToDateTime(Reader["Received"].ToString());
                            sms.SenderMobile = Reader["SenderMobile"].ToString();
                            sms.ReceiverMobile = Reader["ReceiverMobile"].ToString();
                            sms.Message = Reader["SmsMessage"].ToString();
                            obj.SmsList.Add(sms);
                        }
                    }
                    conn.Close();
                }

                using (
                    SqlCommand Cmd =
                        new SqlCommand("SELECT COUNT(*) AS NumberOfMarkers FROM SmsMessages;", conn))
                {
                    conn.Open();
                    int.TryParse(Cmd.ExecuteScalar().ToString(), out obj.Count);
                    conn.Close();
                }
            }


            obj.CurrentPage = page;
            obj.PageSize = pageSize;
            obj.SortBy = sortBy;
            obj.IsAsc = isAsc;
            obj.TotalPages = Math.Ceiling((double) obj.Count/(double) obj.PageSize);

            if (obj.SmsList.Count() <= pageSize)
                obj.IsLastRecord = 2;

            if (obj.IsLastRecord != 2)
            {
                if (obj.SmsList.Count() <= pageSize)
                    obj.IsLastRecord = 1;
                else
                    obj.IsLastRecord = 0;
            }
            return obj;

        }
    }

}