using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationConstraintsObjectModel;
using ValidationConstraintsObjectModel.Entities;
using ValidationConstraintsMethods;
using Nexfi.Tracker.Common.ObjectModel.Entities.ValidationConstraints;
using Nexfi.Tracker.Server.Services.Core;
namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var asset = new AssetEditable();
            var enumerator = new ExpressionEnumerator(typeof(AssetEditable)).ToList();
            Console.WriteLine(string.Format("Type of {0} : ", typeof(AssetEditable).AssemblyQualifiedName));
            foreach (KeyValuePair<string,string> k in enumerator)
            {
                Console.WriteLine(string.Format("Property name : {0}", k.Key));
                Console.WriteLine();

            }
            //Set the asset's properties to default values
            asset.Name = "test";
            asset.Code = "t.PA";
            var nameConstraint = new ValidationConstraint { ObjectType = typeof(AssetEditable).AssemblyQualifiedName, Property = "Name", PropertyType = typeof(string).AssemblyQualifiedName, ConstraintType = trkValidationConstraintType.NotNull };
            nameConstraint = SessionManagement.Db.SaveOrUpdateInstance(nameConstraint);
            Console.WriteLine(string.Format("{0} was saved succesfully"));
            ValidationConstraintService.Instance.ValidateObject(asset);
            nameConstraint = new ValidationConstraint { ObjectType = typeof(AssetEditable).AssemblyQualifiedName, Property = "Code", PropertyType = typeof(string).AssemblyQualifiedName, ConstraintType = trkValidationConstraintType.Null };
            nameConstraint = SessionManagement.Db.SaveOrUpdateInstance(nameConstraint);
            ValidationConstraintService.Instance.ValidateObject(asset);
            Console.ReadLine();
        }
    }
}
