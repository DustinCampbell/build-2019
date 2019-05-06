using System.Threading.Tasks;

namespace TollCollectorLib
{
    public static partial class TollSystem
    {

#nullable enable

        private static Task<Owner> LookupOwnerAsync(string state, string plate) => Task.FromResult(new Owner(state, plate));
        private static Task<Account?> LookupAccountAsync(string license) => Task.FromResult<Account?>(null);

        public class Owner
        {
            public Owner(string state, string plate) { }
            public void SendBill(decimal amount) { }
        }

        public class Account
        {
            public void Charge(decimal amount) { }
        }
    }
}
