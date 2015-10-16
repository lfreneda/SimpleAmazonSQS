namespace SimpleAmazonSQS.Tests
{
    public class FakePerson
    {
        public string FamilyName { get; set; }

        public string Birthname { get; set; }

        public Gender PersonGender { get; set; }
        
        public enum Gender
        {
            Male = 1,
            Female = 2,
        }

        protected bool Equals(FakePerson other)
        {
            return string.Equals(FamilyName, other.FamilyName) && string.Equals(Birthname, other.Birthname) && PersonGender == other.PersonGender;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((FakePerson)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FamilyName?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Birthname?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (int)PersonGender;
                return hashCode;
            }
        }
    }
}