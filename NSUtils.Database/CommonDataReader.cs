using System.Reflection;
using System.Collections.Generic;
using NSUtils;

namespace NSUtils.Database
{
    public class CommonDataReader<TModel> where TModel : class, new()
    {
        private bool _isEmpty;
        public bool isEmpty
        {
            get
            {
                return _isEmpty;
            }
        }

        private List<PropertyInfo> _propertiesMapped;
                
        public List<CommonColumn> Columns { get; set; }

        public List<TModel> Rows { get; set; }

        public CommonDataReader()
        {
            Columns = new List<CommonColumn>();
            Rows = new List<TModel>();
            _propertiesMapped = new List<PropertyInfo>();
            _isEmpty = true;
        }

        public CommonDataReader(object[] rawRow, List<string> names) : this()
        {
            _isEmpty = false;
            for(int i = 0; i < names.Count; i++)
            {
                Columns.Add(new CommonColumn() { Name = names[i], ColumnType = rawRow[i].GetType() });
            }

            MapProperties();
            Add(rawRow);
        }

        private void MapProperties()
        {
            for(int i = 0; i < Columns.Count; i++)
            {
                _propertiesMapped.Add(typeof(TModel).GetRuntimeProperty(Columns[i].Name));
            }
        }

        public void Add(object[] rawRow)
        {
            var model = new TModel();

            for(int i = 0; i < Columns.Count; i++)
            {
                _propertiesMapped[i].SetValueSecure(rawRow[i], model);
            }

            Rows.Add(model);
        }

        
    }
}
