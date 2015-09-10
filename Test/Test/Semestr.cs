using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Semestr
    {
        int number_;
        public List<Subject> LS_;
        int stat_; //0 нет совпадений, 1 есть, 2 полное

        public int stat
        {
            get { return stat_; }
            set { stat_ = value; }
        }

        public int number
        {
            get { return number_; }
            set
            {
                if (value < 0) number_ = 0; else number_ = value;

            }
        }

        public Semestr(int in_number, List<Subject> in_LS)
        {
            LS_ = new List<Subject>();
            LS_.AddRange(in_LS);
            number = in_number;
            stat_ = 0;
        }
    }
}
