using Newtonsoft.Json;
using StockView.CommonUtility;
using StockView.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace StockView.DataAccessLayer
{
    public class BusinessLogic : Utility, IBusinessLogic
    {
        const string connectionString = @"data source = .\SQLEXPRESS; initial catalog = StockViewer; integrated security = True; MultipleActiveResultSets = True; ";

        static HttpClient client = new HttpClient();
        public BusinessLogic()
        {
            client.BaseAddress = new Uri("https://financialmodelingprep.com/");
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));

        }
        object iLock = new object();
        public CompanyQuote GetCompanyQuote(string companySymbol)
        {
            
            string uri = @"api/v3/quote/" + companySymbol;
            if (uri != null)
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var compQuote = response.Content.ReadAsAsync<object>().Result;
                    var obj = JsonConvert.DeserializeObject<List<CompanyQuote>>(compQuote.ToString());
                    if (obj != null)
                    {
                        InsertStockTrendList(obj[0]);
                        return obj[0];
                    }
                }
            }
            return null;
        }
        ObservableCollection<StockTrend> stockTrend = new ObservableCollection<StockTrend>();
        object stocktrendLocker = new object();
        private void InsertStockTrendList(CompanyQuote companyQuote)
        {
            StockTrend newstockTrend = GetStockTrendInstance(companyQuote);
            lock (stocktrendLocker)
            {
                stockTrend.Add(newstockTrend);
                if (stockTrend.Count>1)//save at least 2 entries at a time to minimize the db access.
                {
                    InsertStockTrendToDB(stockTrend);
                    stockTrend.Clear();
                }
            }
        }

        

        private StockTrend GetStockTrendInstance(CompanyQuote companyQuote)
        {
            StockTrend newstockTrend = new StockTrend();
            newstockTrend.StockSymbol = companyQuote.Symbol;
            newstockTrend.StockName = companyQuote.name;
            newstockTrend.Price = companyQuote.Price;
            newstockTrend.Change = companyQuote.Change;
            newstockTrend.YearHigh = companyQuote.YearHigh;
            newstockTrend.YearLow = companyQuote.YearLow;

            return newstockTrend;
        }

        public ObservableCollection<Company> GetAllCompanies()
        {
            ObservableCollection<string> compList = new ObservableCollection<string>();

            string uri = @"api/v3/company/stock/list";
            if (uri != null)
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var companies = response.Content.ReadAsAsync<object>().Result;
                    var obj = JsonConvert.DeserializeObject<symbols>(companies.ToString());
                    if (obj != null)
                    {
                        var res = new ObservableCollection<Company>(obj.symbolsList);
                        InsertCompanyName(res);
                        return res;
                    }
                }
            }
            return null;

        }

       

        #region DBOperation
        private void InsertCompanyName(ObservableCollection<Company> res)
        {
            DataTable tvp = new DataTable();
            tvp.Columns.Add(new DataColumn("Exchange"));
            tvp.Columns.Add(new DataColumn("Symbol"));
            tvp.Columns.Add(new DataColumn("Name"));
            tvp.Columns.Add(new DataColumn("Price"));

            foreach (var item in res)
            {
                tvp.Rows.Add(item.Exchange, item.Symbol,item.Name,item.Price);
            }            
            using (SqlConnection con =new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertAllCompanies", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@data";
                param.Value = tvp;
                cmd.Parameters.Add(param);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }            
        }
        private void InsertStockTrendToDB(ObservableCollection<StockTrend> stockTrend)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("StockSymbol"));
            dt.Columns.Add(new DataColumn("StockName"));
            dt.Columns.Add(new DataColumn("Price"));
            dt.Columns.Add(new DataColumn("Change"));
            dt.Columns.Add(new DataColumn("YearHigh"));
            dt.Columns.Add(new DataColumn("YearLow"));
            foreach (var item in stockTrend)
            {
                dt.Rows.Add(item.StockSymbol, item.StockName, item.Price, item.Change,item.YearHigh,item.YearLow);
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertStockTrend", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@data";
                param.Value = dt;
                cmd.Parameters.Add(param);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion
    }

}
