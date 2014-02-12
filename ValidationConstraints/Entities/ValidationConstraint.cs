using System;
using System.Collections.Generic;
using ValidationConstraintsObjectModel.Constants;

namespace ValidationConstraintsObjectModel.Entities
{
    [Serializable]
    public class ValidationConstraint : ObjectBase
    {
        public ValidationConstraint ParentConstraint { get; set; }
        public trkValidationConstraintType ConstraintType { get; set; }
        public object MainArgument { get; set; }
        public object SecondaryArgument { get; set; }
        public string ObjectType { get; set; }
        public string Property { get; set; }
        /// <summary>
        /// Set using the AssemblyQualifiedName of the type.
        /// </summary>
        public string PropertyType { get; set; }

        public KeyValuePair<string, string> PropertyKey
        {
            get { return new KeyValuePair<string, string>(Property, PropertyType); }
            set
            {
                Property = value.Key;
                PropertyType = value.Value;
            }
        }

        public override string ToString()
        {
            if (MainArgument != null && SecondaryArgument != null && ConstraintType == trkValidationConstraintType.Between)
            {
                return string.Format("{0} {1} {2} & {3}", Property, ConstraintType, MainArgument, SecondaryArgument);
            }
            if (MainArgument != null)
            {
                return string.Format("{0} {1} {2}", Property, ConstraintType, MainArgument);
            }
            return string.Format("{0} {1}", Property, ConstraintType);
        }
    }
}
