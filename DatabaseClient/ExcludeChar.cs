using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient
{
    public class ExcludeChar : ValidationAttribute
    {
        public string Chars { get; set; }

        public ExcludeChar(string chars)
            : base("{0} contains invalid characters")
        {
            Chars = chars;
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                for (int i = 0; i < Chars.Length; i++)
                {
                    var valueAsString = value.ToString();
                    if (valueAsString.Contains(Chars[i].ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
