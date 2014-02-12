using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationConstraintsMethods;
using ValidationConstraintsMethods.FakeDB;
using ValidationConstraintsMethods.Parsers;
using ValidationConstraintsMethods.WCFService;
using ValidationConstraintsObjectModel.Constants;
using ValidationConstraintsObjectModel.Entities;

namespace Test
{
    [TestClass]
    public class ValidationConstraintServiceUnitTests
    {
        private static string _typeOfString;
        private static string _typeOfInt;
        private static string _typeOfAssetEditable;
        private static string _typeOfAssetStatus;
        private static string _typeOfBool;
        private static string _typeOfAssetType;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _typeOfString = typeof(string).AssemblyQualifiedName;
            _typeOfInt = typeof(int).AssemblyQualifiedName;
            _typeOfAssetEditable = new AssetEditable().GetType().AssemblyQualifiedName;
            _typeOfAssetStatus = new AssetStatus().GetType().AssemblyQualifiedName;
            _typeOfBool = typeof(bool).AssemblyQualifiedName;
            _typeOfAssetType = new AssetType().GetType().AssemblyQualifiedName;
        }

        [TestMethod]
        public void ValidationConstraints_GetAllValidationConstraints()
        {
            var result = ValidationConstraintService.Instance.GetAllValidationConstraints();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidationConstraints_GetValidationConstraintById()
        {
            var result = SessionManagement.Db.GetAllValidationConstraints().FirstOrDefault();
            if (result == null) Console.WriteLine("No validation constraints found in the database");
            else
            {
                var secondRun = ValidationConstraintService.Instance.GetValidationConstraintById(result.ToString());
                Assert.IsNotNull(result);
                Assert.IsNotNull(secondRun);
                Assert.AreEqual(result, secondRun);
            }
        }

        [TestMethod]
        public void ValidationConstraints_GetAllValidationConstraintsByObject()
        {
            var result = SessionManagement.Db.GetAllValidationConstraints().FirstOrDefault();
            if (result == null) Console.WriteLine("No validation constraints found in the database");
            else
            {
                var entireList = ValidationConstraintService.Instance.GetAllValidationConstraintsByObject(result.ObjectType);
                Assert.IsNotNull(result);
                Assert.IsNotNull(entireList);
                Assert.IsTrue(entireList.Contains(result));
            }
        }

        [TestMethod]
        public void ValidationConstraints_Save()
        {
            var newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = _typeOfAssetEditable,
                Property = "Code",
                PropertyType = _typeOfString
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Null,
                ObjectType = _typeOfAssetEditable,
                Property = "Code",
                PropertyType = _typeOfString
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Null,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Equal,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1"
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Different,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1"
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Greater,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1"
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Lower,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1"
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.True,
                ObjectType = _typeOfAssetEditable,
                Property = "HandlingQuotes",
                PropertyType = _typeOfBool,
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.False,
                ObjectType = _typeOfAssetEditable,
                Property = "HandlingQuotes",
                PropertyType = _typeOfBool,
            };
            TrySaveAndSucceed(newConstraint);
        }

        [TestMethod]
        public void ValidationConstraints_SaveFailed()
        {
            var newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.False,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.True,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Greater,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Lower,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Between,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Equal,
                ObjectType = _typeOfAssetEditable,
                Property = "Status",
                PropertyType = _typeOfAssetStatus
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.True,
                ObjectType = _typeOfAssetEditable,
                Property = "Code",
                PropertyType = _typeOfString
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.False,
                ObjectType = _typeOfAssetEditable,
                Property = "Code",
                PropertyType = _typeOfString
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.False,
                ObjectType = _typeOfAssetEditable,
                Property = "UnknownProperty",
                PropertyType = _typeOfString
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Between,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1"
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Between,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                SecondaryArgument = "1"
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                SecondaryArgument = "1"
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Null,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                SecondaryArgument = "1"
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = "RandomTypeThatDoesNotExist",
            };
            TrySaveAndFail(newConstraint);
        }

        [TestMethod]
        public void ValidationConstraints_Update()
        {
            var constraintsToSave = new List<ValidationConstraint>();
            var newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.False,
                ObjectType = _typeOfAssetEditable,
                Property = "HandlingQuotes",
                PropertyType = _typeOfBool,
            };
            constraintsToSave.Add(newConstraint);
            var savedItem = ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave).Single();
            constraintsToSave.Clear();
            savedItem.ConstraintType = trkValidationConstraintType.True;
            constraintsToSave.Add(savedItem);
            savedItem = ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave).Single();
            constraintsToSave.Clear();
            Assert.AreEqual(savedItem.ConstraintType, trkValidationConstraintType.True);
            savedItem.RequiresDeletion = true;
            constraintsToSave.Add(newConstraint);
            ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave);
            var supposedToHaveBeenErased = ValidationConstraintService.Instance.GetValidationConstraintById(savedItem.ToString());
            if (supposedToHaveBeenErased != null)
            {
                Assert.Fail(supposedToHaveBeenErased.ToString());
            }
        }


        [TestMethod]
        public void ValidationConstraints_SaveItselfAsParentFails()
        {
            //Check that a validation constraint cannot reference itself 
            var newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
            };

            var constraintsToSave = new List<ValidationConstraint> { newConstraint };
            var savedConstraint = ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave).SingleOrDefault();
            Assert.IsNotNull(savedConstraint);
            constraintsToSave.Clear();
            savedConstraint.ParentConstraint = savedConstraint;
            TrySaveAndFail(newConstraint);

            ////Finally delete the constraint that had to be saved.
            //savedConstraint.RequiresDeletion = true;
            //constraintsToSave.Add(newConstraint);
            //ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave);
            //Assert.IsNull(ValidationConstraintService.Instance.GetValidationConstraintById(savedConstraint.ToString()));
            //constraintsToSave.Clear();
        }

        [TestMethod]
        public void ValidationConstraints_DeleteParentFails()
        {
            //var transaction = SessionManagement.Db.StartTransaction();
            try
            {
                //Check that a validation constraint cannot reference itself 
                var parentConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.NotNull,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Id",
                    PropertyType = _typeOfInt,
                };

                var childConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.NotNull,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Code",
                    PropertyType = _typeOfString,
                };

                var constraintsToSave = new List<ValidationConstraint> { parentConstraint, childConstraint };
                var savedConstraints = ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave);
                Assert.IsNotNull(savedConstraints);
                savedConstraints[1].ParentConstraint = savedConstraints[0];
                savedConstraints = ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave);
                savedConstraints[0].RequiresDeletion = true;
                TrySaveAndFail(savedConstraints);
            }
            finally
            {
                //SessionManagement.Db.RollbackTrans(transaction);
            }
        }

        [TestMethod]
        public void ValidationConstraints_CheckThatArgumentsProperlyParsed()
        {
            var newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Equal,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1"
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Equal,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "NotAnInt"
            };
            TrySaveAndFail(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Between,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1",
                SecondaryArgument = "2"
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Between,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1",
                SecondaryArgument = "NotAnInt"
            };
            TrySaveAndFail(newConstraint);
        }

        [TestMethod]
        public void ValidationConstraints_SecondGreaterThanMainArgument()
        {
            var newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Between,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1",
                SecondaryArgument = "2"
            };
            TrySaveAndSucceed(newConstraint);

            newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Between,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                MainArgument = "1",
                SecondaryArgument = "0"
            };
            TrySaveAndFail(newConstraint);
        }

        [TestMethod]
        public void ValidationConstraints_Validate()
        {
            const string errorMessage = "The object should not have been saved given its constraints";
            //var transac = SessionManagement.Db.StartTransaction();
            IList<ValidationConstraint> testConstraints = new List<ValidationConstraint>();
            try
            {
                var accor = SessionManagement.Db.GetAllAssets().SingleOrDefault(a => a.Name == "ACCOR");
                var total = SessionManagement.Db.GetAllAssets().SingleOrDefault(a => a.Name == "TOTAL");
                Assert.IsNotNull(total);
                var nameConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.Equal,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Name",
                    PropertyType = _typeOfString,
                    MainArgument = "ACCOR"
                };
                testConstraints.Add(nameConstraint);
                var savedNameConstraint = ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints).SingleOrDefault();
                //Should succeed
                ValidationConstraintService.Instance.ValidateObject(accor);
                //Should fail
                try
                {
                    ValidationConstraintService.Instance.ValidateObject(total);
                    Assert.Fail(errorMessage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


                //Second Level
                var idConstraint1 = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.Greater,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Id",
                    PropertyType = _typeOfString,
                    MainArgument = "0",
                    ParentConstraint = savedNameConstraint
                };
                testConstraints.Add(idConstraint1);
                ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints);
                //Should succeed
                ValidationConstraintService.Instance.ValidateObject(accor);

                //Should Fail (Id = 122)
                var idConstraint2 = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.Lower,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Id",
                    PropertyType = _typeOfString,
                    MainArgument = "30",
                    ParentConstraint = savedNameConstraint
                };
                testConstraints.Add(idConstraint2);
                testConstraints = ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints);
                try
                {
                    ValidationConstraintService.Instance.ValidateObject(accor);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                //Remove it now that it has been tested
                testConstraints.ToList().ForEach(c =>
                {
                    if (c == idConstraint2)
                        c.RequiresDeletion = true;
                });

                var handlingQuotesConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.True,
                    ObjectType = _typeOfAssetEditable,
                    Property = "HandlingQuotes",
                    PropertyType = _typeOfBool,
                    ParentConstraint = savedNameConstraint
                };
                testConstraints.Add(handlingQuotesConstraint);
                var thirdLevel = ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints).SingleOrDefault(c => c.ToString() == handlingQuotesConstraint.ToString());
                //Should succeed
                ValidationConstraintService.Instance.ValidateObject(accor);


                var betweenConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.Between,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Id",
                    PropertyType = _typeOfInt,
                    MainArgument = "0",
                    SecondaryArgument = "100000000",
                    ParentConstraint = thirdLevel
                };
                testConstraints.Add(betweenConstraint);
                ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints);
                //Should succeed
                ValidationConstraintService.Instance.ValidateObject(accor);

                var notNullConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.NotNull,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Status",
                    PropertyType = _typeOfAssetStatus,
                    ParentConstraint = thirdLevel
                };
                testConstraints.Add(notNullConstraint);
                ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints);

                //Should succeed
                ValidationConstraintService.Instance.ValidateObject(accor);


                var differentConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.Different,
                    ObjectType = _typeOfAssetEditable,
                    Property = "Id",
                    PropertyType = _typeOfInt,
                    MainArgument = "1",
                    ParentConstraint = thirdLevel
                };

                testConstraints.Add(differentConstraint);
                ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints);
                //Should succeed
                ValidationConstraintService.Instance.ValidateObject(accor);
                 

                var nullConstraint = new ValidationConstraint
                {
                    ConstraintType = trkValidationConstraintType.Null,
                    ObjectType = _typeOfAssetEditable,
                    Property = "AssetType",
                    PropertyType = _typeOfAssetType,
                    ParentConstraint = thirdLevel
                };
                testConstraints.Add(nullConstraint);
                ValidationConstraintService.Instance.SaveOrUpdateList(testConstraints);
                try
                {
                    //Should fail.
                    ValidationConstraintService.Instance.ValidateObject(accor);
                    Assert.Fail(errorMessage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                //If you want to add constraint remove the top one from the list.
            }
            finally
            {
                //SessionManagement.Db.RollbackTrans(transac);
            }

        }

        [TestMethod]
        public void ValidationConstraints_ValidateDeepTree()
        {
            var asset = SessionManagement.Db.GetAllAssets().FirstOrDefault(a => a.Name == "ACCOR");
            var firstConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Equal,
                ObjectType = _typeOfAssetEditable,
                Property = "Name",
                PropertyType = _typeOfString,
                MainArgument = "ACCOR"
            };

            var secondConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.Equal,
                ObjectType = _typeOfAssetEditable,
                Property = "Id",
                PropertyType = _typeOfInt,
                ParentConstraint = firstConstraint,
                MainArgument = "124"
            };

            var thirdConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = _typeOfAssetEditable,
                Property = "Code",
                ParentConstraint = secondConstraint,
                PropertyType = _typeOfString,
            };
            ValidationConstraintService.Instance.SaveOrUpdateList(new List<ValidationConstraint>{firstConstraint, secondConstraint, thirdConstraint});
            ValidationConstraintService.Instance.ValidateObject(asset);
        }
        [TestMethod]
        public void ValidationConstraints_SaveSomeConstraints()
        {
            var portfolioType = new PortfolioEditable().GetType().AssemblyQualifiedName;
            var userInfoType = new UserInfo().GetType().AssemblyQualifiedName;

            var newConstraint = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = portfolioType,
                Property = "Comment",
                PropertyType = portfolioType
            };
            var newConstraint2 = new ValidationConstraint
            {
                ConstraintType = trkValidationConstraintType.NotNull,
                ObjectType = portfolioType,
                Property = "Manager",
                PropertyType = userInfoType
            };

            var constraintsToSave = new List<ValidationConstraint> { newConstraint, newConstraint2 };
            ValidationConstraintService.Instance.SaveOrUpdateList(constraintsToSave);
        }



        [TestMethod]
        public void ValidationConstraints_BasicCheckEnumerator()
        {
            var items = new ExpressionEnumerator(new AssetEditable().GetType()).ToList();
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());
            var goesToSecondLevelAtLeast = false;
            items.ForEach(i =>
            {
                if (i.Key.Contains("."))
                {
                    goesToSecondLevelAtLeast = true;
                    return;
                }
            });
            if (!goesToSecondLevelAtLeast) Assert.Fail();
        }
        private static void TrySaveAndSucceed(ValidationConstraint constraintsToSave)
        {
            var constraintsList = new List<ValidationConstraint> { constraintsToSave };
            TrySaveAndSucceed(constraintsList);
        }

        private static void TrySaveAndSucceed(IList<ValidationConstraint> constraintsList)
        {
            //var transac = SessionManagement.Db.StartTransaction();
            try
            {
                ValidationConstraintService.Instance.SaveOrUpdateList(constraintsList);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            finally
            {
                //SessionManagement.Db.RollbackTrans(transac);
            }
        }

        private static void TrySaveAndFail(ValidationConstraint constraintsToSave)
        {
            var constraintsList = new List<ValidationConstraint> { constraintsToSave };
            TrySaveAndFail(constraintsList);
        }

        private static void TrySaveAndFail(IList<ValidationConstraint> constraintsList)
        {
            //var transac = SessionManagement.Db.StartTransaction();
            bool hasFailed = false;
            try
            {
                ValidationConstraintService.Instance.SaveOrUpdateList(constraintsList);
            }
            catch (Exception e)
            {
                hasFailed = true;
            }
            finally
            {
                Assert.IsTrue(hasFailed);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            SessionManagement.Db.ClearAllConstraints();
        }
    }
}
