// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Globalization;
using TCLite.Constraints;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
    public class ApplyToContextTests
    {
        private TestExecutionContext _context = new TestExecutionContext();

        [TestCase]
        public void DefaultFloatingPointToleranceAttribute()
        {
            var attr = new DefaultFloatingPointToleranceAttribute(0.5);
            attr.ApplyToContext(_context);
            var tolerance = _context.DefaultFloatingPointTolerance;
            Assert.That(tolerance.Mode, Is.EqualTo(ToleranceMode.Linear));
            Assert.That(tolerance.Amount, Is.EqualTo(0.5));
        }

#if NYI
        [TestCase]
        public void SingleThreadedAttribute()
        {
            var attr = new SingleThreadedAttribute();
            attr.ApplyToContext(_context);
            Assert.True(_context.IsSingleThreaded);
        }

        [TestCase]
        public void SetCultureAttribute()
        {
            var attr = new SetCultureAttribute("fr-FR") as IApplyToContext;
            attr.ApplyToContext(_context);
            Assert.That(_context.CurrentCulture, Is.EqualTo(new CultureInfo("fr-FR")));
        }

        [TestCase]
        public void SetUICultureAttribute()
        {
            var attr = new SetUICultureAttribute("fr-FR") as IApplyToContext;
            attr.ApplyToContext(_context);
            Assert.That(_context.CurrentUICulture, Is.EqualTo(new CultureInfo("fr-FR")));
        }
#endif        

#if THREAD_ABORT
        [TestCase]
        public void TimeoutAttribute()
        {
            var attr = new TimeoutAttribute(50) as IApplyToContext;
            attr.ApplyToContext(_context);
            Assert.That(_context.TestCaseTimeout, Is.EqualTo(50));
        }
#endif
    }
}
