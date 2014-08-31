﻿using NUnit.Framework;
using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Handlebars.Test
{
    [TestFixture]
    public class DynamicTests
    {
        [Test]
		public void DynamicObjectBasicTest()
        {
            var model = new MyDynamicModel();

            var source = "Foo: {{foo}}\nBar: {{bar}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("Foo: 1\nBar: hello world", output);
        }

		[Test]
		public void JsonTestIfTruthy()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\",\"truthy\":\"test2\"}");

			var source = "{{myfield}}{{#if truthy}}{{truthy}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1test2", output);
		}

		[Test]
		public void JsonTestIfFalsyMissingField()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\"}");

			var source = "{{myfield}}{{#if mymissingfield}}{{mymissingfield}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1", output);
		}

		[Test]
		public void JsonTestIfFalsyValue()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\",\"falsy\":null}");

			var source = "{{myfield}}{{#if falsy}}{{falsy}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1", output);
		}

        private class MyDynamicModel : DynamicObject
        {
            private Dictionary<string, object> properties = new Dictionary<string, object>
            {
                { "foo", 1 },
                { "bar", "hello world" }
            };

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if(properties.ContainsKey(binder.Name))
                {
                    result = properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
        }
    }
}

