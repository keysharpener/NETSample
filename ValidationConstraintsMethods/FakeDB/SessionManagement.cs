using System.Collections.Generic;
using System.Linq;
using ValidationConstraintsObjectModel.Entities;

namespace ValidationConstraintsMethods.FakeDB
{
    public class SessionManagement
    {
        private static IList<ValidationConstraint> _cachedValidationConstraints = new List<ValidationConstraint>();
        private static IList<AssetEditable> _cachedAssets = new List<AssetEditable>();
        private static SessionManagement _db;
        public static SessionManagement Db
        {
            get
            {
                if (_db == null)
                    _db = new SessionManagement();
                return _db;
            }
        }

        private SessionManagement()
        {
            _cachedAssets.Add(new AssetEditable { Id = 124, Name = "ACCOR", HandlingQuotes = true, Status = new AssetStatus(), AssetType = new AssetType(), Code = "AC.PA" });
            _cachedAssets.Add(new AssetEditable { Id = 295, Name = "TOTAL", HandlingQuotes = true, Status = new AssetStatus(), AssetType = new AssetType(), Code = "TO.PA" });
        }

        public ValidationConstraint GetValidationConstraint(string validationConstraintId)
        {
            return _cachedValidationConstraints.FirstOrDefault(vc => vc.ToString() == validationConstraintId);
        }

        public IList<ValidationConstraint> GetAllValidationConstraints()
        {
            return _cachedValidationConstraints;
        }

        public IEnumerable<AssetEditable> GetAllAssets()
        {
            return _cachedAssets;
        }

        public void ClearAllConstraints()
        {
            _cachedValidationConstraints.Clear();
        }

        public IList<ValidationConstraint> GetValidationConstraintsByObject(string type)
        {
            return GetAllValidationConstraints().Where(vc => type == vc.ObjectType).ToList();
        }

        public void Delete(ValidationConstraint validationConstraint)
        {
            if (_cachedValidationConstraints.FirstOrDefault(vc => vc.Equals(validationConstraint)) != null)
                _cachedValidationConstraints.Remove(validationConstraint);

        }

        public ValidationConstraint SaveOrUpdateInstance(ValidationConstraint validationConstraint)
        {
            var oldItem = _cachedValidationConstraints.FirstOrDefault(vc => vc.Equals(validationConstraint));
            if (oldItem == null)
                _cachedValidationConstraints.Add(validationConstraint);
            else
            {
                _cachedValidationConstraints.Remove(oldItem);
                _cachedValidationConstraints.Add(validationConstraint);
            }
            return validationConstraint;
        }
    }
}
