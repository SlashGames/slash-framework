// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationError.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Validation
{
    /// <summary>
    ///   Information about a validation error of a value for a inspector property.
    /// </summary>
    public class ValidationError
    {
        #region Constants

        /// <summary>
        ///   Default message to use for invalid values.
        /// </summary>
        public const string DefaultMessage = "Value is invalid.";

        /// <summary>
        ///   Message to use for values which are null.
        /// </summary>
        public const string NullMessage = "Value is null.";

        /// <summary>
        ///   Message to use for values which have the wrong type.
        /// </summary>
        public const string WrongTypeMessage = "Value is of wrong type.";

        #endregion

        #region Public Properties

        /// <summary>
        ///   Returns a default validation error.
        /// </summary>
        public static ValidationError Default
        {
            get
            {
                return new ValidationError { Message = DefaultMessage };
            }
        }

        /// <summary>
        ///   Returns a null validation error.
        /// </summary>
        public static ValidationError Null
        {
            get
            {
                return new ValidationError { Message = NullMessage };
            }
        }

        /// <summary>
        ///   Returns a wrong type validation error.
        /// </summary>
        public static ValidationError WrongType
        {
            get
            {
                return new ValidationError { Message = WrongTypeMessage };
            }
        }

        /// <summary>
        ///   Message which describes the validation error.
        /// </summary>
        public string Message { get; set; }

        #endregion
    }
}