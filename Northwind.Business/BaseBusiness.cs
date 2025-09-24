using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Business
{
    public class BaseBusiness
    {
        private Entities _dbContext;
        internal Entities DBContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = new Entities();
                }
                return _dbContext;
            }
        }
        public BaseBusiness() { 
        
        }

    }
}
