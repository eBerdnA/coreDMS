using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CoreDMS.TagHelpers;

namespace coreDMSTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var testValue = "2017-01-05 00:00:00.000 +00:00";
            var testFormat = "dd.MM.yyyy";
            var result = DateTagHelpers.GetFormattedDate(testValue, testFormat);
            var expected = DateTime.Parse(testValue).ToString(testFormat);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2()
        {
            string testValue = "2017-01-08 20:10:08.137 +00:00";
            string testFormat = "dd.MM.yyyy HH:mm:ss";
            var result = DateTagHelpers.GetFormattedDate(testValue, testFormat);
            var expected = DateTime.Parse(testValue).ToString(testFormat);
            Assert.Equal(expected, result);
        }
    }
}
