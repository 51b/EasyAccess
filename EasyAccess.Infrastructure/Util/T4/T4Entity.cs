﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EasyAccess.Infrastructure.Util.T4
{
    /// <summary>
    /// T4实体模型信息类
    /// </summary>
    public class T4Entity
    {
        /// <summary>
        /// 获取 模型所在的程序集名称
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// 获取 模型所在的命名空间
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// 获取 模型所在的项目名称
        /// </summary>
        public string ProjectName { get; private set; }


        /// <summary>
        /// 获取 模型所在的项目的缩写名称
        /// </summary>
        public string ProjectNameAbbr { get; private set; }

        /// <summary>
        /// 获取 模型名称
        /// </summary>
        public string EntityName { get; private set; }

        /// <summary>
        /// 获取 模型描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 主键类型
        /// </summary>
        public Type KeyType { get; private set; }

        /// <summary>
        /// 主键类型名称
        /// </summary>
        public string KeyTypeName { get; private set; }

        public IEnumerable<PropertyInfo> Properties { get; private set; }

        public T4Entity(Type entityType, int projectNameIndex = 0, string projectNameAbbr = "")
        {
            var assemblyFullName = entityType.Assembly.FullName;
            AssemblyName = assemblyFullName.Substring(0, assemblyFullName.IndexOf(",", System.StringComparison.Ordinal));
            Namespace = entityType.Namespace;
            if (Namespace != null)
            {
                ProjectName = Namespace.Split('.')[projectNameIndex];
                ProjectNameAbbr = projectNameAbbr;
            }
            EntityName = entityType.Name;

            var keyProp = entityType.GetProperty("Id");
            KeyType = keyProp.PropertyType;
            KeyTypeName = KeyType.Name;

            var descAttributes = entityType.GetCustomAttributes(typeof(DescriptionAttribute), true);
            Description = descAttributes.Length == 1 ? ((DescriptionAttribute)descAttributes[0]).Description : EntityName;
            Properties = entityType.GetProperties();
        }
    }
}
