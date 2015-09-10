using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class SubjForCompare
    {
        public Subject subj;
        public List<string> lspecname;
        public List<DictionaryItem> lDic;

        public SubjForCompare(Subject in_subject, List<string> in_lspecname, List<DictionaryItem> in_lDic)
        {
            subj = new Subject(in_subject.profile, in_subject.number, in_subject.cikl, in_subject.name, in_subject.forma, in_subject.time);
            if (in_lspecname != null) lspecname = new List<string>(in_lspecname); else lspecname = new List<string>();
            if (in_lDic != null) lDic = new List<DictionaryItem>(in_lDic); else lDic = new List<DictionaryItem>();
        }

    }
}
