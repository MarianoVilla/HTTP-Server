using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    internal static class ExtensionMethods
    {
        public static string? GetDisplayName(this Enum EnumVal)
        {
            return EnumVal?.GetType()?
                            .GetMember(EnumVal.ToString())?
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName();
        }
    }
}
