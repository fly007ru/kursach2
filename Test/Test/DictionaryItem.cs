using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Test
{
    class DictionaryItem
    {
       public string name;
       public bool status;

        public DictionaryItem(string in_name, bool in_status)
       {
           name = in_name;
           status = in_status;
       }
        public XElement XML()
        {
            XElement x = new XElement("DictionaryItem", null);
            x.SetAttributeValue("name",name);
            x.SetAttributeValue("status",status);
            return x;
        }
    }
}
