using System.Collections.Generic;

namespace StockView.DataAccessLayer
{
    public interface IConfigReader
    {
        List<string> ReadFile(string filepath);
        void RemoveFromFile(string filepath, string sym);
        void AddtoFile(string filepath, string sym);
    }
}