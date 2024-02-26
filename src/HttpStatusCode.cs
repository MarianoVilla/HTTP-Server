using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    public enum HttpStatusCode
    {
        [Display(Name = "OK")]
        OK = 200,
        [Display(Name = "Not Implemented")]
        NotImplemented = 501,
        [Display(Name = "Not Found")]
        NotFound = 404,
    }
}
