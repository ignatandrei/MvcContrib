using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCContrib.Export.Renderer
{
    public abstract class Renderer<T>
       where T : class
    {
        public abstract byte[] GetResult();
        public IEnumerable<T> dataSource;
        public List<Column<T>> Columns { get; set; }

    }

    public abstract class StringRenderer<T> : Renderer<T>
        where T : class
    {

        public Encoding e = Encoding.UTF8;
        public abstract string GetFileContentsAsString();
        public override byte[] GetResult()
        {
            return e.GetBytes(GetFileContentsAsString());
        }


    }
}
