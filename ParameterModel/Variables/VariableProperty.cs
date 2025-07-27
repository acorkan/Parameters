using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParameterModel.Variables
{
    /// <summary>
    /// The VariableProperty is used to hold the name of a variable, which must exists in the active IVariablesContext. 
    /// Which variable types can be assigned to this, and wether the allowed variable instances can be read-only or not is
    /// set using the VariableAssignmentAttribute on the property.
    /// </summary>
    public class VariableProperty
    {
        public string Assignment { get; set; } = string.Empty;

        public override string ToString()
        {
            return (Assignment != null) ? Assignment : "";
        }

        public VariableProperty(string assignment)
        {
            Assignment = assignment;
        }

        // Override Equals()
        public override bool Equals(object obj)
        {
            // Null check
            if (obj == null || GetType() != obj.GetType())
            { 
                return false;
            }

            VariableProperty other = (VariableProperty)obj;
            return Assignment == other.Assignment;
        }

        // Override GetHashCode()
        public override int GetHashCode()
        {
            return HashCode.Combine(Assignment);
        }

        // Overload == operator
        public static bool operator ==(VariableProperty obj1, VariableProperty obj2)
        {
            // Check for null and use ReferenceEquals for optimization
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
                return false;

            return obj1.Equals(obj2); // Defer to the Equals method for the actual comparison
        }

        // Overload != operator
        public static bool operator !=(VariableProperty obj1, VariableProperty obj2)
        {
            return !(obj1 == obj2); // Defer to the == operator
        }
    }
}
