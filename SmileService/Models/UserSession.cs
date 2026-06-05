using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmileService.Models;

namespace SmileService.Models
{
    public static class UserSession
    {
        public static User CurrentUser { get; set; }
    }
}
