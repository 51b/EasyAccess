﻿using System.Web.Routing;
using System.Web.Security;
using EasyAccess.Authorization;

// ReSharper disable CheckNamespace
namespace System.Web.Mvc.Html
// ReSharper restore CheckNamespace
{
    public static partial class HtmlExtensions
    {
        /// <summary>
        /// 生成用户定制的标签元素
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="roleIdList">用户Id</param>
        /// <param name="permissionId">权限Id</param>
        /// <param name="tagType">标签类型</param>
        /// <param name="tagId">标签Id</param>
        /// <param name="attributes">属性</param>
        /// <param name="className">样式</param>
        /// <param name="innerHtml">内部Html</param>
        /// <returns>tagType指定的标签元素</returns>
        public static MvcHtmlString Tag(
            this HtmlHelper helper,
            string roleIdList,
            string permissionId,
            TagType tagType,
            string tagId,
            object attributes,
            string className,
            string innerHtml
            )
        {
            if (AuthorizationManager.GetInstance().VerifyPermission(permissionId, roleIdList))
            {
                return HtmlBuilder(tagType, tagId, attributes, className, innerHtml);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 构建Html
        /// </summary>
        /// <param name="tagType">标签类型</param>
        /// <param name="tagId">标签Id</param>
        /// <param name="attributes">属性</param>
        /// <param name="className">样式</param>
        /// <param name="innerHtml">内部Html</param>
        /// <returns>标签元素</returns>
        private static MvcHtmlString HtmlBuilder(
            TagType tagType,
            string tagId,
            object attributes,
            string className,
            string innerHtml
            )
        {
            var attrs = new RouteValueDictionary(attributes);
            return HtmlBuilder(tagType, tagId, attrs, className, innerHtml);
        }

        /// <summary>
        /// 构建Html
        /// </summary>
        /// <param name="tagType">标签类型</param>
        /// <param name="tagId">标签Id</param>
        /// <param name="attributes">属性</param>
        /// <param name="className">样式</param>
        /// <param name="innerHtml">内部Html</param>
        /// <returns>标签元素</returns>
        private static MvcHtmlString HtmlBuilder(
            TagType tagType,
            string tagId,
            RouteValueDictionary attributes,
            string className,
            string innerHtml
            )
        {

            var builder = new TagBuilder(tagType.ToString()) { IdAttributeDotReplacement = "_" };
            if (!string.IsNullOrWhiteSpace(tagId))
            {
                builder.GenerateId(tagId);
            }
            if (!attributes.ContainsKey("class"))
            {
                if (!string.IsNullOrWhiteSpace(className))
                {
                    builder.AddCssClass(className);
                }
            }
            builder.MergeAttributes(attributes);
            builder.InnerHtml = innerHtml;
            return MvcHtmlString.Create(builder.ToString());
        }
    }

    /// <summary>
    /// 标签类型
    /// </summary>
    public enum TagType
    {
// ReSharper disable InconsistentNaming
        a,
        input,
        button,
        div,
        span,
        li,
        script
// ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// 封装了Tag所需的参数信息
    /// </summary>
    public class TagParams
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionId">权限Id</param>
        public TagParams(string permissionId)
        {
            this.PermissionId = permissionId;
        }

        /// <summary>
        /// 角色ID列表
        /// </summary>
        public string RoleIdList { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public TagType TagType { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public string TagId { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public object Attributes { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 内部Html
        /// </summary>
        public string InnerHtml { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// 附加参数
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <returns></returns>
        public bool VerifyPermission()
        {
            return AuthorizationManager.GetInstance().VerifyPermission(this.PermissionId, ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData);
        }
    }
}
