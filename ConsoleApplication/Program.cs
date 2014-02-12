using System;
using System.Collections.Generic;
using System.Linq;
using ValidationConstraintsMethods.FakeDB;
using ValidationConstraintsMethods.Parsers;
using ValidationConstraintsMethods.WCFService;
using ValidationConstraintsObjectModel.Constants;
using ValidationConstraintsObjectModel.Entities;

namespace ConsoleApplication
{
    static class Program
    {
        static void Main(string[] args)
        {
            AssetEditable asset = GenerateSampleAsset();

            var enumerator = new ExpressionEnumerator(typeof(AssetEditable)).ToList();
            var evaluator = new ExpressionEvaluator();

            Console.WriteLine("Type of {0} : ", typeof(AssetEditable).AssemblyQualifiedName);
            foreach (KeyValuePair<string,string> k in enumerator)
            {
                Console.WriteLine("Property name : {0}", k.Key);
                Console.WriteLine("Value : {0}", evaluator.GetValue(asset, k.Key));
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private static AssetEditable GenerateSampleAsset()
        {
            var sampleStatus = new AssetStatus { Code = "TRAD", Id = 2356, Name = "Tradeable", RequiresDeletion = false };
            var sampleType = new AssetType {Code = "B", Name = "Bond"};
            return new AssetEditable { Id = 1, Status = sampleStatus, AssetType = sampleType, HandlingQuotes = true, Name = "Octo Asset", Code = "OCT"};
        }
    }
}
