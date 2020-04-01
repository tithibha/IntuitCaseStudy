using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockView.DataAccessLayer;
using StockView.Model;
using StockView.ViewModel;

namespace StockViewTest
{
    public class FakeDispatcherWrapper : IDispatcherWrapper
    {
        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method)
        {            
            method.DynamicInvoke();            
            Thread.Sleep(1000);
            return null;
        }        
    }

    [TestClass]
    public class StockViewTests
    {
        DummyBusinessLogic dummyBusinessLogic;
        StockConfigReader streader = new StockConfigReader();
        StockDataViewModel stockDataViewModel;
        private void Setup()
        {
            dummyBusinessLogic = new DummyBusinessLogic();
            string filename = @"\XMLFile1.xml";
            stockDataViewModel = new StockDataViewModel(dummyBusinessLogic, filename, streader, new FakeDispatcherWrapper());            
        }
        
        [TestMethod]
        public void TestAddCompany()
        {
            Setup();
            stockDataViewModel.CompanyName = "TestCompanyName2";

            stockDataViewModel.AddCompanyToListCommandExecute();
            Thread.Sleep(1000);
            Assert.AreEqual(stockDataViewModel.companyList.Count, 2, "Company name added correctly");
            stockDataViewModel.Dispose();
        }
                
        [TestMethod]
        public void TestRemoveCompany()
        {
            Setup();
            var removeitem = dummyBusinessLogic.GetCompanyQuote("testSymbol2");
            stockDataViewModel.RemoveCommandExecute(removeitem);            
            Assert.AreEqual(stockDataViewModel.CompanyQuotes.Count, 1, "Company name removed correctly");
            stockDataViewModel.Dispose();
        }
    }

    
    public class DummyBusinessLogic : IBusinessLogic
    {
        public CompanyQuote CreateDummyQuote(string sym)
        {
            if (sym == "testSymbol")
            {
                CompanyQuote companyQuote = new CompanyQuote();
                companyQuote.Symbol = "testSymbol";
                companyQuote.name = "TestCompanyName";
                companyQuote.Price = "200";
                return companyQuote;
            }
            else
            {
                CompanyQuote companyQuote1 = new CompanyQuote();
                companyQuote1.Symbol = "testSymbol2";
                companyQuote1.name = "TestCompanyName2";
                companyQuote1.Price = "202";
                return companyQuote1;
            }
        }
        
        public ObservableCollection<Company> GetAllCompanies()
        {
            ObservableCollection<Company> allcomlist = new ObservableCollection<Company>();

            Company company3 = new Company();
            company3.Exchange = "DummyExchange1";
            company3.Name = "TestCompanyName";
            company3.Price = "200";
            company3.Symbol = "testSymbol";

            Company company1 = new Company();
            company1.Exchange = "DummyExchange2";
            company1.Name = "TestCompanyName2";
            company1.Price = "202";
            company1.Symbol = "testSymbol2";

            allcomlist.Add(company1);
            allcomlist.Add(company3);

            return allcomlist;
        }


        public CompanyQuote GetCompanyQuote(string companySymbol)
        {

            if (companySymbol == "testSymbol")
            {
                return CreateDummyQuote("testSymbol");
            }
            else if(companySymbol=="testSymbol2")
            {
                return CreateDummyQuote("testSymbol2");
            }
            else
            { return null; }
        }
    }
}
