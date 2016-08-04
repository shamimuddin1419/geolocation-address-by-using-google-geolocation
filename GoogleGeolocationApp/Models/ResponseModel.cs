using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleGeolocationApp.Models
{
    public class ResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public object Data { set; get; }
    }
}