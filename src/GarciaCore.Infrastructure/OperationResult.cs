using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public class OperationResult
    {
        public virtual bool Success { get; set; }
        public virtual ValidationResults ValidationResults { get; set; }
        /// <summary>
        /// Returns localized text for the first validation result if exists. Returns empty string otherwise.
        /// </summary>
        public string LocalizedValidationMessage
        {
            get
            {
                // TODO
                // return GarciaLocalizationManager.Localize(this.ValidationMessage);
                return ValidationMessage;
            }
        }

        /// <summary>
        /// Returns text for the first validation result if exists. Returns empty string otherwise.
        /// </summary>
        public string ValidationMessage
        {
            get
            {
                if (ValidationResults == null || ValidationResults.Count == 0 || ValidationResults[0] == null || ValidationResults[0].Messages == null)
                {
                    return null;
                }

                return ValidationResults[0].Messages[0];
            }
        }

        protected OperationResult(bool success, string validationError)
        {
            ValidationResults = new ValidationResults();
            Success = success;

            if (!string.IsNullOrEmpty(validationError))
            {
                ValidationResults.Add(new ValidationResult(validationError));
            }
        }

        protected OperationResult(bool success, ValidationResults validationResults)
        {
            ValidationResults = validationResults;
            Success = success;
        }

        /// <summary>
        /// Success: false
        /// </summary>
        /// <param name="ValidationError"></param>
        public OperationResult(string validationError)
            : this(false, validationError)
        {
        }

        /// <summary>
        /// Success: true
        /// </summary>
        /// <param name="ValidationError"></param>
        public OperationResult()
            : this(true, string.Empty)
        {
        }

        public ValidationResult AddValidationResult(string message)
        {
            var validationResult = new ValidationResult(message);
            ValidationResults.Add(validationResult);
            return validationResult;
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Item { get; set; }

        protected OperationResult(T item, bool success, string validationError)
            : base(success, validationError)
        {
            Item = item;
        }

        protected OperationResult(T item, bool success, ValidationResults validationResults)
           : base(success, validationResults)
        {
            Item = item;
        }

        public OperationResult(T item, string validationError)
            : this(item, string.IsNullOrEmpty(validationError), validationError)
        {
        }

        public OperationResult(T item)
            : this(item, validationError: null)
        {
        }

        /// <summary>
        /// Success: false
        /// </summary>
        public OperationResult()
            : this(default(T), false, validationError: null)
        {
        }

        public OperationResult(OperationResult result, T item)
            : this(item, result.Success, result.ValidationResults)
        {

        }
    }
}
