﻿using System.Collections.Generic;
using System.Linq;
using EasyAccess.Infrastructure.Util.DataConverter;

namespace EasyAccess.Infrastructure.Util.EasyUi
{
    public class EasyUiDataGrid
    {
        /// <summary>
        /// 记录总数
        /// </summary>
        private long _total = 0;

        /// <summary>
        /// rows
        /// </summary>
        private ICollection<Dictionary<string, object>> _rows = new List<Dictionary<string, object>>();

        /// <summary>
        /// footer
        /// </summary>
        private ICollection<Dictionary<string, object>> _footer = new List<Dictionary<string, object>>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EasyUiDataGrid() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="total">记录总数</param>
        /// <param name="rows">行记录</param>
        public EasyUiDataGrid(long total, ICollection<Dictionary<string, object>> rows)
        {
            this._total = total;
            this._rows = rows;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="total">记录总数</param>
        /// <param name="rows">行记录</param>
        /// <param name="footer">汇总记录</param>
        public EasyUiDataGrid(long total, ICollection<Dictionary<string, object>> rows, IList<Dictionary<string, object>> footer)
            :this(total, rows)
        {
            this._footer = footer;
        }

        /// <summary>
        /// 设置rows
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="total">总记录数</param>
        /// <param name="rows">List数据</param>
        /// <returns>EasyUiDataGrid实例</returns>
        public EasyUiDataGrid SetRows<T>(long total, ICollection<T> rows) where T : class
        {
            this._total = total;
            if (total > 0)
            {
                this._rows = new DataConverter<T>().ToDictionary(rows);
            }
            return this;
        }
        
        /// <summary>
        /// 设置Footer
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="footer">List数据</param>
        /// <returns>EasyUiDataGrid实例</returns>
        public EasyUiDataGrid SetFooter<T>(ICollection<T> footer) where T : class
        {
            this._footer = new DataConverter<T>().ToDictionary(footer);
            return this;
        }

        /// <summary>
        /// 添加行记录
        /// </summary>
        /// <param name="rows">行记录</param>
        /// <returns>EasyUiDataGrid实例</returns>
        public EasyUiDataGrid AddRows(ICollection<Dictionary<string, object>> rows)
        {
            foreach (var row in rows)
            {
                _rows.Add(row);
            }
            return this;
        }

        /// <summary>
        /// 添加汇总信息
        /// </summary>
        /// <param name="footer">数据</param>
        /// <returns>EasyUiDataGrid实例</returns>
        public EasyUiDataGrid AddFooter(ICollection<Dictionary<string, object>> footer)
        {
            foreach (var row in footer)
            {
                _footer.Add(row);
            }
            return this;
        }

        /// <summary>
        /// 获取可序列化的Model
        /// </summary>
        /// <returns>object</returns>
        public object GetSerializableModel()
        {
            if (_footer.Count != 0)
            {
                return new { total = _total, rows = _rows, footer = _footer };
            }
            else
            {
                return new { total = _total, rows = _rows };
            }
        }
    }
}
