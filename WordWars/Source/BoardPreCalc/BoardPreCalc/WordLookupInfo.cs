using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardPreCalc
{
    public class WordLookupInfoData
    {
        public bool IsWord = false;
        public bool HasChildren = false;

        public override bool Equals(object obj)
        {
            if (obj is WordLookupInfoData)
            {
                WordLookupInfoData other = (WordLookupInfoData) obj;
                return ((other.IsWord == IsWord) && (other.HasChildren == HasChildren));
            }
            else
                return base.Equals(obj);
        }
    }

    public class WordLookupInfo
    {
        public Dictionary<string, WordLookupInfoData> LookupInfo = new Dictionary<string, WordLookupInfoData>();
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in LookupInfo)
            {
                result.AppendFormat("Key: {0} \t\tIsWord: {1} \t\tHasChildren: {2}", item.Key, item.Value.IsWord, item.Value.HasChildren);
                result.AppendLine();
            }
            return result.ToString();
        }
    }
}
