using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ValidationConstraintsMethods.FakeDB;
using ValidationConstraintsObjectModel.Constants;
using ValidationConstraintsObjectModel.Contracts;
using ValidationConstraintsObjectModel.Entities;

namespace ValidationConstraintsMethods.WCFService
{
    public class ValidationConstraintService 
    {
        private static ValidationConstraintService _instance;

        /// <summary>
        /// Returns the unique instance of the <see cref="ValidationConstraintService"/> class.
        /// </summary>
        /// <returns>The unique instance of the <see cref="ValidationConstraintService"/> class.</returns>
        public static ValidationConstraintService Instance
        {
            get { return _instance ?? (_instance = new ValidationConstraintService()); }
        }


        public ValidationConstraint GetValidationConstraintById(string validationConstraintId)
        {
            try
            {
                return SessionManagement.Db.GetValidationConstraint(validationConstraintId);
            }
            catch (Exception ex)
            {
                throw new Exception();
                //throw new FunctionalException(MessageDictionary.COMMON_ERR_LOADALL, ex);
            }
        }

        public IList<ValidationConstraint> GetAllValidationConstraints()
        {
            try
            {
                return SessionManagement.Db.GetAllValidationConstraints();
            }
            catch (Exception ex)
            {
                throw new Exception();
                //throw new FunctionalException(MessageDictionary.COMMON_ERR_LOADALL, ex);
            }
        }

        public IList<ValidationConstraint> GetAllValidationConstraintsByObject(string type)
        {
            try
            {
                return SessionManagement.Db.GetValidationConstraintsByObject(type);
            }
            catch (Exception ex)
            {
                throw new Exception();
                //throw new FunctionalException(MessageDictionary.COMMON_ERR_LOADALL, ex);
            }
        }

        public IList<ValidationConstraint> SaveOrUpdateList(IList<ValidationConstraint> constraints)
        {
            try
            {
                if (constraints == null)
                    throw new Exception();
                    //throw new ArgumentNullException("ValidationConstraint", MessageDictionary.COMMON_ERR_INVALIDOBJSAVE);

                //ITransactionStatus saveOrUpdateTransaction = SessionManagement.Db.StartTransaction();
                IList<ValidationConstraint> savedConstraints = new List<ValidationConstraint>();
                try
                {
                    foreach (ValidationConstraint validationConstraint in constraints)
                    {
                        if (validationConstraint.RequiresDeletion && !validationConstraint.IsNew)
                        {
                            DeleteObject(validationConstraint);
                        }
                        else
                        {
                            string message;
                            if (CanBeSaved(validationConstraint, out message))
                            {
                                var savedConstraint = SessionManagement.Db.SaveOrUpdateInstance(validationConstraint); //Initially a call to the server generic base type
                                if (savedConstraint != null) savedConstraints.Add(savedConstraint);    
                            }
                            else
                            {
                                throw new Exception(message);
                            }
                        }
                    }
                    //SessionManagement.Db.CommitTrans(saveOrUpdateTransaction);
                    return savedConstraints;
                }
                catch (Exception ex)
                {
                    throw;
                    //SessionManagement.Db.RollbackTrans((saveOrUpdateTransaction));
                    //throw new TechnicalException(ExceptionDictionary.The_constraint_could_not_be_saved_into_the_database, ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
                //throw new FunctionalException(MessageDictionary.COMMON_ERR_SAVE, ex);
            }
        }

        protected /*override*/ bool CanBeSaved(ValidationConstraint constraint, out string message)
        {

            StringBuilder sb = new StringBuilder();
            if (constraint.ParentConstraint != null && Equals(constraint, constraint.ParentConstraint))
                sb.AppendLine("A constraint cannot reference itself as parent");
            var test = Type.GetType(constraint.PropertyType);
            if (test == null)
                sb.AppendLine("The Object Type associated to the constraint could not be found");
            if (test != null && test.IsSubclassOf(typeof(ObjectBase)))
                switch (constraint.ConstraintType)
                {
                    case trkValidationConstraintType.False:
                    case trkValidationConstraintType.True:
                    case trkValidationConstraintType.Greater:
                    case trkValidationConstraintType.Lower:
                    case trkValidationConstraintType.Between:
                    case trkValidationConstraintType.Equal:
                        sb.AppendLine(string.Format("The constraint type specified cannot be applied to the selected parameter {0}", constraint));
                        break;
                }
            if (string.IsNullOrEmpty(sb.ToString()) && constraint.MainArgument != null || constraint.SecondaryArgument != null)
            {
                switch (constraint.ConstraintType)
                {
                    case trkValidationConstraintType.Null:
                    case trkValidationConstraintType.NotNull:
                        sb.AppendLine(string.Format(string.Format("The arguments you specified are incompatible with the constraint type {0}"),constraint));
                        break;
                    case trkValidationConstraintType.Between:
                        if (!(constraint.MainArgument != null && constraint.SecondaryArgument != null))
                            sb.AppendLine("Both arguments must be specified");
                        break;
                }
                try
                {
                    Object mainArgument = null;
                    Object secondArgument = null;
                    if (constraint.MainArgument != null)
                    {
                        MethodInfo m = test.GetMethod("Parse", new[] { typeof(string) });
                        if (m != null)
                        {
                            mainArgument = m.Invoke(null, new[] { constraint.MainArgument });
                            if (mainArgument == null) throw new Exception();
                        }
                    }
                    if (constraint.SecondaryArgument != null && constraint.ConstraintType == trkValidationConstraintType.Between)
                    {
                        MethodInfo m = test.GetMethod("Parse", new[] { typeof(string) });
                        if (m != null)
                        {
                            secondArgument = m.Invoke(null, new[] { constraint.SecondaryArgument });
                            if (secondArgument == null) throw new Exception();
                        }
                        // MainArgument < SecondArgument when it is a between constraint.
                        if (ValidationConstraintHelper.IsGreater(secondArgument, mainArgument))
                            sb.AppendLine("The second argument must be greater than the first");
                    }
                    else if (constraint.SecondaryArgument != null)
                        sb.AppendLine("The second argument is only used in the Between constraint and will therefore be ignored");
                }
                catch (Exception)
                {
                    throw new Exception();
                    //throw new FunctionalException(string.Format(ExceptionDictionary.The_value_of_the_argument_could_not_be_correctly_translated_into_the_specified_type, constraint));
                }
            }

            if ((constraint.ConstraintType == trkValidationConstraintType.True || constraint.ConstraintType == trkValidationConstraintType.False) && constraint.PropertyType != typeof(bool).AssemblyQualifiedName)
            {
                sb.AppendLine(string.Format("{0} is not a boolean and_therefore not compatible with_a {1} constraint", constraint.Property, constraint.ConstraintType));
                //sb.AppendLine(string.Format(ExceptionDictionary._0__is_not_a_boolean_and_therefore_not_compatible_with_a__1__constraint, constraint.Property, constraint.ConstraintType));
            }
            message = sb.ToString();
            return string.IsNullOrEmpty(message);
        }

        protected /*override*/ ValidationConstraint SaveOrUpdateObject(ValidationConstraint validationConstraint)
        {
            //return validationConstraint.IsNew ? SessionManagement.Db.SaveNew(validationConstraint) : SessionManagement.Db.Update(validationConstraint);
            return SessionManagement.Db.SaveOrUpdateInstance(validationConstraint);
        }

        protected /*override*/ bool CanBeDeleted(ValidationConstraint obj, out string message)
        {
            message = string.Empty;
            StringBuilder sb = new StringBuilder();
            var children = SessionManagement.Db.GetAllValidationConstraints().Where(c => Equals(c.ParentConstraint, obj)).ToList();
            if (children.Any())
                sb.AppendLine(string.Format("The constraint {0} cannot be deleted because it is linked to child constraints", obj));
            message = sb.ToString();
            return string.IsNullOrEmpty(message);
        }

        protected /*override*/ bool CanBeDeleted(IEnumerable<ValidationConstraint> objects, out string message) //Not implemented
        {
            message = string.Empty;
            return true;
        }

        protected /*override*/ void DeleteObject(ValidationConstraint obj)
        {
            string message;
            if (CanBeDeleted(obj, out message))
                SessionManagement.Db.Delete(obj);
            else
                throw new Exception(message);
        }

        public void ValidateObject(IRepositoryWorkflowEntity record)
        {
            string message = null;
            if (record != null)
            {
                ValidationConstraintHelper.ProcessConstraints(record, GetAllValidationConstraintsByObject(record.GetType().AssemblyQualifiedName), out message);
            }
            if (!string.IsNullOrEmpty(message)) throw new Exception(message);
        }
    }
}
