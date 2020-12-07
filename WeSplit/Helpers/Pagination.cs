using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Helpers
{
    public class PageInfo
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
    class Pagination : INotifyPropertyChanged
    {
        public int CurrentPage { get; set; }
        public int RowsPerPage { get; set; } = 4;

        private int totalPages;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int TotalPages
        {
            get => totalPages; 
            set
            {
                totalPages = value;
                Pages = new List<PageInfo>();
                for (int i = 1; i <= totalPages; i++)
                {
                    Pages.Add(new PageInfo()
                    {
                        Page = i,
                        TotalPages = totalPages
                    });
                }
                OnPropertyChanged();
            }
        }
        public List<PageInfo> Pages { get; set; }
    }
}
