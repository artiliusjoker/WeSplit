using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Models
{
    class DatabaseEntity
    {
		private static DatabaseEntity _Entity;
		public static DatabaseEntity Entity
		{
			get
			{
				if (_Entity == null)
					_Entity = new DatabaseEntity();
				return _Entity;
			}
			set
			{
				_Entity = value;
			}
		}
		public WESPLITEntities DB { get; set; }
		private DatabaseEntity()
		{
			DB = new WESPLITEntities();
		}
	}
}
