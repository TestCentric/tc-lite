// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// Contains the result of matching a <see cref="Constraint"/> against an actual value.
    /// </summary>
    public class ConstraintResult : IConstraintResult
    {
        readonly IConstraint _constraint;

		// Prefixes used in all failure messages. All must be the same
		// length, which is held in the PrefixLength field. Should not
		// contain any tabs or newline characters.
		/// <summary>
		/// Prefix used for the expected value line of a message
		/// </summary>
		protected static readonly string Pfx_Expected = "  Expected: ";

		/// <summary>
		/// Prefix used for the actual value line of a message
		/// </summary>
		protected static readonly string Pfx_Actual = "  But was:  ";

		/// <summary>
		/// Length of a message prefix
		/// </summary>
		public static readonly int PrefixLength = Pfx_Expected.Length;
		
        #region Constructors

        /// <summary>
        /// Constructs a <see cref="ConstraintResult"/> for a particular <see cref="Constraint"/>.
        /// </summary>
        /// <param name="constraint">The Constraint to which this result applies.</param>
        /// <param name="actualValue">The actual value to which the Constraint was applied.</param>
        public ConstraintResult(IConstraint constraint, object actualValue)
        {
            _constraint = constraint;
            ActualValue = actualValue;
        }

        /// <summary>
        /// Constructs a <see cref="ConstraintResult"/> for a particular <see cref="Constraint"/>.
        /// </summary>
        /// <param name="constraint">The Constraint to which this result applies.</param>
        /// <param name="actualValue">The actual value to which the Constraint was applied.</param>
        /// <param name="status">The status of the new ConstraintResult.</param>
        public ConstraintResult(IConstraint constraint, object actualValue, ConstraintStatus status)
            : this(constraint, actualValue)
        {
            Status = status;
        }

        /// <summary>
        /// Constructs a <see cref="ConstraintResult"/> for a particular <see cref="Constraint"/>.
        /// </summary>
        /// <param name="constraint">The Constraint to which this result applies.</param>
        /// <param name="actualValue">The actual value to which the Constraint was applied.</param>
        /// <param name="isSuccess">If true, applies a status of Success to the result, otherwise Failure.</param>
        public ConstraintResult(IConstraint constraint, object actualValue, bool isSuccess)
            : this(constraint, actualValue)
        {
            Status = isSuccess ? ConstraintStatus.Success : ConstraintStatus.Failure;
        }

        #endregion

        #region IConstraintResult Members

        /// <summary>
        /// Display friendly name of the constraint.
        /// </summary>
        public string DisplayName => _constraint.DisplayName;

        /// <summary>
        /// Gets and sets the ResultStatus for this result.
        /// </summary>
        public ConstraintStatus Status { get; set; }

        /// <summary>
        /// True if actual value meets the Constraint criteria otherwise false.
        /// </summary>
        public virtual bool IsSuccess => Status == ConstraintStatus.Success;

        /// <summary>
        /// Description of the constraint may be affected by the state the constraint had
        /// when <see cref="Constraint.ApplyConstraint{TActual}(TActual)"/> was performed against the actual value.
        /// </summary>
        public string Description => _constraint.Description;

        /// <summary>
        /// The actual value that was passed to the <see cref="Constraint.ApplyConstraint{TActual}(TActual)"/> method.
        /// </summary>
        public object ActualValue { get; }

        #endregion

        #region Write Methods

        /// <summary>
        /// Write the failure message to the MessageWriter provided
        /// as an argument. The default implementation simply passes
        /// the result and the actual value to the writer, which
        /// then displays the constraint description and the value.
        ///
        /// Constraints that need to provide additional details,
        /// such as where the error occurred, can override this.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display the message</param>
        public virtual void WriteMessageTo(MessageWriter writer)
        {
            WriteExpectedLineTo(writer);
            WriteActualLineTo(writer);
            WriteAdditionalLinesTo(writer);
        }

        public virtual void WriteExpectedLineTo(MessageWriter writer)
        {
            writer.Write(Pfx_Expected);
            writer.WriteLine(Description);
        }

        public virtual void WriteActualLineTo(MessageWriter writer)
        {
            writer.Write(Pfx_Actual);
            writer.WriteActualValue(ActualValue);
            writer.WriteLine();
        }

        /// <summary>
        /// Write some additional failure message.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display the message</param>
        public virtual void WriteAdditionalLinesTo(MessageWriter writer)
        {
            //By default it does not write anything to writer but can be overriden in classes where needed. 
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. The default implementation simply writes
        /// the raw value of actual, leaving it to the writer to
        /// perform any formatting.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public virtual void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteActualValue(ActualValue);
        }

        #endregion
    }
}
