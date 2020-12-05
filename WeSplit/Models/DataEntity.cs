using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Models
{
    class DataEntity
    {
		private static DataEntity _Entity;
		public static DataEntity Entity
		{
			get
			{
				if (_Entity == null)
					_Entity = new DataEntity();
				return _Entity;
			}
			set
			{
				_Entity = value;
			}
		}
		public WESPLITEntities DB { get; set; }
		private DataEntity()
		{
			DB = new WESPLITEntities();
		}
	}
}
