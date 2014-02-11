using System.Runtime.Serialization;

namespace Nexfi.Tracker.Common.ObjectModel.Entities.ValidationConstraints
{
    [DataContract]
    public enum trkValidationConstraintType
    {
        [EnumMember]
        Null = 0,
        [EnumMember]
        NotNull = 1,
        [EnumMember]
        Greater = 2,
        [EnumMember]
        Lower = 3,
        [EnumMember]
        Equal = 4,
        [EnumMember]
        Between = 5,
        [EnumMember]
        Different = 6,
        [EnumMember]
        True = 7,
        [EnumMember]
        False = 8
    }
}