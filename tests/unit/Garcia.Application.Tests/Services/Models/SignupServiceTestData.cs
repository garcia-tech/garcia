using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Identity.Models.Request;

namespace Garcia.Application.Tests.Services.Models
{
    public class SignupServiceTestData : Credentials, IEnumerable<object[]>
    {
        public SignupServiceTestData()
        {
            Username = "testuser";
            Password = "pass123";
        }
        public string Email { get; set; } = "test@garcia.com.tr";

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new SignupServiceTestData()
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
