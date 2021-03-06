﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EasyAccess.Infrastructure.Extensions;

namespace EasyAccess.Infrastructure.Util.DataConverter
{
    public static class ConvertToList
    {
        public static List<T> ToList<T>(this DataConverter<T> dataConverter, DataTable dataTable, ConvertToListOptions<T> options = null) where T : class, new()
        {
            InitOptions(dataConverter, ref options);
            ValidateColumnMapper<T>(dataTable, options);
            return Conver(dataConverter, dataTable, options);
        }

        private static List<T> Conver<T>(DataConverter<T> dataConverter, DataTable dataTable, ConvertToListOptions<T> options) where T : class, new()
        {
            var lst = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                var item = new T();
                foreach (var columnInfo in options.ColumnMapper)
                {
                    columnInfo.Value.SetValue(item, GetValue(options, columnInfo, row));
                }
                lst.Add(item);
            }
            return lst;
        }

        private static object GetValue<T>(ConvertToListOptions<T> options, KeyValuePair<string, PropertyInfo> columnInfo, DataRow row)
            where T : class, new()
        {
            object val = null;
            if (options.Projection.ContainsKey(columnInfo.Key))
            {
                val = options.Projection[columnInfo.Key].GetMethodInfo().GetParameters().Count() == 1 
                    ? options.Projection[columnInfo.Key].DynamicInvoke(row[columnInfo.Key]) 
                    : options.Projection[columnInfo.Key].DynamicInvoke(row[columnInfo.Key], row);
            }
            else if (columnInfo.Value.PropertyType.IsEnum)
            {
                val = Enum.Parse(columnInfo.Value.PropertyType, row[columnInfo.Key].ToString());
            }
            else
            {
                if (string.IsNullOrWhiteSpace(row[columnInfo.Key].ToString()) && columnInfo.Value.PropertyType.IsNumeric())
                {
                    val = Convert.ChangeType(0, columnInfo.Value.PropertyType);
                }
                else
                {
                    val = Convert.ChangeType(row[columnInfo.Key], columnInfo.Value.PropertyType);
                }
            }
            return val;
        }

        private static void InitOptions<T>(DataConverter<T> dataConverter, ref ConvertToListOptions<T> options) where T : class
        {
            if (options == null)
            {
                options = new ConvertToListOptions<T>();
                foreach (var property in dataConverter.PropertyToShow)
                {
                    options.MapColumn(property.Name, property);
                }
            }
        }

        private static void ValidateColumnMapper<T>(DataTable dataTable, ConvertToListOptions<T> options) where T : class
        {
            var requireColumns = options.ColumnMapper.Select(x => x.Key);
            var missColumns = requireColumns.Where(x => !dataTable.Columns.Contains(x)).ToArray();
            if (missColumns.Count() != 0)
            {
                throw new KeyNotFoundException("缺少列名为：【" + string.Join("】,【", missColumns) + "】的配置信息");
            }
        }
    }

    public class ConvertToListOptions<T> where T : class
    {
        public Dictionary<string, PropertyInfo> ColumnMapper { get; private set; }
        public Dictionary<string, Delegate> Projection { get; private set; }


        public ConvertToListOptions()
        {
            ColumnMapper = new Dictionary<string, PropertyInfo>();
            Projection = new Dictionary<string, Delegate>();
        }

        private ConvertToListOptions<T> MapColumnExpr<TProperty>(Expression<Func<T, TProperty>> expr, string columnName, Delegate projection = null)
        {
            PropertyInfo propertyInfo;
            if (expr.Body is UnaryExpression)
            {
                propertyInfo = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member as PropertyInfo;
            }
            else if (expr.Body is MemberExpression)
            {
                propertyInfo = ((MemberExpression)expr.Body).Member as PropertyInfo;
            }
            else
            {
                throw new NotSupportedException();
            }
            return MapColumn(columnName, propertyInfo, projection);
        }

        public ConvertToListOptions<T> MapColumn<TProperty>(Expression<Func<T, TProperty>> expr, string columnName, Func<string, DataRow, TProperty> projection)
        {
            return MapColumnExpr(expr, columnName, projection);
        }

        public ConvertToListOptions<T> MapColumn<TProperty>(Expression<Func<T, TProperty>> expr, string columnName, Func<string, TProperty> projection = null)
        {
            return MapColumnExpr(expr, columnName, projection);
        }

        public ConvertToListOptions<T> MapColumn(string columnName, PropertyInfo propertyInfo, Delegate projection = null)
        {
            if (!ColumnMapper.ContainsKey(columnName))
            {
                ColumnMapper.Add(columnName, propertyInfo);
                if (projection != null)
                {
                    Projection.Add(columnName, projection);
                }
                return this;
            }
            else
            {
                throw new MappingException("列名【" + columnName + "】重复配置");
            }
        }
    }
}