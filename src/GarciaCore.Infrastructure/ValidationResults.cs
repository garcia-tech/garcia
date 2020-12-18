using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public class ValidationResults : List<ValidationResult>
    {
        public bool IsValid
        {
            get
            {
                return this.Count(x => !x.IsValid) == 0;
            }
        }
    }

    public class ValidationResults<T> : List<ValidationResult<T>>
    {
        public bool IsValid
        {
            get
            {
                return this.Count(x => !x.IsValid) == 0;
            }
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Messages { get; set; }
        public string PropertyName { get; set; }

        protected ValidationResult(bool isValid, List<string> messages, string propertyName = null)
        {
            this.IsValid = isValid;
            this.Messages = messages;
            this.PropertyName = propertyName;

            if (messages == null)
                messages = new List<string>();
        }

        //public ValidationResult(bool IsValid, string Message)
        //    : this(IsValid, new List<string>() { Message })
        //{
        //}

        ///// <summary>
        ///// IsValid: false
        ///// </summary>
        //public ValidationResult()
        //    : this(false, new List<string>())
        //{
        //}

        /// <summary>
        /// IsValid: false
        /// </summary>
        public ValidationResult(string message, string propertyName = null)
            : this(false, new List<string>() { message }, propertyName)
        {
        }

        /// <summary>
        /// IsValid: false
        /// </summary>
        public ValidationResult(List<string> messages, string propertyName = null)
            : this(false, messages, propertyName)
        {
        }
    }

    public class ValidationResult<T> : ValidationResult
    {
        public T Item { get; set; }

        protected ValidationResult(T item, bool isValid, List<string> messages, string propertyName = null)
            : base(isValid, messages, propertyName)
        {
            this.Item = item;
        }

        /// <summary>
        /// IsValid: false
        /// </summary>
        public ValidationResult(T item, string message, string propertyName = null)
            : this(item, false, new List<string>() { message }, propertyName)
        {
        }

        /// <summary>
        /// IsValid: false
        /// </summary>
        public ValidationResult(T item, string propertyName = null)
            : this(item, false, new List<string>(), propertyName)
        {
        }
    }
}
