using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFINITE.CORE.Data.CodeGenerator.Generator
{
    public partial class CodeTemplate
    {
        public static string RemoveText(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Remove(Pos1, Pos2 - Pos1);
            FinalString = FinalString.Replace(FirstString, "");
            FinalString = FinalString.Replace(LastString, "");
            return FinalString;
        }
        public static string RemovePrefix(string text)
        {
            if(!string.IsNullOrWhiteSpace(text) && text.Length>3)
            {
                List<string> prefix = new List<string>()
                {
                    "ref",
                    "mst",
                    "trs",
                    "tmp",
                    "rkp",
                    "set",
                    "hr_",
                    "map",
                };
                if(prefix.Any(d=>d == text.Substring(0,3).ToLower()))
                    return text.Substring(3, text.Length - 3);
                
            }
            return text;
        }
        public static string FirstCharToLowerCase(string str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

            return str;
        }

        public static string GetPrefix(string text)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(text) && text.Length > 3)
            {
                switch(text.Substring(0,3).ToLower())
                {
                    case "ref": result= "Referensi"; break;
                    case "mst": result = "Master"; break;
                    case "trs": result = "Transaction"; break;
                    case "tmp": result = "Temporary"; break;
                    case "rkp": result = "Rekap"; break;
                    case "set": result = "Setting"; break;
                    case "hr_": result = "HumanResource"; break;
                    case "map": result = "Mapping"; break;
                }
            }
            return result;
        }
        public static string ParseType(Type type)
        {
            string typename = type.FullName.ToLower();
            if (typename.Contains("guid"))
                return "Guid";
            else if (typename.Contains("boolean"))
                return "bool";
            else if (typename.Contains("datetime"))
                return "DateTime";
            else if (typename.Contains("decimal"))
                return "decimal";
            else if (typename.Contains("double"))
                return "double";
            else if (typename.Contains("int32"))
                return "int";
            else if (typename.Contains("int64"))
                return "long";
            else if (typename.Contains("int16"))
                return "short";
            else if (typename.Contains("single"))
                return "float";
            else if (typename.Contains("string"))
                return "string";
            else if (typename.Contains("byte[]"))
                return "byte[]";
            else
                return type.FullName;

        }

        public static (bool have,string value) CheckCreateBy(IEnumerable<IProperty> list_properties)
        {
            List<string> create_by = new List<string>()
            {
                "createby",
                "create_by",
                "createdby",
                "created_by",
            };
            var properties = list_properties.Where(d => create_by.Contains(d.Name.ToLower())).FirstOrDefault();
            if (properties != null)
                return (true, properties.Name);

            return (false, "");
        }
        public static (bool have, string value) CheckCreateDate(IEnumerable<IProperty> list_properties)
        {
            List<string> create_by = new List<string>()
            {
                "createdate",
                "create_date",
                "createddate",
                "created_date",
            };
            var properties = list_properties.Where(d => create_by.Contains(d.Name.ToLower())).FirstOrDefault();
            if (properties != null)
                return (true, properties.Name);

            return (false, "");
        }
        public static (bool have, string value) CheckUpdateBy(IEnumerable<IProperty> list_properties)
        {
            List<string> create_by = new List<string>()
            {
                "updateby",
                "update_by",
                "updatedby",
                "updated_by",
            };
            var properties = list_properties.Where(d => create_by.Contains(d.Name.ToLower())).FirstOrDefault();
            if (properties != null)
                return (true, properties.Name);

            return (false, "");
        }
        public static (bool have, string value) CheckUpdateDate(IEnumerable<IProperty> list_properties)
        {
            List<string> create_by = new List<string>()
            {
                "updatedate",
                "update_date",
                "updateddate",
                "updated_date",
            };
            var properties = list_properties.Where(d => create_by.Contains(d.Name.ToLower())).FirstOrDefault();
            if (properties != null)
                return (true, properties.Name);

            return (false, "");
        }
    }
}
