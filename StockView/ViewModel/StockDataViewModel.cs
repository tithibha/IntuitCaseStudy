using StockView.CommonUtility;
using StockView.DataAccessLayer;
using StockView.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace StockView.ViewModel
{
    public class StockDataViewModel : Utility,IDisposable
    {
        IBusinessLogic bizLogic;
        ICommand addCommand;
        ICommand removeCommand;
        IConfigReader confReader;
        
        public List<string> companyList = new List<string>();
        private ObservableCollection<CompanyQuote> companyQuote = new ObservableCollection<CompanyQuote>();
        private string companyName;
        private IDispatcherWrapper _dispatcherWrapper;
        
        string path = Directory.GetCurrentDirectory() ;
        public StockDataViewModel(IBusinessLogic businessLogic,string fileName=null,IConfigReader confreader=null, IDispatcherWrapper dispatcherWrapper = null)
        {
            
            path = path + fileName;
            _dispatcherWrapper = dispatcherWrapper ?? new DispatcherWrapper(Application.Current.Dispatcher);
            bizLogic = businessLogic;
            AllCompanies = bizLogic.GetAllCompanies();
            confReader = confreader;
            ShowDefinedStocks(confReader);

            InitTimer();
        }

        private void ShowDefinedStocks(IConfigReader confreader)
        {
            
            companyList = confreader.ReadFile(path);
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 3
            };
            Parallel.ForEach(companyList, options, company =>
            {
                var item = bizLogic.GetCompanyQuote(company);
                CheckToExludeDuplicateCompanyStocksInList(item);
            });
        }



        

        #region Commands
        public ICommand AddCompanyToListCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(x => { AddCompanyToListCommandExecute(); }, x => CanAddCompanyToListCommand()));
            }
        }

        private bool CanAddCompanyToListCommand()
        {
            return true;
        }
        
        public void AddCompanyToListCommandExecute()
        {
            if (!string.IsNullOrEmpty(CompanyNameNotNull))
            {
                var sym = GetCompanySymbol(CompanyNameNotNull);
                if (!string.IsNullOrEmpty(sym))
                {
                    var hasitem=companyList.Where(x => x == sym).ToList();

                    if (hasitem.Count() == 0)
                    {
                        companyList.Add(sym);

                        confReader.AddtoFile(path, sym);
                        BackgroundWorker backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AddNewCompany);
                        backgroundWorker.RunWorkerAsync(sym);                        
                    }
                }
                
            }

        }

        public ICommand RemoveCommand
        {
            get
            {
                return removeCommand ?? (removeCommand = new RelayCommand( RemoveCommandExecute, x => CanRemoveCommand()));
            }
        }

        private bool CanRemoveCommand()
        {
            return true;
        }
        
        public void RemoveCommandExecute(object param)
        {
            _dispatcherWrapper.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                lock (locker)
                {
                    var item = param as CompanyQuote;                    
                    companyList.Remove(item.Symbol);
                    confReader.RemoveFromFile(path, item.Symbol);// companyList);
                    var itemtoremove=CompanyQuotes.Where(x => x.Symbol == item.Symbol).ToList();
                    CompanyQuotes.Remove(itemtoremove[0]);
                }
            }));

        }
#endregion

        private string GetCompanySymbol(string companyName)
        {
            try
            {
                var sym = AllCompanies.FirstOrDefault(x => x.Name == companyName).Symbol;
                return sym;
            }
            catch
            {
                return null;
            }
        }

        private void AddNewCompany(object sender, DoWorkEventArgs e)
        {
            string arg = (string)e.Argument;
            var compQuote = bizLogic.GetCompanyQuote(arg);
            CheckToExludeDuplicateCompanyStocksInList(compQuote);
            
        }
        public string CompanyNameNotNull
        {
            get
            { return companyNameNull; }
            set
            {
                if ((companyNameNull != value))
                {
                    companyNameNull = value;
                    OnPropertyChange("CompanyNameNotNull");
                }
            }
        }

        public string CompanyName
        {
            get
            { return companyName; }
            set
            {
                if((companyName != value)) 
                {
                    companyName = value;
                    OnPropertyChange("CompanyName");
                    if (!string.IsNullOrEmpty(companyName))
                        CompanyNameNotNull = companyName;
                }
            }
        }

        ObservableCollection<Company> allCompanies = new ObservableCollection<Company>();
        public ObservableCollection<Company> AllCompanies
        {
            get { return allCompanies; }
            set
            {
                allCompanies = value;
                OnPropertyChange("AllCompanies");
            }
        }

        public ObservableCollection<CompanyQuote> CompanyQuotes
        {
            get { return companyQuote; }
            set
            {
                companyQuote = value;
                OnPropertyChange("CompanyQuotes");
            }
        }

        Timer timer;
        public void InitTimer()
        {
            timer = new Timer(timer_Tick,
               this.GetType().Name,
               TimeSpan.FromSeconds(0),
               TimeSpan.FromSeconds(60));                       
        }

        private void timer_Tick(object state)
        {
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 10
            };
            Parallel.ForEach(companyList, options, company =>
            {
                var item = bizLogic.GetCompanyQuote(company);
                CheckToExludeDuplicateCompanyStocksInList(item);
            });
        }

        
        object locker = new object();
        private string companyNewName=string.Empty;
        private string companyNameNull;

        private void CheckToExludeDuplicateCompanyStocksInList(CompanyQuote item)
        {
            _dispatcherWrapper.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                lock (locker)
                {
                    var ExistingQuote = CompanyQuotes.FirstOrDefault(x => x.Symbol == item.Symbol);
                    if (ExistingQuote == null)
                    {
                        CompanyQuotes.Add(item);
                    }
                    else
                    {
                        CompanyQuotes.Remove(ExistingQuote);
                        CompanyQuotes.Add(item);
                    }
                }
            }));
            
        }

        public void Dispose()
        {
            if (timer != null) ;
            {
                timer.Dispose();
            }
        }
    }

    public interface IDispatcherWrapper
    {
        DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method);
    }
    public class DispatcherWrapper : IDispatcherWrapper
    {
        private Dispatcher _dispatcher;

        public DispatcherWrapper(Dispatcher dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }
            _dispatcher = dispatcher;
        }

        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method)
        {
            return _dispatcher.BeginInvoke(priority, method);
        }

        
    }
}
