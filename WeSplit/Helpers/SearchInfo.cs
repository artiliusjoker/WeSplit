using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Helpers
{
    class SearchInfo
    {
        public bool TripFinishedSearchChecked { get; set; }
        public bool TripOngoingSearchChecked { get; set; }
        public bool MemberSearchChecked { get; set; }
        public string Keyword { get; set; }
        public SearchInfo()
        {
            TripFinishedSearchChecked = true;
            MemberSearchChecked = true;
            TripOngoingSearchChecked = true;
        }
    }
}
