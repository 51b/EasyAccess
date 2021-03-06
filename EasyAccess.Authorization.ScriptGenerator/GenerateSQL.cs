﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using EasyAccess.Authorization.Attr;
using EasyAccess.Authorization.ScriptGenerator.SqlHelper;

namespace EasyAccess.Authorization.ScriptGenerator
{
    public class GenerateSql
    {
        private string DllName { get; set; }
        private string DllFilePath { get; set; }
        private string ScriptPath { get; set; }
        private ISqlStatement SqlStatement { get; set; }

        private IDictionary<string, MenuAttribute> menuDic = new Dictionary<string, MenuAttribute>();
        private IDictionary<string, PermissionAttribute> permissionDic = new Dictionary<string, PermissionAttribute>();
        private IList<Type> targetTypes = new List<Type>();

        public GenerateSql(string path, string outputPath, string dllName, string outputName, ISqlStatement sqlStatement)
        {
            this.DllName = dllName;
            this.DllFilePath = path;
            this.SqlStatement = sqlStatement;
            this.ScriptPath = outputPath + @"\" + outputName;
        }

        public void CreateScript(out string msg)
        {
            try
            {
                DeleteUsedScript();
                CreateNewScript();

                msg = "脚本已生成，路径：" + ScriptPath;
            }
            catch (Exception ex)
            {
                msg = "生成失败，错误信息：" + ex.ToString();
            }
        }

        private void DeleteUsedScript()
        {
            if (File.Exists(ScriptPath))
            {
                File.Delete(ScriptPath);
            }
        }

        private void CreateNewScript()
        {
            var fileStream = new FileStream(ScriptPath, FileMode.Create);
            var streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding("UTF-8"));

            WriteStream(streamWriter);

            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }

        private void WriteStream(StreamWriter streamWriter)
        {
            DirectoryInfo dir = new DirectoryInfo(DllFilePath);
            var file = dir.GetFiles().FirstOrDefault(x => x.Name == DllName);
            if (file != null)
            {
                Assembly assembly = Assembly.LoadFrom(file.FullName);
                Type[] types = assembly.GetTypes();
                streamWriter.Write(SqlText(types));
            }

        }

        private string SqlText(IEnumerable<Type> types)
        {
            VerifyTypes(types);

            var sqlText = new StringBuilder();

            sqlText.Append(SqlStatement.BeforeGen());

            var menuLst = new List<MenuAttribute>();
            foreach (var type in targetTypes)
            {
                menuLst.AddRange(GetMenuInstance(type));
            }
            menuLst = menuLst.OrderBy(x => x.Depth).ThenBy(x => x.Index).ToList();
            foreach (var menu in menuLst)
            {
                sqlText.Append(MenuSql(menu));
            }
            sqlText.Append(DelectUsesdMenu());

            foreach (var type in targetTypes)
            {
                sqlText.Append(PermissionSql(type));
            }
            sqlText.Append(DeleteUsedPermission());

            sqlText.Append(SqlStatement.AfterGen());

            sqlText.Replace("''", "NULL");

            return sqlText.ToString();
        }

        private void VerifyTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (type.GetCustomAttributes<MenuAttribute>(false).Any())
                {
                    targetTypes.Add(type);
                }
            }
        }

        private IEnumerable<MenuAttribute> GetMenuInstance(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var menuLst = new List<MenuAttribute>();
            var attrs = type.GetCustomAttributes<MenuAttribute>(false);
            foreach (MenuAttribute attr in attrs)
            {
                if (menuDic.ContainsKey(attr.Id))
                {
                    throw new Exception("存在重复的MenuAttribute.Id :" + attr.Id);
                }
                else
                {
                    menuDic.Add(attr.Id, attr);
                }
                menuLst.Add(attr);
            }
            return menuLst;
        }

        private string MenuSql(MenuAttribute menu)
        {
            return string.Format(SqlStatement.GenMenu(), new object[] {
                    menu.Id,
                    menu.ParentId,
                    menu.Name,
                    menu.Url,
                    menu.System,
                    menu.Depth,
                    menu.Index
                });
        }

        private string DelectUsesdMenu()
        {
            string sql = "";
            if (menuDic.Keys.Count > 0)
            {
                var deleteTarget = new StringBuilder();
                foreach (var id in menuDic.Keys)
                {
                    deleteTarget.Append("'" + id + "',");
                }
                sql = string.Format(SqlStatement.DeleteMenu(), deleteTarget.ToString().Substring(0, deleteTarget.Length - 1));
            }
            return sql;
        }

        private string PermissionSql(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var sql = new StringBuilder();
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                var attrs = method.GetCustomAttributes(typeof(PermissionAttribute), false);
                foreach (PermissionAttribute attr in attrs)
                {
                    if (attr.Id == null || attr.Dependent == true)
                    {
                        continue;
                    } 
                    else if (attr.MenuId == null){
                        throw new Exception("PermissionAttribute.Id :" + attr.Id + " 未指定MenuId");
                    }
                    else if (permissionDic.ContainsKey(attr.Id))
                    {
                        throw new Exception("存在重复的PermissionAttribute.Id :" + attr.Id);
                    }
                    else if (!menuDic.ContainsKey(attr.MenuId))
                    {
                        throw new Exception("不存在MenuAttribute.Id :" + attr.MenuId);
                    }
                    else
                    {
                        permissionDic.Add(attr.Id, attr);

                        sql.Append(string.Format(SqlStatement.GenPermission(), new object[] {
                            attr.Id,
                            attr.MenuId,
                            attr.Name,
                            attr.ActionUrl
                        }));
                    }
                }
            }
            return sql.ToString();
        }

        private string DeleteUsedPermission()
        {
            string sql = "";
            if (permissionDic.Keys.Count > 0)
            {
                var deleteTarget = new StringBuilder();
                foreach (var id in permissionDic.Keys)
                {
                    deleteTarget.Append("'" + id + "',");
                }
                sql = string.Format(SqlStatement.DeletePermission(), deleteTarget.ToString().Substring(0, deleteTarget.Length - 1));
            }
            return sql;
        }

    }
}
