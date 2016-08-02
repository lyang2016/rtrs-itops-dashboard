using System;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Oracle.ManagedDataAccess.Client;
using RTRSCommon.Model;

namespace RTRSOpDashboard.WebService.Tests
{
    public class TestHelper
    {
        public static string ConnectionString { get; set; }

        public static void RunQuery(string query)
        {
            using (var conn = new OracleConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public static void RunQuery(string query, params object[] queryValues)
        {
            RunQuery(string.Format(query, queryValues));
        }

        public static DataTable GetData(string query)
        {
            var data = new DataTable();

            using (var conn = new OracleConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
                conn.Close();
            }
            return data;
        }

        public static DataTable GetData(string query, params object[] queryValues)
        {
            return GetData(string.Format(query, queryValues));
        }

        public static string FormatOracleDateTime(DateTime dt)
        {
            return dt.ToString("dd-MMM-yyyy hh':'mm':'ss tt").ToUpper();
        }

        public static string FormatOracleDateTimeWithMS(DateTime dt)
        {
            return dt.ToString("dd-MMM-yyyy hh':'mm':'ss'.'fff tt").ToUpper();
        }

        public static string FormatOracleDateOnly(DateTime dt)
        {
            return dt.ToString("dd-MMM-yyyy").ToUpper();
        }

        public static void AssertProperties(Object expected, DataRow row)
        {
            var type = expected.GetType();
            foreach (var info in type.GetProperties())
            {
                var exp = info.GetValue(expected, null);
                var att = info.GetCustomAttributes(typeof(DBFieldName), false);

                if (att.Count() == 1)
                {
                    var act = row[((DBFieldName)att[0]).Field];
                    if (info.PropertyType.IsValueType || typeof(string) == info.PropertyType)
                    {
                        if (typeof(DateTime) == info.PropertyType)
                        {
                            Assert.AreEqual(((DateTime)exp).ToString("yyyy-MM-dd HH:mm:ss"),
                                ((DateTime)act).ToString("yyyy-MM-dd HH:mm:ss"),
                                string.Format("Expected {0} but got {1} for property {2}", exp, act, info.Name));
                        }
                        else if (typeof(bool) == info.PropertyType)
                        {
                            var sExp = ((bool)exp) ? "Y" : "N";
                            Assert.AreEqual(sExp, act, string.Format("Expected {0} but got {1} for property {2}", sExp, act, info.Name));
                        }
                        else
                        {
                            Assert.AreEqual(exp, act,
                                string.Format("Expected {0} but got {1} for property {2}", exp, act, info.Name));
                        }
                    }
                }
            }
        }
    }
}