using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 카테고리테스트
{
    public class DataFormat
    {
        public int index;
        public string name;
        public int parentIndex;
        public List<DataFormat> childList;

        private DataFormat() { }

        public DataFormat(int index, string name, int parentIndex)
        {
            this.index = index;
            this.name = name;
            this.parentIndex = parentIndex;
            childList = new List<DataFormat>();
        }

    }
}
