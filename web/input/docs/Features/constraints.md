Title: Constraints
Description: Lists all constraints supported by TC-Lite and the syntax elements used to invoke them in an assertion.
Order: 5
---

The _NUnit_ Constraint syntax has been re-implemented based on Generic methods to avoid boxing and provide greater type safety. Improvements are ongoing work is ongoing in this area. The following table lists constraints, which are currently supported. New constraints are added on an ongoing basis.

The "Expression Syntax" column shows how the constraint is created while parsing a constraint expression. For constraints that may begin an expression, the "Initial Syntax" column shows the syntax used to initialize the expression. Note that some constraints have been developed and tested but do not yet have syntax elements defined.

| Constraint                     | Expression Syntax    | Initial Syntax
| ------------------------------ | -------------------- | --------------
| `AllItemsConstraint`           | `All`                | `Is.All`, `Has.All`
| `AndConstraint`                | `And`
| `CollectionEquivalentConstraint` | `EquivalentTo`     | `Is.EquivalentTo`
| `EmptyCollectionConstraint`    | `Empty`              | `Is.Empty`
| `EmptyConstraint`              | `Empty`              | `Is.Empty`
| `EmptyStringConstraint`        | `Empty`              | `Is.Empty`
| `EndsWithConstraint`
| `EqualConstraint`              | `EqualTo`            | `Is.EqualTo`
|                                | `Zero`               | `Is.Zero`
| `ExactTypeConstraint`          | `TypeOf`             | `Is.TypeOf`
| `ExceptionTypeConstraint`      |                      |
| `FalseConstraint`              | `False`              | `Is.False`
| `GreaterThanConstraint`        | `GreaterThan`        | `Is.GreaterThan`
|                                | `Positive`           | `Is.Positive`
| `GreaterThanOrEqualConstraint` | `GreaterThanOrEqual` | `Is.GreaterThanOrEqual`
|                                | `AtLeast`            | `Is.AtLeast`
| `InstanceOfTypeConstraint`     | `InstanceOf`         | `Is.InstanceOf`
| `LessThanConstraint`           | `LessThan`           | `Is.LessThan`
|                                | `Negative`           | `Is.Negative`
| `LessThanOrEqualConstraint`    | `LessThanOrEqual`    | `Is.LessThanOrEqual`
|                                | `AtMost`             | `Is.AtMost`
| `NoItemConstraint`             | `None`               | `Has.None`, `Has.No`
| `NotConstraint`                | `Not`                | `Is.Not`
| `NullConstraint`               | `Null`               | `Is.Null`
| `OrConstraint`                 | `Or`
| `PropertyConstraint`
| `PropertyExistsConstraint`
| `RegexConstraint`
| `SameAsConstraint`             | `SameAs`             | `Is.SameAs`
| `SomeItemsConstraint`          | `Some`               | `Has.Some`, `Contains.Item`
| `StartsWithConstraint`
| `SubstringConstraint`          | `Substring`          | `Contains.Substring`
| `ThrowsConstraint`             |                      | `Throws.TypeOf`
| `ThrowsExceptionConstraint`    |                      | `Throws.Exception`
| `ThrowsNothingConstraint`      |                      | `Throws.Nothing`
| `TrueConstraint`               | `True`               | `Is.True`
| `UniqueItemsConstraint`        | `Unique`             | `Is.Unique`
