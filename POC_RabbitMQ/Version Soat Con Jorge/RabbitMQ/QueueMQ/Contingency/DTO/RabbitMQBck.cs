
namespace QueueMQ.Contingency.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Globalization;

    public class RabbitMQBck //: ISQLiteModel
    {
        private const string _DATEFORMAT = "MM/dd/yyyy HH:mm:ss.fff";
        private const string _TABLENAME = "RabbitMQBck";
        public string TableName { get { return _TABLENAME; } set { } }
        public int id { get; set; }
        public string json { get; set; }
        public string destiny { get; set; }
        private RabbitMQDestinyType.Type _type;
        public object type
        {
            get
            {
                return _type;
            }
            set
            {
                switch (value)
                {
                    case string stringValue: _type = (RabbitMQDestinyType.Type)int.Parse(value.ToString()); break;
                    case int intValue: _type = (RabbitMQDestinyType.Type)intValue; break;
                }
            }
        }
        private DateTime _date { get; set; }
        public object date
        {
            get { return _date; }
            set
            {
                switch (value)
                {
                    case string stringValue: _date = DateTime.ParseExact(stringValue, _DATEFORMAT, CultureInfo.InvariantCulture); break;
                    case DateTime dateValue: _date = dateValue; break;
                }
            }
        }
        private RabbitMQEstatus.Type _status;
        public object status
        {
            get
            {
                return _status;
            }
            set
            {
                switch (value)
                {
                    case string stringValue: _status = (RabbitMQEstatus.Type)int.Parse(value.ToString()); break;
                    case int intValue: _status = (RabbitMQEstatus.Type)intValue; break;
                }
            }
        }

        public RabbitMQBck() { }
        public bool Save() { return true; }

        /* bool ISQLiteModel.Save()
         {
             throw new NotImplementedException();
         }*/
        string getUpdateCommand()
        {
            return (new StringBuilder())
                .Append("UPDATE ")
                .Append(_TABLENAME)
                .Append(" SET ")
                .Append(" json =\"")
                .Append(json)
                .Append("\",")
                .Append(" destiny =\"")
                .Append(destiny.ToString())
                .Append("\",")
                .Append(" type =")
                .Append(type.ToString())
                .Append(",")
                .Append(" date =\"")
                .Append(((DateTime)date).ToString(_DATEFORMAT))
                .Append("\",")
                .Append(" status =")
                .Append(status.ToString())
                .Append(" WHERE id = ")
                .Append(id.ToString()).ToString();
        }
        string getInsertCommand()
        {
            return (new StringBuilder())
                .Append("INSERT INTO ")
                .Append(_TABLENAME)
                .Append(" (json,destiny,type,date,status) ")
                .Append(" VALUES (")
                .Append("\"")
                .Append(json)
                .Append("\",")
                .Append("\"")
                .Append(destiny.ToString())
                .Append("\",")
                .Append(type.ToString())
                .Append(",")
                .Append("\"")
                .Append(((DateTime)date).ToString(_DATEFORMAT))
                .Append("\",")
                .Append(status.ToString())
                .Append(")")
                .Append(id.ToString()).ToString();
        }
        /* bool Save() {

         }*/
        bool setValues(Dictionary<string, object> data)
        {
            try
            {
                data.Select(item =>
                {
                    switch (item.Key.ToUpper())
                    {
                        case "JSON": json = item.Value.ToString(); break;
                        case "DESTINY": destiny = item.Value.ToString(); break;
                        case "TYPE": type = item.Value; break;
                        case "DATE": date = item.Value; break;
                        case "STATUS": status = item.Value; break;
                    }
                    return 0;
                }).ToArray();
                return true;
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            {
                //throw Ex;
                return false;
            }
        }
        // bool Update();

        /* bool ISQLiteModel.Select(int id)
         {
             string SelectCommand = (new StringBuilder())
                                     .Append("SELECT * FROM ")
                                     .Append(_TABLENAME)
                                     .Append("WHERE id = ")
                                     .Append(id.ToString())
                                     .ToString();

         }*/
    }

}
