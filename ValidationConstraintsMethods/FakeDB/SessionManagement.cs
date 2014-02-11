using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexfi.Tracker.Common.ObjectModel.Entities.ValidationConstraints;
using ValidationConstraintsObjectModel.Entities;

namespace ValidationConstraintsMethods
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
            _cachedAssets.Add(new AssetEditable { Id = 124, Name = "ACCOR" , HandlesQuotes = true, AssetStatus = new AssetStatus(), AssetType = new AssetType(), Code = "AC.PA"});
            _cachedAssets.Add(new AssetEditable { Id = 295, Name = "TOTAL", HandlesQuotes = true, AssetStatus = new AssetStatus(), AssetType = new AssetType(), Code = "TO.PA" });
        }

        public ValidationConstraint GetValidationConstraint(int validationConstraintId)
        {
            return _cachedValidationConstraints.FirstOrDefault(vc => vc.Id == validationConstraintId);
        }

        public IList<ValidationConstraint> GetAllValidationConstraints()
        {
            return _cachedValidationConstraints;
        }

        public IList<AssetEditable> GetAllAssets()
        {
            return _cachedAssets;
        }


        public IList<ValidationConstraint> GetValidationConstraintsByObject(string type)
        {
            return GetAllValidationConstraints().Where(vc => type == vc.ObjectType).ToList();
        }

        public void Delete(ValidationConstraint validationConstraint)
        {
            if (_cachedValidationConstraints.Contains(validationConstraint))
                _cachedValidationConstraints.Remove(validationConstraint);
        }

        public ValidationConstraint SaveOrUpdateInstance(ValidationConstraint validationConstraint)
        {
            var oldItem = _cachedValidationConstraints.FirstOrDefault(vc => vc.Id == validationConstraint.Id);
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
