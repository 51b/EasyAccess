﻿using System;
using System.Collections;
using System.Linq;
using EasyAccess.Infrastructure.Constant;
using EasyAccess.Infrastructure.Util.DataConverter;

namespace EasyAccess.Infrastructure.Util
{
    public class OperationResult
    {
        public OperationResult()
        {
            this.Extras = new ArrayList();
        }

        public OperationResult(StatusCode statusCode)
            :this()
        {
            this.StatusCode = statusCode;
        }

        public OperationResult(StatusCode statusCode, string message)
            : this(statusCode)
        {
            this.Message = message;
        }

        public OperationResult(StatusCode statusCode, string message, object data)
            :this(statusCode, message)
        {
            this.Data = data;
        }

        public OperationResult SetData<T>(T data) where T : class
        {
            this.Data = new DataConverter<T>().ToDictionary(data);
            return this;
        }

        public OperationResult SetExtras(params object[] extras)
        {
            if (extras.Length == 1)
            {
                this.Extras.Add(extras);
            }
            else
            {
                this.Extras.AddRange(extras);
            }
            return this;
        }

        public StatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public ArrayList Extras { get; set; }
    }
}
