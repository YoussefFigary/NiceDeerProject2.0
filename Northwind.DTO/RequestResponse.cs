using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.DTO
{
    public class RequestResponse
    {
        public bool Success { get; set; }
        public string HttpStatus { get; set; }
        public object JsonData { get; set; }
        public string Message { get; set; }
    }
}
