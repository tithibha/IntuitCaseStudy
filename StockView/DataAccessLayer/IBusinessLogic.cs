using System.Collections.Generic;
using System.Collections.ObjectModel;
using StockView.Model;

namespace StockView.DataAccessLayer
{
    public interface IBusinessLogic
    {
        ObservableCollection<Company> GetAllCompanies();
        CompanyQuote GetCompanyQuote(string companySymbol);

    }
}