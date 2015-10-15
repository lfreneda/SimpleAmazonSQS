namespace SimpleAmazonSQS.Tests
{
    public class FakeBody
    {
        public string FamilyName { get; set; }

        public string Birthname { get; set; }

        protected bool Equals(FakeBody other)
        {
            return string.Equals(FamilyName, other.FamilyName) && string.Equals(Birthname, other.Birthname);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((FakeBody) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((FamilyName?.GetHashCode() ?? 0)*397) ^ (Birthname?.GetHashCode() ?? 0);
            }
        }
    }
}