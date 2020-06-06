// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;
using System.Xml;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Tests
{
    /// <summary>
    /// The TestMethod class represents a Test implemented as a method.
    /// Because of how exceptions are handled internally, this class
    /// must incorporate processing of expected exceptions. A change to
    /// the Test interface might make it easier to process exceptions
    /// in an object that aggregates a TestMethod in the future.
    /// </summary>
	public class TestMethod : Test
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TestMethod"/> class.
        /// </summary>
        /// <param name="method">The method to be used as a test.</param>
        /// <param name="parentSuite">The suite or fixture to which the new test will be added</param>
        public TestMethod(MethodInfo method, ITest parentSuite) 
			: base( method.ReflectedType ) 
		{
            Name = method.Name;
            FullName += "." + Name;

            // Disambiguate call to base class methods
            // TODO: This should not be here - it's a presentation issue
            if( method.DeclaringType != method.ReflectedType)
                Name = method.DeclaringType.Name + "." + method.Name;

            // Needed to give proper fullname to test in a parameterized fixture.
            // Without this, the arguments to the fixture are not included.
            string prefix = method.ReflectedType.FullName;
            if (parentSuite != null)
            {
                prefix = parentSuite.FullName;
                FullName = prefix + "." + Name;
            }

            Method = method;
            MethodName = Method.Name;
        }

        #region Properties

        /// <summary>
        /// The ParameterSet used to create this test method
        /// </summary>
        internal ParameterSet TestCaseParameters { get; set; }

#if NYI
        /// <summary>
        /// Gets a list of custom decorators for this test.
        /// </summary>
        public IList<ICommandDecorator> CustomDecorators { get; } = new List<ICommandDecorator>();
#endif
        internal bool HasExpectedResult => TestCaseParameters?.HasExpectedResult ?? false;

        internal object ExpectedResult => TestCaseParameters?.ExpectedResult;

#if NYI
        internal bool IsAsync => Method.IsDefined(typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute), false);
#endif

        #endregion

        #region Test Overrides

        /// <summary>
        /// Returns an XmlNode representing the current result after
        /// adding it as a child of the supplied parent node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="recursive">If true, descendant results are included</param>
        /// <returns></returns>
        public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            XmlNode thisNode = base.AddToXml(parentNode, recursive);

            thisNode.AddAttribute("seed", Seed.ToString());

            return thisNode;
        }

        /// <summary>
        /// Gets the name used for the top-level element in the
        /// XML representation of this test
        /// </summary>
        public override string XmlElementName
        {
            get { return "test-case"; }
        }

        #endregion

        #region Helper Methods

#if NYI
        private TestCommand ApplyDecoratorsToCommand(TestCommand command)
        {
            CommandDecoratorList decorators = new CommandDecoratorList();

            // Add Standard stuff
            decorators.Add(new SetUpTearDownDecorator());

            // Add Decorators supplied by attributes and parameter sets
            foreach (ICommandDecorator decorator in CustomDecorators)
                decorators.Add(decorator);

            decorators.OrderByStage();

            foreach (ICommandDecorator decorator in decorators)
            {
                command = decorator.Decorate(command);
            }

            return command;
        }
#endif

        #endregion
    }
}