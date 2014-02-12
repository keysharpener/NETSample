using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ValidationConstraintsMethods.Parsers;
using ValidationConstraintsObjectModel.Constants;
using ValidationConstraintsObjectModel.Entities;

namespace ValidationConstraintsMethods
{
    public static class ValidationConstraintHelper
    {
        public static bool ProcessConstraints(object item, IList<ValidationConstraint> constraints, out string finalMessage)
        {
            StringBuilder sb = new StringBuilder();
            var absoluteParents = constraints.Where(c => c.ParentConstraint == null).ToList();
            foreach (var validationConstraint in absoluteParents)
            {
                string message = null;
                if (constraints.Any(c => c.ParentConstraint == validationConstraint))
                    RecursiveProcess(item, constraints, validationConstraint, out message);
                else
                {
                    var respected = ProcessConstraint(item, validationConstraint);
                    if (!respected) message = string.Format("{0} is not respected", validationConstraint);
                }
                if (!string.IsNullOrEmpty(message)) sb.AppendLine(message);
            }
            finalMessage = sb.ToString();
            return string.IsNullOrEmpty(finalMessage);
        }

        private static void RecursiveProcess(object item, IList<ValidationConstraint> constraints, ValidationConstraint parentConstraint, out string message)
        {
            StringBuilder sb = new StringBuilder();
            message = null;
            var parentConstraintIsRespected = ProcessConstraint(item, parentConstraint);
            if (!parentConstraintIsRespected) return;
            foreach (var source in constraints.Where(c => Equals(c.ParentConstraint, parentConstraint)))
            {
                var constraintIsRespected = ProcessConstraint(item, source);
                var constraintHasChildren = constraints.Any(c => Equals(c.ParentConstraint, source));
                if (constraintIsRespected && constraintHasChildren)
                {
                    //Go down the tree
                    string subMessage = null;
                    RecursiveProcess(item, constraints, source, out subMessage);
                    if (!string.IsNullOrEmpty(subMessage)) sb.AppendLine(subMessage);
                }
                else if (!constraintIsRespected && !constraintHasChildren)
                {
                    //if all parents are true and the final child is false, the constraint is not respected.
                    sb.AppendLine(string.Format("{0} is not respected", source));
                }
            }
            message = sb.ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private static bool ProcessConstraint(object item, ValidationConstraint constraint)
        {
            var argumentType = item.GetType();
            object mainArgument = null;
            object secondArgument = null;
            if (constraint.ObjectType == argumentType.AssemblyQualifiedName)
            {
                var evaluator = new ExpressionEvaluator();
                var propertyValue = evaluator.GetValue(item, constraint.Property);
                if (constraint.MainArgument != null) mainArgument = GetValue(constraint.MainArgument, propertyValue.GetType());
                if (constraint.SecondaryArgument != null) secondArgument = GetValue(constraint.SecondaryArgument, propertyValue.GetType());
                switch (constraint.ConstraintType)
                {
                    case trkValidationConstraintType.Null:
                        if (propertyValue is string)
                            return string.IsNullOrEmpty(propertyValue.ToString());
                        return (propertyValue == null);
                    case trkValidationConstraintType.NotNull:
                        if (propertyValue is string)
                            return !string.IsNullOrEmpty((string)propertyValue.ToString());
                        return (propertyValue != null);
                    case trkValidationConstraintType.Greater:
                        if (mainArgument == null) return false;
                        return IsGreater(mainArgument, propertyValue);
                    case trkValidationConstraintType.Lower:
                        if (mainArgument == null) return false;
                        return IsGreater(propertyValue, mainArgument);
                    case trkValidationConstraintType.Equal:
                        if (mainArgument == null) return false;
                        return AreEqual(propertyValue, mainArgument);
                    case trkValidationConstraintType.Between:
                        if (mainArgument == null) return false;
                        if (secondArgument == null) return false;
                        return (IsGreater(mainArgument, propertyValue) && IsGreater(propertyValue, secondArgument));
                    case trkValidationConstraintType.Different:
                        return !AreEqual(propertyValue, mainArgument);
                    case trkValidationConstraintType.False:
                        return !((bool)propertyValue);
                    case trkValidationConstraintType.True:
                        return ((bool)propertyValue);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return false;
        }

        private static object GetValue(object getValue, Type typeToConvertTo)
        {
            object mainArgument = null;
            if (typeToConvertTo == typeof(string)) return getValue.ToString();
            if (getValue != null)
            {
                MethodInfo m = typeToConvertTo.GetMethod("Parse", new Type[] { typeof(string) });
                if (m != null)
                {
                    mainArgument = m.Invoke(null, new[] { getValue });
                    if (mainArgument == null) throw new Exception();
                }
            }
            return mainArgument;
        }

        private static bool AreEqual(object propertyValue, object mainArgument)
        {
            return propertyValue.Equals(mainArgument);
        }

        public static bool IsGreater(dynamic smallerArgument, dynamic biggerArgument)
        {
            if (biggerArgument.GetType() != smallerArgument.GetType() || smallerArgument as IComparable == null) return false;
            return smallerArgument < biggerArgument;
        }
    }
}