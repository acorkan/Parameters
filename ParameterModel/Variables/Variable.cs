using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParameterModel.Variables
{
    public class Variable 
    {
        public string Assignment { get; set; } = string.Empty;

        public override string ToString()
        {
            return (Assignment != null) ? Assignment : "";
        }

        public Variable(string assignment)
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

            Variable other = (Variable)obj;
            return Assignment == other.Assignment;
        }

        // Override GetHashCode()
        public override int GetHashCode()
        {
            return HashCode.Combine(Assignment);
        }

        // Overload == operator
        public static bool operator ==(Variable obj1, Variable obj2)
        {
            // Check for null and use ReferenceEquals for optimization
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
                return false;

            return obj1.Equals(obj2); // Defer to the Equals method for the actual comparison
        }

        // Overload != operator
        public static bool operator !=(Variable obj1, Variable obj2)
        {
            return !(obj1 == obj2); // Defer to the == operator
        }
    }
}
